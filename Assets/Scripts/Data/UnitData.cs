using System;
using System.Collections.Generic;
using System.Data;


public enum Grade
{
	D,
	C,
	B,
	A,
	S,
	SS,
	SSS,
	_END,

}


[Serializable]
public class StatsStringInfo
{
	public StatsType type;
	public string value;

}
[Serializable]
public class AdvancementRequire
{
	public long tid;
	public int stageNumber;

}


[System.Serializable]
public class UnitAdvancementInfo
{
	public int level;
	public string nameKey;
	public string resource;
	public List<StatsStringInfo> stats;
	public Int64 skillTid = 0;
	public Int64 finalSkillTid = 0;

	public AdvancementRequire requirement;
}

/// <summary>
/// Raw 데이터
/// </summary>
[Serializable]
public class UnitData : BaseData
{
	public string name;
	//데이터 테이블에만 표시되는 설명 

	public UnitType type;
	public string resource;

	public Int64 skillTid = 0;
	public List<UnitAdvancementInfo> upgradeInfoList;



	public UnitData()
	{
		//기본적으로 무조건 추가 되어야 할 데이터들
		//데이터가 없을때만 넣도록 한다.
		if (upgradeInfoList == null || upgradeInfoList.Count == 0)
		{
			upgradeInfoList = new List<UnitAdvancementInfo>();

		}
	}

	public UnitData Clone()
	{
		UnitData data = new UnitData();
		data.name = name;
		data.resource = resource;

		data.skillTid = skillTid;
		data.tid = tid;

		data.upgradeInfoList = new List<UnitAdvancementInfo>(upgradeInfoList);

		return data;
	}
}

public enum UnitType
{
	_NONE,
	Player,
	NormalEnemy,
	BossEnemy,
	TreasureBox
}
