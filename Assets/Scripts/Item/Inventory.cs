using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
	public static Inventory Instance;
	public static Inventory it => Instance;

	private List<ItemBase> items = new List<ItemBase>();
	public InventoryAbilityCalculator abilityCalculator = new InventoryAbilityCalculator();

	public float refillCheckDeltaTime;


	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		refillCheckDeltaTime += Time.deltaTime;

		if(refillCheckDeltaTime < ConfigMeta.it.REFILL_UPDATE_CYCLE)
		{
			return;
		}

		refillCheckDeltaTime -= ConfigMeta.it.REFILL_UPDATE_CYCLE;

		foreach(var item in items)
		{
			if(item is ItemMoney)
			{
				ProcessRefill(item as ItemMoney);
			}
		}
	}

	public void Initialize(List<InstantItem> instantItems)
	{
		items.Clear();

		foreach(var item in instantItems)
		{
			ItemBase itemBase = ItemCreator.MakeItemBase(item, out VResult vResult);
			if(vResult.Fail())
			{
				PopAlert.it.Create(vResult);
				continue;
			}

			items.Add(itemBase);
		}

		abilityCalculator.CalculateAbility(items);
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
		if(itemBase != null)
		{
			return itemBase.Count;
		}
		else
		{
			return new IdleNumber();
		}
	}

	public long ItemTid(string _hashTag)
	{
		var itemData = DataManager.it.Get<ItemDataSheet>().GetByHashTag(_hashTag);
		if (itemData != null)
		{
			return itemData.tid;
		}
		else
		{
			return 0;
		}
	}

	public ItemBase FindItemByHashTag(string _hashTag)
	{
		foreach(var item in items)
		{
			if(item.data.hashTag == _hashTag)
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

		return null;
	}

	public ItemBase FindItemByTid(Int64 _tid)
	{
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

		if(item.Count < _count)
		{
			return vResult.SetFail(VResultCode.LACK_ITEM, $"has: {item.Count}, need: {_count}", _tidParams: _tid);
		}

		return vResult.SetOk();
	}

	public VResult CheckMoney(string _hashTag, IdleNumber _count)
	{
		var itemData = DataManager.it.Get<ItemDataSheet>().GetByHashTag(_hashTag);
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
			return vResult.SetFail(VResultCode.LACK_ITEM, "Item is Empty", _tidParams: _tid );
		}

		if(item is ItemMoney)
		{
			// 최대개수보다 적어지는 순간에 리필 시간 갱신이 필요하다
			var itemMoney = (item as ItemMoney);
			if(itemMoney.Refillable && itemMoney.PrepareNextRefillUpdate(1, out string nextRefill))
			{
				itemMoney.SetNextRefill(nextRefill);
			}
		}


		if (item.Count < _count)
		{
			return vResult.SetFail(VResultCode.LACK_ITEM, $"has: {item.Count}, need: {_count}", _tidParams: _tid);
		}

		item.Count -= _count;
		return vResult.SetOk();
	}

	public VResult ConsumeItem(string _hashTag, IdleNumber _count)
	{
		var itemData = DataManager.it.Get<ItemDataSheet>().GetByHashTag(_hashTag);
		if (itemData != null)
		{
			return ConsumeItem(itemData.tid, _count);
		}
		else
		{
			return new VResult().SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. invalid hashtag: {_hashTag}");
		}
	}

	public VResult AddItem(long _tid, long _count)
	{
		var vResult = new VResult();
		var item = FindItemByTid(_tid);

		// 없으면 새로 생성
		if (item == null)
		{
			var itemData = DataManager.it.Get<ItemDataSheet>().Get(_tid);
			if(itemData == null)
			{
				return vResult.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. invalid tid: {_tid}");
			}

			var instantItem = ItemCreator.MakeInstantItem(itemData);
			ItemBase itemBase = ItemCreator.MakeItemBase(instantItem, out vResult);
			if(vResult.Fail())
			{
				return vResult;
			}

			itemBase.Count += _count;
			items.Add(itemBase);

			return vResult.SetOk();
		}


		item.Count += _count;
		return vResult.SetOk();
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
			for (int i = 0 ; i < items.Count ; i++)
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
}

public class InventoryAbilityCalculator
{
	public class AbilityCalculator
	{
		private ItemType itemType;


		/// <summary>
		/// 장착시 제일 좋은 아이템
		/// </summary>
		public long bestEquipTid;
		/// <summary>
		/// 장착효과
		/// </summary>
		public AbilityInfo equipAbility;
		/// <summary>
		/// 보유 보너스
		/// </summary>
		public Dictionary<AbilityType, IdleNumber> toOwnAbilities = new Dictionary<AbilityType, IdleNumber>();


		public AbilityCalculator(ItemType _itemType)
		{
			itemType = _itemType;
		}

