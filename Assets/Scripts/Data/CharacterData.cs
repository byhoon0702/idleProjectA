using System;

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

	public long classTid;
	public long raceTid;
	public ElementType element;
	public Grade grade;
	public int starlevel;
	public IdleNumber hp;
	public IdleNumber attackPower;

	public string skillID = "";
	public int skillLevel = 1;
	//유저 캐릭터는 설정 하지 않는 값 
	//추후 성(star)급은 StarRank 로 이름 지을 것 
	public RankType rankType;

	public CharacterData Clone()
	{
		CharacterData data = new CharacterData();
		data.name = name;
		data.resource = resource;
		data.tid = tid;
		data.classTid = classTid;
		data.raceTid = raceTid;
		data.element = element;
		data.grade = grade;
		data.starlevel = starlevel;
		data.hp = hp;
		data.attackPower = attackPower;
		data.skillID = skillID;
		data.skillLevel = skillLevel;
		data.rankType = rankType;
		return data;
	}
}
