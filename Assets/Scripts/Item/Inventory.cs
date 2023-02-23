using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




/// <summary>
/// 아이템이 변경된경우, <see cref="abilityCalculator"/>데이터의 갱신이 필요할 수 있다.
/// </summary>
public class Inventory : MonoBehaviour
{
	/*
	 * 자주 쓰는 아이템의경우는 캐싱을 해서 사용한다.
	 */
	public long GoldTid
	{
		get
		{
			if (goldTid == 0)
			{
				goldTid = DataManager.Get<ItemDataSheet>().GetByHashTag("gold").tid;
			}

			return goldTid;
		}
	}
	private long goldTid;

	public long DiaTid
	{
		get
		{
			if (diaTid == 0)
			{
				diaTid = DataManager.Get<ItemDataSheet>().GetByHashTag("dia").tid;
			}

			return diaTid;
		}
	}
	private long diaTid;






	public static Inventory Instance;
	public static Inventory it => Instance;

	private List<ItemBase> items = new List<ItemBase>();

	/// <summary>
	/// 수치 계산용으로 가져갈때 빼곤 접근하지 말아주세요.
	/// </summary>
	public List<ItemBase> Items => items;

	public InventoryAbilityCalculator abilityCalculator = new InventoryAbilityCalculator();

	public float refillCheckDeltaTime;




	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		refillCheckDeltaTime += Time.deltaTime;

		if (refillCheckDeltaTime < ConfigMeta.it.REFILL_UPDATE_CYCLE)
		{
			return;
		}

		refillCheckDeltaTime -= ConfigMeta.it.REFILL_UPDATE_CYCLE;

