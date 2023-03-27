using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVeterancyData
{
	private static VResult _result = new VResult();

	private VeterancyItemData itemData;
	private UserVeterancyData veterancyData;
	public long ItemTid => itemData.tid;
	public string Icon => itemData.Icon;
	public int ItemLevel
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return item.Level;
			}
			else
			{
				return 1;
			}
		}
	}
	public string ItemName => $"{itemData.name} LV.{ItemLevel}";
	public int LevelupConsumeCount
	{
		get
		{
			var sheet = DataManager.Get<UserPropertyDataSheet>();

			int outCount = sheet.LevelupConsume(itemData.userPropertyTid);
			return outCount;
		}
	}
	public string CostText => LevelupConsumeCount.ToString("N0");

	public string CurrentStatText
	{
		get
		{
			var item = GetItem();
			if (item == null)
			{
				return $"0";
			}
			else
			{
				return $"{item.ToOwnAbility.GetValue(item.Level).ToString()}";
			}
		}
	}

	public string NextStatText
	{
		get
		{
			var item = GetItem();
			if (item == null)
			{
				return $"{itemData.OwnAbilityInfo.GetValue(1).ToString()}";
			}
			else
			{
				return $"{item.ToOwnAbility.GetValue(item.Level + 1).ToString()}";
			}
		}
	}



	public VResult Setup(long _itemTid)
	{
		var sheet = DataManager.Get<PropertyItemDataSheet>();
		VeterancyItemData itemData = sheet.Get(_itemTid);
		if (itemData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. itemTid: {_itemTid}");
		}

		return Setup(itemData);
	}

	public VResult Setup(VeterancyItemData _itemData)
	{
		var sheet = DataManager.Get<UserPropertyDataSheet>();
		var propertyData = sheet.Get(_itemData.userPropertyTid);

		if (propertyData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"UserPropertyDataSheet. itemTid: {_itemData.tid}, skillTid: {_itemData.userPropertyTid}");
		}

		return Setup(_itemData, propertyData);
	}

	public VResult Setup(VeterancyItemData _itemData, UserVeterancyData _propertyData)
	{
		itemData = _itemData;
		veterancyData = _propertyData;

		return _result.SetOk();
	}

	public ItemProperty GetItem()
	{
		return Inventory.it.FindItemByTid(itemData.tid) as ItemProperty;
	}

	public bool Levelupable()
	{
		var sheet = DataManager.Get<UserPropertyDataSheet>();
		var item = GetItem();
		if (item == null || item.Count == 0)
		{
			return false;
		}

		if (UserInfo.info.RemainPropertyPoint < LevelupConsumeCount)
		{
			return false;
		}

		return true;
	}

	public void LevelupItem(Action onLevelupSuccess = null)
	{
		if (Levelupable() == false)
		{
			return;
		}


		// 레벨이 오르면 남은 특성포인트는 자동으로 계산되니 별도 처리필요 없음
		var item = GetItem();
		item.AddLevel(1);

		onLevelupSuccess?.Invoke();
	}
}
