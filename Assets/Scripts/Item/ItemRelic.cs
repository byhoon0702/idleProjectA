using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRelic : ItemBase
{
	public ItemRelicData relicData;

	public override int MaxLevel => relicData.maxLevel;
	public int LevelupConsumeCount => relicData.consumePoint;


	public override int Exp
	{
		get
		{
			int itemCount = Inventory.it.ItemCount(Tid).GetValueToInt();
			return itemCount;
		}
	}


	public int NextExp
	{
		get
		{
			var sheet = DataManager.it.Get<ItemRelicDataSheet>();
			ItemRelicData levelData = sheet.Get(data.relicLinkTid);

			return levelData.consumePoint;
		}
	}

	public override VResult Setup(InstantItem _instantItem)
	{
		VResult vResult = base.Setup(_instantItem);

		if(vResult.Fail())
		{
			return vResult;
		}

		var sheet = DataManager.it.Get<ItemRelicDataSheet>();

		relicData = sheet.Get(data.relicLinkTid);

		if (relicData == null)
		{
			return vResult.SetFail(VResultCode.NO_META_DATA, $"ItemRelicDataSheet. tid: {data.tid}, relicLinkTid: {data.relicLinkTid}");
		}

		return vResult;
	}

	/// <summary>
	/// 유물 레벨업 확률
	/// </summary>
	public float GetLevelupRatio(int _level)
	{
		return relicData.startUpgradeRatio - (_level * relicData.decreaseSuccessRatio);
	}

	/// <summary>
	/// 유물 레벨업 확률(현재레벨)
	/// </summary>
	public float GetCurrentLevelupRatio()
	{
		return GetLevelupRatio(Level);
	}

	/// <summary>
	/// 레벨업 시도. 소비는 별도 처리필요
	/// </summary>
	public bool TryLevelUp()
	{
		float ratio = GetCurrentLevelupRatio();
		bool result = SkillUtility.Cumulative(ratio);

		if(result)
		{
			AddLevel(1);
		}

		return result;
	}
	public override string ToString()
	{
		return $"[{ItemName}({Tid})] -  [{Grade}], lv: {Level}, {Exp}/{NextExp}";
	}
}
