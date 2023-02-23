using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMasteryData
{
	private static VResult _result = new VResult();

	private ItemData itemData;
	private UserMasteryData masteryData;

	public long ItemTid => itemData.tid;
	public string Icon => itemData.Icon;

	public string MasteryName => itemData.name;
	public int ConsumePoint => masteryData.consumePoint;

	public string PointText
	{
		get
		{
			var item = GetItem();
			if(item != null)
			{
				return $"{item.Level}/{item.MaxLevel}";
			}
			else
			{
				var data = DataManager.Get<UserMasteryDataSheet>().Get(masteryData.tid);
				if (data != null)
				{
					return $"0/{data.maxLevel}";
				}
				else
				{
					return $"0/0";
				}
			}
		}
	}
	public string MasteryAbilityText
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return $"{item.ToOwnAbility.GetValue(item.Level).ToString()} -> {item.ToOwnAbility.GetValue(item.Level + 1).ToString()}"; ;
			}
			else
			{
				return $"0 -> {DataManager.Get<ItemDataSheet>().Get(itemData.tid).ToOwnAbilityInfo.ToString()}";
			}
		}
	}

	public bool IsUnlock
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return item.IsUnlock();
			}
			else
			{
				return false;
			}
		}
	}



	public VResult Setup(UserMasteryData _masteryData)
	{
		masteryData = _masteryData;

		var itemSheet = DataManager.Get<ItemDataSheet>();

		itemData = itemSheet.GetByMasteryTid(_masteryData.tid);
		if (itemData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. masteryTid: {_masteryData.tid}");
		}

		return _result.SetOk();
	}

	public ItemMastery GetItem()
	{
		return Inventory.it.FindItemByTid(itemData.tid) as ItemMastery;
	}

	public bool Levelupable()
	{
		ItemMastery item = GetItem();

		if (item == null)
		{
			return false;
		}

		if (item.Levelupable() == false)
		{
			return false;
		}


		if (UserInfo.RemainMasteryPoint < ConsumePoint)
		{
			return false;
		}

		return true;
	}

	public void LevelupMastery(Action onLevelupSuccess = null)
	{
		if (Levelupable() == false)
		{
			return;
		}

		// 레벨이 오르면 남은 특성포인트는 자동으로 계산됨
		ItemMastery item = GetItem();
		item.AddLevel(1);

		onLevelupSuccess?.Invoke();
	}
}