		public void Calculate(List<ItemBase> _items)
		{
			toOwnAbilities.Clear();
			ItemBase bestItem = null;
			ItemBase equipItem = null;
			Int64 equipTid = GetUserEquipItem(itemType);

			foreach (var item in _items)
			{
				if (item.Type != itemType)
				{
					continue;
				}

				// 보유효과 계산
				if (toOwnAbilities.ContainsKey(item.ToOwnAbility.type) == false)
				{
					toOwnAbilities.Add(item.ToOwnAbility.type, new IdleNumber());
				}

				toOwnAbilities[item.ToOwnAbility.type] += item.ToOwnAbility.value;


				// 장착시 제일 좋은 아이템 체크
				if (bestItem == null)
				{
					bestItem = item;
				}
				else if (bestItem.ToOwnAbility.value < item.ToOwnAbility.value)
				{
					bestItem = item;
				}

				// 장착중인 아이템이 있으면 캐싱
				if (item.Tid == equipTid)
				{
					equipItem = item;
				}
			}

			// 아이템을 아예 보유하지 않은경우 bestItem이 없을 수 있다
			if (bestItem != null)
			{
				bestEquipTid = bestItem.Tid;
			}

			// 장착 아이템 체크
			if (equipItem != null)
			{
				equipAbility = equipItem.EquipAbility;
			}
			else
			{
				equipAbility = new AbilityInfo();
			}
		}
	}

	public AbilityCalculator weapon = new AbilityCalculator(ItemType.Weapon);
	public AbilityCalculator armor = new AbilityCalculator(ItemType.Armor);
	public AbilityCalculator accessory = new AbilityCalculator(ItemType.Accessory);
	public AbilityCalculator relic = new AbilityCalculator(ItemType.Relic);

	public Dictionary<AbilityType, IdleNumber> abilityTotal = new Dictionary<AbilityType, IdleNumber>();


	public static long GetUserEquipItem(ItemType _itemType)
	{
		switch (_itemType)
		{
			case ItemType.Weapon:
				return UserInfo.EquipWeaponTid;
			case ItemType.Armor:
				return UserInfo.EquipArmorTid;
			case ItemType.Accessory:
				return UserInfo.EquipAccessoryTid;
			default:
				return 0;
		}
	}


	/// <summary>
	/// 효과가 변경됬을때 호출
	/// (최적화 하고 싶으면 타입별로 부분부분 호출후에 RefreshAbilityTotal() 갱신만 해주면 됨)
	/// </summary>
	public void CalculateAbility(List<ItemBase> _items)
	{
		weapon.Calculate(_items);
		armor.Calculate(_items);
		accessory.Calculate(_items);
		relic.Calculate(_items);

		RefreshAbilityTotal();
	}

	public void RefreshAbilityTotal()
	{
		abilityTotal.Clear();


		// 보유보너스 계산
		foreach(var abil in weapon.toOwnAbilities)
		{
			if(abilityTotal.ContainsKey(abil.Key) == false)
			{
				abilityTotal.Add(abil.Key, new IdleNumber());
			}

			abilityTotal[abil.Key] += abil.Value;
		}
		foreach (var abil in armor.toOwnAbilities)
		{
			if (abilityTotal.ContainsKey(abil.Key) == false)
			{
				abilityTotal.Add(abil.Key, new IdleNumber());
			}

			abilityTotal[abil.Key] += abil.Value;
		}
		foreach (var abil in accessory.toOwnAbilities)
		{
			if (abilityTotal.ContainsKey(abil.Key) == false)
			{
				abilityTotal.Add(abil.Key, new IdleNumber());
			}

			abilityTotal[abil.Key] += abil.Value;
		}
		foreach (var abil in relic.toOwnAbilities)
		{
			if (abilityTotal.ContainsKey(abil.Key) == false)
			{
				abilityTotal.Add(abil.Key, new IdleNumber());
			}

			abilityTotal[abil.Key] += abil.Value;
		}



		// 장착 보너스 계산
		if (weapon.equipAbility != null && weapon.equipAbility.type != AbilityType._NONE && abilityTotal.ContainsKey(weapon.equipAbility.type) == false)
		{
			abilityTotal[weapon.equipAbility.type] += weapon.equipAbility.value;
		}
		if (armor.equipAbility != null && armor.equipAbility.type != AbilityType._NONE && abilityTotal.ContainsKey(armor.equipAbility.type) == false)
		{
			abilityTotal[armor.equipAbility.type] += armor.equipAbility.value;
		}
		if (accessory.equipAbility != null && accessory.equipAbility.type != AbilityType._NONE && abilityTotal.ContainsKey(accessory.equipAbility.type) == false)
		{
			abilityTotal[accessory.equipAbility.type] += accessory.equipAbility.value;
		}
	}
}
