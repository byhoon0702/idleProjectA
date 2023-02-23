using System;
using System.Collections.Generic;
using System.Data;

[Serializable]
public class BasicStat
{
	public Stats type;
	public string value;
}

/// <summary>
/// Raw 데이터
/// </summary>
[Serializable]
public class UnitData : BaseData
{
	public string name;
	//데이터 테이블에만 표시되는 설명 
	public string resource;
	public long skillEffectTidNormal;

	public Grade grade;
	public int starlevel;
	public List<BasicStat> statusDataList;

	public float attackTime;
	public float attackCoolTime;

	public float criticalRate;
	public float criticalPowerRate;

	public Int64 skillTid = 0;
	public Int64 finalSkillTid = 0;
	//유저 캐릭터는 설정 하지 않는 값 
	//추후 성(star)급은 StarRank 로 이름 지을 것 
	//public RankType rankType;

	public List<UnitStatusPerLevelData> statusPerLevels;

	public UnitData()
	{
		//기본적으로 무조건 추가 되어야 할 데이터들
		//데이터가 없을때만 넣도록 한다.
		if (statusDataList == null || statusDataList.Count == 0)
		{
			statusDataList = new List<BasicStat>();
			statusDataList.Add(new BasicStat() { type = Stats.Attackpower, value = "1000" });
			statusDataList.Add(new BasicStat() { type = Stats.Hp, value = "1" });
			statusDataList.Add(new BasicStat() { type = Stats.AttackSpeed, value = "1" });
			statusDataList.Add(new BasicStat() { type = Stats.Movespeed, value = "5" });
		}
	}

	public UnitData Clone()
	{
		UnitData data = new UnitData();
		data.name = name;
		data.resource = resource;
		data.tid = tid;
		data.grade = grade;
		data.starlevel = starlevel;
		data.attackCoolTime = attackCoolTime;

		data.criticalRate = criticalRate;
		data.criticalPowerRate = criticalPowerRate;
		data.skillTid = skillTid;
		data.finalSkillTid = finalSkillTid;

		data.skillEffectTidNormal = skillEffectTidNormal;
		data.statusPerLevels = new List<UnitStatusPerLevelData>(statusPerLevels);
		data.statusDataList = statusDataList;

		return data;
	}
}

[Serializable]
public class UnitStatusPerLevelData
{
	public int level;

	public string attackPower;
	public string attackPowerPerLevel;

	public string hp;
	public string hpPerLevel;

	public UnitStatusPerLevelData Clone()
	{
		UnitStatusPerLevelData data = new UnitStatusPerLevelData();

		data.level = level;

		data.attackPower = attackPower;
		data.attackPowerPerLevel = attackPowerPerLevel;

		data.hp = hp;
		data.hpPerLevel = hpPerLevel;

		return data;
	}

	public UnitStatusPerLevelData()
	{
		level = 1;
		attackPower = "100";
		attackPowerPerLevel = "0";
		hp = "100";
		hpPerLevel = "1.5";
	}
}
