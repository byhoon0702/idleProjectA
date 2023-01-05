using System;
using UnityEngine;

public enum RankType
{
	NONE = 0,
	MINION = 1,
	ELITE,
	MID_BOSS,
	BOSS,
	FINISH_GEM,
}

public enum RaceType
{
	NONE,
	HUMAN,
	ORC,
	ELF,
	UNDEAD,
	SUMMON,
}

public enum ElementType
{
	FIRE,
	WATER,
	LEAF,
	DARK,
	LIGHT,
}

public enum Grade
{
	D,
	C,
	B,
	A,
	S,
	SS,
	SSS,
}


/// <summary>
/// Raw 데이터
/// </summary>
[Serializable]
public class CharacterData
{
	public string name;
	public string resource;
	public long tid;

	public ClassType classType;
	public RaceType race;
	public ElementType element;
	public Grade grade;
	public int starlevel;
	public IdleNumber hp;
	public float attackRange = 1;
	public float attackSpeed = 1;
	public float attackTime = 1;
	[Range(0.1f, 2f)]
	public float attackRate = 1;
	public IdleNumber attackPower;

	/// <summary>
	/// 치명타확률
	/// </summary>
	public float criticalRatio = 0.2f;
	/// <summary>
	/// 치명타 피해량
	/// </summary>
	public float criticalChangeUpRatio = 1;

	public float moveSpeed = 1;
	public float searchTime = 1f;
	public float searchRange = 1;
	public string skillID = "";
	public AttackType attackType;
	//유저 캐릭터는 설정 하지 않는 값 
	//추후 성(star)급은 StarRank 로 이름 지을 것 
	public RankType rankType;

	public CharacterData Clone()
	{
		CharacterData data = new CharacterData();
		data.name = name;
		data.resource = resource;
		data.tid = tid;
		data.classType = classType;
		data.race = race;
		data.element = element;
		data.grade = grade;
		data.starlevel = starlevel;
		data.hp = hp;
		data.attackRange = attackRange;
		data.attackSpeed = attackSpeed;
		data.attackTime = attackTime;
		data.moveSpeed = moveSpeed;
		data.attackRate = attackRate;
		data.attackPower = attackPower;
		data.searchTime = searchTime;
		data.searchRange = searchRange;
		data.skillID = skillID;
		data.attackType = attackType;
		data.rankType = rankType;
		return data;
	}
}
