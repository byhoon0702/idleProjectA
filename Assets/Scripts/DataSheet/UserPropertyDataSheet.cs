using System;
using System.Collections.Generic;

[Serializable]
public class UserVeterancyData : BaseData
{
	public Int32 maxLevel;
	public Int32 consumePoint;
}


[Serializable]
public class UserPropertyDataSheet : DataSheetBase<UserVeterancyData>
{

	/// <summary>
	/// 특성 레벨업 비용
	/// </summary>
	public Int32 LevelupConsume(long _userPropertyTid)
	{
		var consumeInfo = DataManager.Get<UserPropertyDataSheet>().Get(_userPropertyTid);

		return consumeInfo.consumePoint;
	}
}
