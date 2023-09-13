using System;
using System.Collections.Generic;

public enum GachaType
{
	Equip = 1,
	Skill = 2,
	Pet = 3,
	Relic = 4,
}

public enum GachaButtonType
{
	Gacha10 = 0,
	Gacha100 = 1,
	Ads = 2,
	Ticket10 = 3,
	Ticket100 = 4,
}
[Serializable]
public class GachaItem
{
	public RewardCategory category;
	public long tid;
	public int chance;
}

[Serializable]
public class GachaChanceInfo
{
	public Grade grade;
	public List<int> chances;

}

[Serializable]
public class GachaLevelInfo
{
	public int level;
	public int exp;

	public ChanceReward reward;
	public GachaLevelInfo()
	{
		level = 0;
		exp = 0;
		reward = null;
	}
}

[Serializable]
public class GachaDataSummonInfo
{
	/// <summary>
	/// 소환타입
	/// </summary>
	public GachaButtonType summonType;

	/// <summary>
	/// 기본 소환개수
	/// </summary>
	public int defaultCount;
	/// <summary>
	/// 소환 증가값
	/// </summary>
	public int addCount;
	/// <summary>
	/// 최대 소환개수
	/// </summary>
	public int maxCount;

	/// <summary>
	/// 소환에 필요한 재화
	/// </summary>
	public long itemTid;

	/// <summary>
	/// 소환비용
	/// </summary>
	public int cost;

	/// <summary>
	/// 소환 최대 횟수
	/// </summary>
	public int summonMaxCount;
	/// <summary>
	/// 쿨타임
	/// </summary>
	public float cooltime;
}

[Serializable]
public class GachaData : BaseData
{

	public GachaType gachaType;
	public GachaDataSummonInfo[] summonInfos;
	public List<GachaLevelInfo> gachaLevelInfos;

	public List<GachaChanceInfo> chances;


}

[Serializable]
public class GachaDataSheet : DataSheetBase<GachaData>
{
	public GachaData GetByType(GachaType _type)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].gachaType == _type)
			{
				return infos[i];
			}
		}

		return null;
	}

	public List<GachaData> GetDataByType(GachaType type)
	{
		return infos.FindAll(x => x.gachaType == type);
	}
}