		foreach (var item in items)
		{
			if (item is ItemMoney)
			{
				ProcessRefill(item as ItemMoney);
			}
		}
	}

	public void Initialize(List<InstantItem> instantItems)
	{
		items.Clear();

		foreach (var item in instantItems)
		{
			ItemBase itemBase = ItemCreator.MakeItemBase(item.DeepClone(), out VResult vResult);
			if (vResult.Fail())
			{
				PopAlert.it.Create(vResult);
				continue;
			}

			items.Add(itemBase);
		}


		foreach (var item in items)
		{
			if (item is ItemMoney)
			{
				ProcessRefill(item as ItemMoney);
			}
		}

		abilityCalculator.CalculateAbilityAll(items);
	}

	public IdleNumber ItemCount(long _itemTid)
	{
		ItemBase itemBase = FindItemByTid(_itemTid);
		if (itemBase != null)
		{
			return itemBase.Count;
		}
		else
		{
			return new IdleNumber();
		}
	}

	public IdleNumber ItemCount(string _hashTag)
	{
		ItemBase itemBase = FindItemByHashTag(_hashTag);
		if (itemBase != null)
		{
			return itemBase.Count;
		}
		else
		{
			return new IdleNumber();
		}
	}

	public ItemBase FindItemByHashTag(string _hashTag)
	{
		foreach (var item in items)
		{
			if (item.data.hashTag == _hashTag)
			{
				return item;
			}
		}

		return null;
	}

	public List<ItemBase> FindItemsByType(ItemType _itemType)
	{
		List<ItemBase> resultItems = new List<ItemBase>();

		foreach (var item in items)
		{
			if (item.data.itemType == _itemType)
			{
				resultItems.Add(item);
			}
		}

		return resultItems;
	}

	public ItemBase FindItemByTid(Int64 _tid)
	{
		if (_tid == 0)
		{
			// 0인경우 명시적으로 처리
			return null;
		}

		for (int i = 0; i < items.Count; i++)
		{
			var item = items[i];
			if (item.Tid == _tid)
			{
				return item;
			}
		}

		return null;
	}

	public VResult CheckMoney(Int64 _tid, IdleNumber _count)
	{
		VResult vResult = new VResult();

		var item = FindItemByTid(_tid);

		if (item == null)
		{
			return vResult.SetFail(VResultCode.LACK_ITEM, "Item is Empty", _tidParams: _tid);
		}

		if (item.Count < _count)
		{
			return vResult.SetFail(VResultCode.LACK_ITEM, $"has: {item.Count}, need: {_count}", _tidParams: _tid);
		}

		return vResult.SetOk();
	}

	public VResult CheckMoney(string _hashTag, IdleNumber _count)
	{
		var itemData = DataManager.Get<ItemDataSheet>().GetByHashTag(_hashTag);
		if (itemData != null)
		{
			return CheckMoney(itemData.tid, _count);
		}
		else
		{
			return new VResult().SetFail(VResultCode.NO_META_DATA, $"invalid hashtag: {_hashTag}");
		}
	}

	public VResult ConsumeItem(Int64 _tid, IdleNumber _count)
	{
		VResult vResult = new VResult();

		var item = FindItemByTid(_tid);

		if (item == null)
		{
			return vResult.SetFail(VResultCode.LACK_ITEM, "Item is Empty", _tidParams: _tid);
		}

		if (item is ItemMoney)
		{
			// 최대개수보다 적어지는 순간에 리필 시간 갱신이 필요하다
			var itemMoney = (item as ItemMoney);
			if (itemMoney.Refillable && itemMoney.PrepareNextRefillUpdate(1, out string nextRefill))
			{
				itemMoney.SetNextRefill(nextRefill);
			}
		}

		if (item.Count < _count)
		{
			return vResult.SetFail(VResultCode.LACK_ITEM, $"has: {item.Count}, need: {_count}", _tidParams: _tid);
		}

		if (_count == 0)
		{
			return vResult.SetFail(VResultCode.INVALID_ITEM_CHANDE_0, _tidParams: _tid);
		}

		IdleNumber before = item.Count;
		item.Count -= _count;

		VLog.ItemLog($"[{item.ItemName}({item.Tid})] 소비 {before.ToString()} -> {item.Count.ToString()}. consume: {_count.ToString()}");

		EventCallbacks.CallItemChanged(new List<long>() { _tid });
		return vResult.SetOk();
	}

	public VResult ConsumeItem(string _hashTag, IdleNumber _count)
	{
		var itemData = DataManager.Get<ItemDataSheet>().GetByHashTag(_hashTag);
		if (itemData != null)
		{
			return ConsumeItem(itemData.tid, _count);
		}
		else
		{
			return new VResult().SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. invalid hashtag: {_hashTag}");
		}
	}

	public void AddItems(List<GachaResult> _resultItems)
	{
		List<long> changedItems = new List<long>();

		foreach (var resultItem in _resultItems)
		{
			VResult result = AddItem(resultItem.itemTid, resultItem.itemCount, false);
			if (result.Fail())
			{
				VLog.ItemLogError(result.ToString());
				continue;
			}

			if (changedItems.Contains(resultItem.itemTid) == false)
			{
				changedItems.Add(resultItem.itemTid);
			}
		}

		EventCallbacks.CallItemChanged(changedItems);
		abilityCalculator.CalculateAbilityAll(items);
	}

	public VResult AddItem(string _hashTag, IdleNumber _count, bool _sendEventAndUpdateData = true)
	{
		var itemData = DataManager.Get<ItemDataSheet>().GetByHashTag(_hashTag);

		if (itemData == null)
		{
			return new VResult().SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. invalid hashtag: {_hashTag}");
		}

		return AddItem(itemData.tid, _count, _sendEventAndUpdateData);
	}

	/// <summary>
	/// _sendEventAndUpdateData : 아이템 변화에 따른 데이터 자동 업데이트
	/// </summary>
	public VResult AddItem(long _tid, IdleNumber _count, bool _sendEventAndUpdateData = true)
	{
		var result = new VResult();
		ItemBase item = FindItemByTid(_tid);

		if (_count == 0)
		{
			return result.SetFail(VResultCode.INVALID_ITEM_CHANDE_0, _tidParams: _tid);
		}

		IdleNumber before;
		// 없으면 새로 생성
		if (item != null)
		{
			before = item.Count;
			item.Count += _count;
		}
		else
		{
			before = new IdleNumber(0);

			var itemData = DataManager.Get<ItemDataSheet>().Get(_tid);
			if (itemData == null)
			{
				return result.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. invalid tid: {_tid}");
			}

			var instantItem = ItemCreator.MakeInstantItem(itemData);
			item = ItemCreator.MakeItemBase(instantItem, out result);
			if (result.Fail())
			{
				return result;
			}

			item.Count += _count;
			items.Add(item);
		}

		VLog.ItemLog($"[{item.ItemName}({item.Tid})] 획득 {before.ToString()} -> {item.Count.ToString()}. added: {_count.ToString()}");

		if (_sendEventAndUpdateData)
		{
			EventCallbacks.CallItemChanged(new List<long>() { _tid });
			abilityCalculator.CalculateAbilityAll(items);
		}

		return result.SetOk();
	}

	/// <summary>
	///  리필형 아이템 체크
	/// </summary>
	private void ProcessRefill(ItemMoney _itemMoney)
	{
		_itemMoney.refill.ComputeAt = TimeManager.it.server_utc;

		RefillResult refillResult;
		bool is_refill = _itemMoney.ProcessUpdate(out refillResult);
		if (is_refill)
		{
			for (int i = 0; i < items.Count; i++)
			{
				var item = items[i];
				if (item.Tid == _itemMoney.Tid)
				{
					item.Count += refillResult.addCount;
					var itemMoney = item as ItemMoney;

					itemMoney.SetNextRefill(refillResult.nextRefill);
					itemMoney.SetNextReset(refillResult.nextReset);

					return;
				}
			}
		}
	}

	public void ResetProperty()
	{
		foreach (var propItem in FindItemsByType(ItemType.Property))
		{
			(propItem as ItemProperty).ResetLevel();
		}
	}

	public void ResetMastery()
	{
		foreach (var mastery in FindItemsByType(ItemType.Mastery))
		{
			(mastery as ItemMastery).ResetLevel();
		}
	}
}
