using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBufsf : ItemBase
{
	public UserBuffData buffData;

	public override int MaxLevel => buffData.maxLevel;
	public override int Level => UserDataCalculator.GetBuffLevelInfoByExp(Exp).level;
	public BuffExpInfo levelInfo => UserDataCalculator.GetBuffLevelInfoByExp(Exp);

	public override VResult Setup(InstantItem _instantItem)
	{
		VResult result = new VResult();
		instantItem = _instantItem;

		data = DataManager.Get<BuffItemDataSheet>().Get(_instantItem.tid);
		if (data == null)
		{
			return result.SetFail(VResultCode.NO_META_DATA);
		}

		result = SetupMetaData();
		if (result.Fail())
		{
			return result;
		}

		return result.SetOk();
	}

	private VResult SetupMetaData()
	{
		VResult vResult = new VResult();

		BuffItemData buffItem = data as BuffItemData;
		buffData = DataManager.Get<UserBuffDataSheet>().Get(buffItem.userBuffTid);

		if (buffData == null)
		{
			return vResult.SetFail(VResultCode.NO_META_DATA, $"UserTrainingDataSheet. tid: {data.tid}, userTrainingTid: {buffItem.userBuffTid}");
		}

		return vResult.SetOk();
	}

	public bool Levelupable()
	{
		return IsMaxLv == false;
	}
}
