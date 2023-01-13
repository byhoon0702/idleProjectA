﻿using System;



/// <summary>
/// Raw 데이터
/// </summary>
[Serializable]
public class CharacterData : ItemData
{
	public string name;
	//데이터 테이블에만 표시되는 설명 
	public string resource;

	public long classTid;
	public long raceTid;
	public ElementType element;
	public Grade grade;
	public int starlevel;
	public IdleNumber hp;
	public IdleNumber attackPower;
	public Int64 skillTid = 0;
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
		data.skillTid = skillTid;
		data.rankType = rankType;
		return data;
	}
}
