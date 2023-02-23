using System;
using System.Collections.Generic;

[Serializable]
public class GachaLevelData : BaseData
{
	public int level;
	public int nextExp;
}

[Serializable]
public class GachaLevelDataSheet : DataSheetBase<GachaLevelData>
{
	public GachaLevelData Get(long tid)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].tid == tid)
			{
				return infos[i];
			}
		}

		return null;
	}

	/// <summary>
	/// 현재 경험치에 맞는 레벨을 리턴
	/// </summary>
	public GachaLevelData GetByExp(long _exp)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].nextExp > _exp)
			{
				return infos[i];
			}
		}

		return infos[infos.Count - 1];
	}

	public GachaLevelData GetByLevel(int _level)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].level == _level)
			{
				return infos[i];
			}
		}

		return null;
	}

	public int MaxLevel()
	{
		return infos[infos.Count - 1].level;
	}

	/// <summary>
	/// 현재레벨에 맞게 계산된 보여지는 EXP
	/// </summary>
	public long CurrExp(long _totalExp)
	{
		var levelInfo = GetByExp(_totalExp);
		var beforeLevelInfo = GetByLevel(levelInfo.level - 1);

		if (beforeLevelInfo != null)
		{
			return _totalExp - beforeLevelInfo.nextExp;
		}
		else
		{
			return _totalExp;
		}
	}

	/// <summary>
	/// 현재레벨에 맞게 계산된 보여지는 다음레벨 EXP
	/// </summary>
	public long NextExp(long _totalExp)
	{
		var levelInfo = GetByExp(_totalExp);
		return levelInfo.nextExp;
	}
}
