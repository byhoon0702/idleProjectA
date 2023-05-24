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



[System.Serializable]
public class SkillLevelSheet
{
	public int level;
	public float attackRange;
	public int attackCount;
	public float attackInterval;
}


public enum SkillTreeType
{
	ATTACK,
	DEFENCE,
	UTILITY,
	SPECIAL,

}

public enum ValueModifyTarget
{
	NONE,
	CHARACTER,
	SKILL,

}


[Serializable]
public class SkillData : ItemData
{
	public string ui_Description;
	public SkillTreeType type;

	public ValueModifyTarget valueModifyTarget;
	public ItemStats useValue;
	public SkillCooldownType cooldownType;
	public float cooldownValue;
	public SkillActiveType activeType;

	public int maxLevel;
	public long rootSkillTid;
	public List<SkillLevelSheet> levelSheet;

	public bool hideInUI;
	public SkillData()
	{

	}
	//에디트용으로만 호출 할 것
	public SkillData(SkillItemObject itemObject)
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
