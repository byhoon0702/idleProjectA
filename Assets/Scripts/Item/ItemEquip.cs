using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquip : ItemBase
{
	public EquipLevelData levelData;


	public override int Exp
	{
		get
		{
			int itemCount = Inventory.it.ItemCount(Tid).GetValueToInt();
			return itemCount;
		}
	}

	public int nextExp => levelData.needCount;

	public float expRatio
	{
		get
		{
			float outValue = Mathf.Clamp((float)Exp / nextExp, 0, 1);
			return outValue;
		}
	}

	public override VResult Setup(InstantItem _instantItem)
	{
		VResult vResult = base.Setup(_instantItem);
		if(vResult.Fail())
		{
			return vResult;
		}

		vResult = SetupMetaData();
		if(vResult.Fail())
		{
			return vResult;
		}

		return vResult.SetOk();
	}

	private VResult SetupMetaData()
	{
		VResult vResult = new VResult();

		var sheet = DataManager.it.Get<EquipLevelDataSheet>();
		levelData = sheet.FindLevelInfo(Level);

		if (levelData == null)
		{
			return vResult.SetFail(VResultCode.NO_META_DATA, $"EquipLevelDataSheet. tid: {data.tid}, lv: {Level}");
		}

		return vResult.SetOk();
	}

	public bool Levelupable()
	{
		return nextExp <= Exp;
	}

	public override void AddLevel(int _level)
	{
		base.AddLevel(_level);

		VResult vResult = SetupMetaData();
		if(vResult.Fail())
		{
			PopAlert.it.Create(vResult);
		}
	}

	public override string ToString()
	{
		return $"[{ItemName}({Tid})] -  [{Grade}], lv: {Level}, {Exp}/{nextExp}({expRatio})";
	}
}
