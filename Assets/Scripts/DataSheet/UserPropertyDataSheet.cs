using System;
using System.Collections.Generic;

[Serializable]
public class UserPropertyData : BaseData
{
	public Int32 maxLevel;
	public Int32 consumePoint;
}


[Serializable]
public class UserPropertyDataSheet : DataSheetBase<UserPropertyData>
{
	public UserPropertyData Get(long tid)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == tid)
			{
				return infos[i];
			}
		}
		return null;
	}

	/// <summary>
	/// 특성 레벨업 비용
	/// </summary>
	public Int32 LevelupConsume(long _userPropertyTid)
	{
		var consumeInfo = DataManager.Get<UserPropertyDataSheet>().Get(_userPropertyTid);

		return consumeInfo.consumePoint;
	}
}
