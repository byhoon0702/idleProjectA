
using System;

[Serializable]
public class UserLevelData : BaseData
{
	public Int32 level;
	public Int32 nextExp;
	public Int32 propertyPoint;
}

[Serializable]
public class UserLevelDataSheet : DataSheetBase<UserLevelData>
{
	public UserLevelData Get(long tid)
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
}
