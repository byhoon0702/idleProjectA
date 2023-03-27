using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIBuffData
{
	private static VResult _result = new VResult();

	public ItemData itemData;
	public UserBuffData userBuffData;

	public long ItemTid => itemData.tid;
	public long UserBuffTid => userBuffData.tid;

	public string TitleText
	{
		get
		{
			var item = GetItem();
			return $"{userBuffData.name} LV.{item.Level}";
		}
	}
	public long currentExp
	{
		get
		{
			var item = GetItem();
			return item.Exp - item.levelInfo.beginExp;
		}
	}
	public long maxExp
	{
		get
		{
			var item = GetItem();
			return item.levelInfo.nextExp - item.levelInfo.beginExp;
		}
	}

	public VResult Setup(ItemData _itemData, UserBuffData _buffData)
	{
		itemData = _itemData;
		userBuffData = _buffData;

		return _result.SetOk();
	}

	public VResult Setup(ItemData _itemData)
	{
		BuffItemData buffItem = _itemData as BuffItemData;
		var buffData = DataManager.Get<UserBuffDataSheet>().Get(buffItem.userBuffTid);

		if (buffData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"UserTrainingDataSheet. itemTid: {_itemData.tid}, userTrainingTid: {buffItem.userBuffTid}");
		}

		return Setup(_itemData, buffData);
	}

	public ItemBufsf GetItem()
	{
		return Inventory.it.FindItemByTid(itemData.tid) as ItemBufsf;
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

		// dltmdduq1118 2023/03/07
		// 레벨업 조건 체크

		return true;
	}

	public void BuffExpUp(Action onSuccess = null)
	{
		bool levelupable = Levelupable();
		if (levelupable == false)
		{
			return;
		}

		var item = GetItem();
		item.AddExp(1);
		onSuccess?.Invoke();
	}
}
