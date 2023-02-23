using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSkill : ItemBase
{
	public EquipLevelData levelData;
	public SkillData skillData;


	public override int Exp
	{
		get
		{
			int itemCount = Inventory.it.ItemCount(Tid).GetValueToInt() - 1;// 1개는 보유해야함
			return itemCount;
		}
	}

	public override string Icon => skillData.Icon;

	public int NextExp => levelData.needCount;

	public float expRatio
	{
		get
		{
			float outValue = Mathf.Clamp((float)Exp / NextExp, 0, 1);
			return outValue;
		}
	}

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

		var equipLevelSheet = DataManager.Get<EquipLevelDataSheet>();
		levelData = equipLevelSheet.FindLevelInfo(Level);

		if (levelData == null)
		{
			return vResult.SetFail(VResultCode.NO_META_DATA, $"EquipLevelDataSheet. tid: {data.tid}, lv: {Level}");
		}

		var skillSheet = DataManager.Get<SkillDataSheet>();
		skillData = skillSheet.Get(data.skillTid);
		if(skillData == null)
		{
			return vResult.SetFail(VResultCode.NO_META_DATA, $"SkillDataSheet. itemtid: {data.tid}, skillTid: {data.skillTid}");
		}

		return vResult.SetOk();
	}

	public bool Levelupable()
	{
		return NextExp <= Exp;
	}

	public override void AddLevel(int _level)
	{
		base.AddLevel(_level);

		VResult vResult = SetupMetaData();
		if (vResult.Fail())
		{
			PopAlert.it.Create(vResult);
		}
	}

	public override string ToString()
	{
		return $"[{ItemName}({Tid})] -  [{Grade}], lv: {Level}, {Exp}/{NextExp}({expRatio})";
	}
}
