using System;
using System.Collections.Generic;

[Serializable]
public class UnitLevelData : BaseData
{
	public Int32 level;
	public Int32 needCount;
}

[Serializable]
public class UnitLevelDataSheet : DataSheetBase<UnitLevelData>
{
	private int maxLevel;
	public int MaxLv
	{
		get
		{
			if (maxLevel == 0)
			{
				foreach (var lvData in infos)
				{
					maxLevel = UnityEngine.Mathf.Max(maxLevel, lvData.level);
				}
			}

			return maxLevel;
		}
	}



	public UnitLevelData GetByLevel(Int32 _level)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].level == _level)
			{
				return infos[i];
			}
		}

		return null;
	}
}
