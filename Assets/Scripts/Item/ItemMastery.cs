using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMastery : ItemBase
{
	public UserMasteryData masteryData;

	public override int MaxLevel => masteryData.maxLevel;






	public override VResult Setup(InstantItem _instantItem)
	{
		VResult vResult = base.Setup(_instantItem);

		if (vResult.Fail())
		{
			return vResult;
		}

		vResult = SetupMetaData();
		if (vResult.Fail())
		{
			return vResult;
		}

		return vResult.SetOk();
	}


	private VResult SetupMetaData()
	{
		VResult vResult = new VResult();

		masteryData = DataManager.Get<UserMasteryDataSheet>().Get(data.userMasteryTid);

		if (masteryData == null)
		{
			return vResult.SetFail(VResultCode.NO_META_DATA, $"UserMasteryDataSheet. tid: {data.tid}, userMasteryTid: {data.userMasteryTid}");
		}

		return vResult.SetOk();
	}

	public bool IsUnlock()
	{
		if(masteryData.preTid == 0)
		{
			return true;
		}


		ItemData prevItemData = DataManager.Get<ItemDataSheet>().GetByMasteryTid(masteryData.preTid);
		if(prevItemData == null)
		{
			return false;
		}
		
		ItemMastery preItem = Inventory.it.FindItemByTid(prevItemData.tid) as ItemMastery;
		if(preItem == null)
		{
			return false;
		}

		return preItem.IsMaxLv;
	}

	public bool Levelupable()
	{
		return IsUnlock() && IsMaxLv == false;
	}

	public void ResetLevel()
	{
		instantItem.level = 0;
	}

	/// <summary>
	/// 사용중인 마스터리 포인트
	/// </summary>
	/// <returns></returns>
	public int GetUsingMasteryPoint()
	{
		int total = masteryData.consumePoint * Level;

		return total;
	}

	public override string ToString()
	{
		return $"[{ItemName}({Tid})], lv: {Level})";
	}
}
