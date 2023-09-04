using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UserLevelData : BaseData
{
	public int level;
	public int baseExp;
	public int expIncrement;
	public float firstWeight;
	public float secondWeight;
}

[System.Serializable]
public class UserLevelDataSheet : DataSheetBase<UserLevelData>
{

	public UserLevelData GetDataByLevel(int level)
	{
		for (int i = infos.Count - 1; i >= 0; i--)
		{
			if (infos[i].level <= level)
			{
				return infos[i];
			}
		}

		return null;
	}
}
