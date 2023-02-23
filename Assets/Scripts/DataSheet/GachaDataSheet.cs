using System;
using System.Collections.Generic;

[Serializable]
public class GachaData : BaseData
{
	public GachaType gachaType;
	public GachaDataSummonInfo[] summonInfos;
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
	/// 소환 증가값(광고는 11회 ~ 35회까지 증가한다)
	/// </summary>
	public int addCount;
	/// <summary>
	/// 최대 소환개수
	/// </summary>
	public int maxCount;

	/// <summary>
	/// 소환에 필요한 재화
	/// </summary>
	public long itemTidSummon;

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

	public long gachaEntryTid;
}

[Serializable]
public class GachaDataSheet : DataSheetBase<GachaData>
{
	public GachaData Get(long tid)
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

	public GachaData GetByType(GachaType _type)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].gachaType == _type)
			{
				return infos[i];
			}
		}

		return null;
	}
}

public enum GachaType
{
	Equip,
	Skill,
	Pet
}
public enum GachaButtonType
{
	Gacha11,
	Gacha35,
	Ads
}
