using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTraining : ItemBase
{
	public UserTrainingData trainingData;

	public override int MaxLevel => trainingData.maxLevel;




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

		trainingData = DataManager.Get<UserTrainingDataSheet>().Get(data.userTrainingTid);

		if (trainingData == null)
		{
			return vResult.SetFail(VResultCode.NO_META_DATA, $"UserTrainingDataSheet. tid: {data.tid}, userTrainingTid: {data.userTrainingTid}");
		}

		return vResult.SetOk();
	}

	public bool Levelupable()
	{
		return IsMaxLv == false;
	}

	public override string ToString()
	{
		return $"[{ItemName}({Tid})] -  [{Grade}], lv: {Level})";
	}
}
