using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum SkillCooldownType
{
	/// <summary>
	/// 시간
	/// </summary>
	TIME,
	/// <summary>
	/// 맞은 횟수
	/// </summary>
	HITCOUNT,
	/// <summary>
	/// 죽인 횟수
	/// </summary>
	KILLCOUNT,
	/// <summary>
	/// 공격 횟수
	/// </summary>
	ATTACKCOUNT,

	/// <summary>
	/// 애니메이션에서 호출
	/// </summary>
	ANIMATION,

	/// <summary>
	/// 쿨타임 없이 연속적인
	/// </summary>
	CONTINUOUS,

}

public enum SkillActiveType
{
	ACTIVE,
	PASSIVE,
}
public enum TargetingType
{
	TARGETING,
	NONTARGETING,
}

public enum Target
{
	PLAYER,
	ENEMY,
}

public enum StatusEffect
{
	NONE,
	STUN,
}


[System.Serializable]
public class SkillLevelSheet
{
	public string description_key;
	public int evolutionLevel;
	public int maxLevel;
	//타겟 수
	public int targetCount;
	//사거리
	public float attackRange;
	//공격 횟수
	public int attackCount;
	//공격 간격
	public float attackInterval;
	//타격 범위
	public float hitRange;
	//타격 할 적 수
	public int hitCount;

	//넉백 파워
	public float knockbackPower;
	//스킬 지속시간
	public float duration;

	public StatusEffect statusEffect;

	public SkillLevelSheet()
	{
		evolutionLevel = 0;
		maxLevel = 100;
		targetCount = 1;    // 타겟 카운트 2
		attackRange = 1;    // 공격 사거리 3 
		attackCount = 1;    // 공격 횟수 4
		attackInterval = 0; // 공격 간격 5
		hitRange = 0;       // 공격 범위 6
		hitCount = 1;       // 범위 내의 타겟 수 7
		knockbackPower = 0; // 넉백 8
		duration = 0;       // 지속시간  9
		statusEffect = StatusEffect.NONE; // 상태이상 10
	}
}


public enum SkillType
{
	ATTACK,
	HEAL,
	BUFF,
	DEBUFF
}

public enum SkillTimeType
{
	NONE,
	DURATION,
	INTERVAL,
}

public enum ValueModifyTarget
{
	NONE,
	CHARACTER,
	SKILL,
}
public enum RequirementType
{
	/// <summary>
	/// 스킬 해금 조건 없음
	/// </summary>
	NONE,
	//이전 스킬 레벨
	BASESKILL,
	//유저 레벨
	USERLEVEL,
	//스테이지
	NORMAL_STAGE,
	//승급 단계
	ADVANCEMENT,
	//몬스터 처치
	MONSTERKILL,

	//일일 몬스터 처리
	DAILYKILLCOUNT,
}

[System.Serializable]
public class RequirementInfo
{
	public RequirementType type;
	public long parameter1;
	public int parameter2;
}

[Serializable]
public class SkillDetailData
{
	public SkillCooldownType cooldownType;
	public float cooldownValue;
	public SkillActiveType activeType;


	//public long rootSkillTid;

}


[Serializable]
public class SkillData : ItemData
{
	public string ui_Description;
	public SkillTimeType timeType;
	public SkillType skillType;

	public string animation;

	public ValueModifyTarget valueModifyTarget;
	public ItemStats useValue;


	public SkillDetailData detailData;
	public List<SkillLevelSheet> levelSheet;
	public bool hideInUI;
	public SkillData()
	{

	}


}

[Serializable]
public class SkillDataSheet : DataSheetBase<SkillData>
{
	public List<SkillData> GetList(Grade _grade)
	{
		List<SkillData> outData = new List<SkillData>();

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].itemGrade == _grade)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}

	//public SkillItemData GetByHashTag(string _hashTag)
	//{
	//	if (cachedTidList.TryGetValue(_hashTag, out var data))
	//	{
	//		return data;
	//	}

	//	for (int i = 0; i < infos.Count; i++)
	//	{
	//		if (infos[i].hashTag == _hashTag)
	//		{
	//			cachedTidList.Add(_hashTag, infos[i]);
	//			return infos[i];
	//		}
	//	}

	//	return null;
	//}
}
