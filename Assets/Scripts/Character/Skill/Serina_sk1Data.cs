using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 아군 전체를 공격력의 521%만큼 회복시키고 5초동안 아군 전체의 공격속도를 14%, 이동속도를 21% 증가시킨다.
/// </summary>
[Serializable]
public class Serina_sk1Data : SkillBaseData
{
	/// <summary>
	/// 공격속도 증가 데이터
	/// </summary>
	[Tooltip("공격속도 증가 데이터")]
	public AttackSpeedUpConditionData attackSpeedUpData = new AttackSpeedUpConditionData();

	/// <summary>
	/// 이동속도 증가 데이터
	/// </summary>
	[Tooltip("이동속도 증가 데이터")]
	public MoveSpeedUpConditionData moveSpeedUpData = new MoveSpeedUpConditionData();

	/// <summary>
	/// 힐량 증가량
	/// </summary>
	[Tooltip("힐량 증가량")]
	public float healMul = 1;
}


public class Serina_sk1 : SkillBase
{
	public override string iconPath => "";

	public override float skillUseTime => 0;
	public override bool needAttackState => false;

	private Serina_sk1Data skillData;



	public Serina_sk1(Serina_sk1Data _skillData) : base(_skillData)
	{
		skillData = _skillData;
		fontColor = Color.green;
	}

	public override void Action()
	{
		base.Action();
		List<Character> targetList;
		if (owner is PlayerCharacter)
		{
			targetList = CharacterManager.it.GetPlayerCharacters();
		}
		else
		{
			targetList = CharacterManager.it.GetEnemyCharacters();
		}

		// 힐하면 풀피됨
		foreach (var target in targetList)
		{
			target.conditionModule.AddCondition(new AttackSpeedUpCondition(owner, skillData.attackSpeedUpData));
			target.conditionModule.AddCondition(new MoveSpeedUpCondition(owner, skillData.moveSpeedUpData));

			target.Heal(target, target.info.AttackPower() * skillData.healMul, name, fontColor);
		}
	}
}
