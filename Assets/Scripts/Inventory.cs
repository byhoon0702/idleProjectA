using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
	public static Inventory Instance;
	public static Inventory it => Instance;

	private void Awake()
	{
		Instance = this;

		Init();
	}

	private List<ItemBase> listItems = new List<ItemBase>();
	private List<ItemBase> moneyItemList = new List<ItemBase>();

	private ItemBase goldItemBase;

	public ItemBase GoldItem => goldItemBase;
	private ItemBase lightDust; // 진급 재화
	public ItemBase LightDust => lightDust;

	public void Init()
	{
		ItemBase gold_item = new ItemBase();
		gold_item.tid = 1;
		gold_item.count = new IdleNumber(9000, 0);

		ItemBase light_dust = new ItemBase();
		light_dust.tid = 2;
		light_dust.count = new IdleNumber(9000, 0);

		var relicItems = MakeRelicItem();
		foreach (var item in relicItems)
		{
			UpdateMoneyItem(item);
		}
		UpdateMoneyItem(gold_item);
		UpdateMoneyItem(light_dust);


		goldItemBase = gold_item;
		lightDust = light_dust;
	}

	public void UpdateMoneyItem(ItemBase _ItemBase)
	{
		var item = FindItemByTid(_ItemBase.tid);
		if (item == null)
		{
			listItems.Add(_ItemBase);
		}
		else
		{
			int index = listItems.IndexOf(item);
			listItems.RemoveAt(index);
			listItems[index] = _ItemBase;
		}
	}

	public ItemBase FindItemByTid(Int64 _tid)
	{
		for (int i = 0; i < listItems.Count; i++)
		{
			var item = listItems[i];
			if (item.tid == _tid)
			{
				return item;
			}
		}
		return null;
	}

	public bool CheckMoney(Int64 _tid, IdleNumber _price)
	{
		var item = FindItemByTid(_tid);
		if (item == null)
		{
			return false;
		}
		return item.count >= _price;
	}

	public bool ConsumeItem(Int64 _tid, IdleNumber _price)
	{
		var item = FindItemByTid(_tid);

		if (item == null)
		{
			return false;
		}
		if (item.count < _price)
		{
			return false;
		}

		item.count -= _price;
		return true;
	}

	private List<ItemBase> MakeRelicItem()
	{
		List<ItemBase> data = new List<ItemBase>();

		ItemBase item1 = new ItemBase();
		item1.tid = 111;
		item1.count = new IdleNumber(10);

		data.Add(item1);

		ItemBase item2 = new ItemBase();
		item2.tid = 112;
		item2.count = new IdleNumber(10);

		data.Add(item2);

		ItemBase item3 = new ItemBase();
		item3.tid = 113;
		item3.count = new IdleNumber(10);

		data.Add(item3);

		ItemBase item4 = new ItemBase();
		item4.tid = 114;
		item4.count = new IdleNumber(10);

		data.Add(item4);

		ItemBase item5 = new ItemBase();
		item5.tid = 115;
		item5.count = new IdleNumber(10);

		data.Add(item5);

		ItemBase item6 = new ItemBase();
		item6.tid = 116;
		item6.count = new IdleNumber(10);

		data.Add(item6);

		ItemBase item7 = new ItemBase();
		item7.tid = 117;
		item7.count = new IdleNumber(10);

		data.Add(item7);

		ItemBase item8 = new ItemBase();
		item8.tid = 118;
		item8.count = new IdleNumber(10);

		data.Add(item8);

		ItemBase item9 = new ItemBase();
		item9.tid = 119;
		item9.count = new IdleNumber(10);

		data.Add(item9);

		ItemBase item10 = new ItemBase();
		item10.tid = 120;
		item10.count = new IdleNumber(10);

		data.Add(item10);

		ItemBase item11 = new ItemBase();
		item11.tid = 121;
		item11.count = new IdleNumber(10);

		data.Add(item11);

		ItemBase item12 = new ItemBase();
		item12.tid = 122;
		item12.count = new IdleNumber(10);

		data.Add(item12);

		ItemBase item13 = new ItemBase();
		item13.tid = 123;
		item13.count = new IdleNumber(10);

		data.Add(item13);

		ItemBase item14 = new ItemBase();
		item14.tid = 124;
		item14.count = new IdleNumber(10);

		data.Add(item14);

		ItemBase item15 = new ItemBase();
		item15.tid = 125;
		item15.count = new IdleNumber(10);

		data.Add(item15);

		return data;
	}
}
