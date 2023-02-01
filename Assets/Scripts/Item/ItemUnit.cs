using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUnit : ItemBase
{
	public UnitLevelData levelData;

	public int nextExp
	{
		get
		{
			var sheet = DataManager.it.Get<UnitLevelDataSheet>();
			UnitLevelData levelData;

			if (sheet.MaxLv <= Level)
			{
				levelData = sheet.GetByLevel(sheet.MaxLv);
			}
			else
			{
				levelData = sheet.GetByLevel(Level + 1);
			}

			return levelData.needCount;
		}
	}

	public float expRatio
	{
		get
		{
			float outValue = Mathf.Clamp((float)Exp / nextExp, 0, 1);
			return outValue;
		}
	}

	public override int MaxLevel => DataManager.it.Get<UnitLevelDataSheet>().MaxLv;


	public override VResult Setup(InstantItem _instantItem)
	{
		VResult vResult = base.Setup(_instantItem);
		if(vResult.Fail())
		{
			return vResult;
		}

		vResult = SetupMetaData();
		if (vResult.Fail())
		{
			return vResult;
		}

		return vResult;
	}

	private VResult SetupMetaData()
	{
		VResult vResult = new VResult();
		var sheet = DataManager.it.Get<UnitLevelDataSheet>();

		if (sheet.MaxLv <= Level)
		{
			levelData = sheet.GetByLevel(sheet.MaxLv);
		}
		else
		{
			levelData = sheet.GetByLevel(Level + 1);
		}

		if(levelData == null)
		{
			return vResult.SetFail(VResultCode.NO_META_DATA, $"UnitLevelDataSheet. tid: {data.tid}, lv: {Level}, maxlv: {MaxLevel}");
		}

		return vResult.SetOk();
	}

	public override void AddExp(int _exp)
	{
		base.AddExp(_exp);

		while (nextExp <= Exp)
		{
			if(Level >= MaxLevel)
			{
				break;
			}


			instantItem.exp -= nextExp;
			instantItem.level++;

			VResult result = SetupMetaData();
			if(result.Fail())
			{
				PopAlert.it.Create(result);
				return;
			}
		}
	}


	public override string ToString()
	{
		return $"[{ItemName}({Tid})] -  [{Grade}], lv: {Level}, {Exp}/{nextExp}({expRatio})";
	}
}
