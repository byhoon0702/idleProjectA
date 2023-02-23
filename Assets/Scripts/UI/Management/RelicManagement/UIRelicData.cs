using System;
using System.Collections;
using System.Collections.Generic;



public class UIRelicData
{
	private static VResult _result = new VResult();

	public ItemData itemData;

	public long ItemTid => itemData.tid;
	public string Icon => itemData.Icon;
	public string ItemName => $"{itemData.name} LV. {ItemLevel}";
	
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
				return 0;
			}
		}
	}
	public long LevelupConsumeTid => ItemTid;
	public IdleNumber LevelupConsumeCount
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return new IdleNumber(item.NextExp);
			}
			else
			{
				return new IdleNumber(0);
			}
		}
	}

	public string CurrentStatText
	{
		get
		{
			var item = GetItem();

			if (item != null)
			{
				return $"{item.ToOwnAbility.type} {item.ToOwnAbility.GetValue(item.Level).ToString()}";
			}
			else
			{
				return $"{itemData.ToOwnAbilityInfo.type} 0";
			}
		}
	}

	public string NextStatText
	{
		get
		{
			var item = GetItem();

			if (item != null)
			{
				if (item.IsMaxLv)
				{
					return "MAX";
				}
				else
				{
					return $"{item.ToOwnAbility.type} {item.ToOwnAbility.GetValue(item.Level + 1).ToString()}";
				}
			}
			else
			{
				return $"{itemData.ToOwnAbilityInfo.type} {itemData.ToOwnAbilityInfo.value}";
			}
		}
	}

	public IdleNumber LevelupCost
	{
		get
		{
			var item = GetItem();

			if (item != null)
			{
				return new IdleNumber(item.NextExp);
			}
			else
			{
				return new IdleNumber(0);
			}
		}
	}
	public string LevelupCostText => LevelupCost.ToString();



	public VResult Setup(ItemData _itemData)
	{
		itemData = _itemData;
		return _result.SetOk();
	}

	public ItemRelic GetItem()
	{
		return Inventory.it.FindItemByTid(ItemTid) as ItemRelic;
	}

	public bool Levelupable()
	{
		var item = GetItem();
		if (item == null || item.Count == 0)
		{
			return false;
		}

		if (item.Levelupable() == false)
		{
			return false;
		}

		if (Inventory.it.CheckMoney(LevelupConsumeTid, LevelupConsumeCount).Fail())
		{
			return false;
		}

		return true;
	}

	public void LevelupItem(Action onLevelupSuccess)
	{

		if (Levelupable())
		{
			if (Inventory.it.ConsumeItem(LevelupConsumeTid, LevelupConsumeCount).Ok())
			{
				var item = GetItem();
				item.AddLevel(1);
				onLevelupSuccess?.Invoke();
			}
		}
	}
}
