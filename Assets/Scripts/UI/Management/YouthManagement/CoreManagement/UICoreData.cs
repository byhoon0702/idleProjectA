using System;
using System.Collections;
using System.Collections.Generic;

public class UICoreData
{
	private static VResult _result = new VResult();

	private ItemData itemData;
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

	//장비류만 For문 돌려야함
	public string NextStatText
	{
		get
		{
			var item = GetItem();
			//if (item == null)
			//{
			//	return $"{itemData.ToOwnAbilityInfo.value.ToString()}";
			//}
			//else
			{
				return $"{item.ToOwnAbility.GetValue(item.Level + 1).ToString()}";
			}
		}
	}
	public string LevelupConsumeHashTag => "corepoint";
	public IdleNumber LevelupConsumeCount
	{
		get
		{
			var item = GetItem();
			var sheet = DataManager.Get<UserGradeDataSheet>();
			if (item == null)
			{
				return sheet.CoreLevelupConsume(0);
			}
			else
			{
				return sheet.CoreLevelupConsume(item.Level);
			}
		}
	}






	public VResult Setup(ItemData _itemData)
	{
		itemData = _itemData;
		return _result.SetOk();
	}

	public ItemCore GetItem()
	{
		return Inventory.it.FindItemByTid(itemData.tid) as ItemCore;
	}

	public bool Levelupable()
	{
		var sheet = DataManager.Get<UserGradeDataSheet>();
		var item = GetItem();
		if (item == null || item.Count == 0)
		{
			return false;
		}

		if (item.Levelupable() == false)
		{
			return false;
		}

		if (Inventory.it.CheckMoney(LevelupConsumeHashTag, LevelupConsumeCount).Fail())
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

		var item = GetItem();
		if (Inventory.it.ConsumeItem(LevelupConsumeHashTag, LevelupConsumeCount).Ok())
		{
			item.AddLevel(1);
			onLevelupSuccess?.Invoke();
		}
	}
}
