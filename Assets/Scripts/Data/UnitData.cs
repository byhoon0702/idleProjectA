using System;

/// <summary>
/// Raw 데이터
/// </summary>
[Serializable]
public class UnitData : BaseData
{
	public string name;
	//데이터 테이블에만 표시되는 설명 
	public string resource;
	public long classTid;
	public Grade grade;
	public int starlevel;
	public long hp;
	public long attackPower;
	public float attackSpeed;
	public string attackSound;
	public AttackType attackType;
	public float criticalRate;
	public float criticalPowerRate;
	public float moveSpeed;
	public Int64 skillTid = 0;
	//유저 캐릭터는 설정 하지 않는 값 
	//추후 성(star)급은 StarRank 로 이름 지을 것 
	public RankType rankType;
	public string projectileResource;

	public UnitData Clone()
	{
		UnitData data = new UnitData();
		data.name = name;
		data.resource = resource;
		data.tid = tid;
		data.classTid = classTid;
		data.grade = grade;
		data.starlevel = starlevel;
		data.hp = hp;
		data.attackPower = attackPower;
		data.attackSpeed = attackSpeed;
		data.attackSound = attackSound;
		data.attackType = attackType;
		data.criticalRate = criticalRate;
		data.criticalPowerRate = criticalPowerRate;
		data.moveSpeed = moveSpeed;
		data.skillTid = skillTid;
		data.rankType = rankType;
		data.projectileResource = projectileResource;


		return data;
	}
}
