using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipData
{
	private static VResult _result = new VResult();

	public ItemData itemData;

	public long ItemTid => itemData.tid;
	public string Icon => itemData.Icon; 
	public long LevelupConsumeTid => ItemTid;

	public int ItemLevel
	{ 
		get
		{
			var item = GetItem();
			if(item != null)
			{
				return item.Level;
			}
			else
			{
				return 0;
			}
		}
	}
	public string ItemLevelText => ItemLevel.ToString();
	public float ExpRatio
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return item.expRatio;
			}
			else
			{
				return 0;
			}
		}
	}
	public string ExpText
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				if (item.IsMaxLv)
				{
					return $"Max";
				}
				else
				{
					return $"{item.Exp.ToString("N0")} / {item.nextExp.ToString("N0")}";
				}
			}
			else
			{
				return "미보유";
			}
		}
	}



	public VResult Setup(ItemData _itemData)
	{
		itemData = _itemData;
		return _result.SetOk();
	}

	public ItemEquip GetItem()
	{
		return Inventory.it.FindItemByTid(ItemTid) as ItemEquip;
	}
}
