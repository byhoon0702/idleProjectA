using System.Collections.Generic;
using UnityEngine;


public class Serina_sk1Data : SkillBaseData
{
	/// <summary>
	/// 공격속도 증가 데이터
	/// </summary>
	public AttackSpeedUpConditionData attackSpeedUpData;

	/// <summary>
	/// 이동속도 증가 데이터
	/// </summary>
	public MoveSpeedUpConditionData moveSpeedUpData;

	/// <summary>
	/// 힐량 증가량
	/// </summary>
	public float healMul;


	/// <summary>
	/// 아군 전체를 공격력의 521%만큼 회복시키고 5초동안 아군 전체의 공격속도를 14%, 이동속도를 21% 증가시킨다.
	/// </summary>
	public Serina_sk1Data(AttackSpeedUpConditionData _attackSpeedUpData, MoveSpeedUpConditionData _moveSpeedUpData, float _healMul, float _cooltime) : base(_cooltime)
	{
		attackSpeedUpData = _attackSpeedUpData;
		moveSpeedUpData = _moveSpeedUpData;
		healMul = _healMul;
	}
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

			target.Heal(target, target.info.AttackPower() * skillData.healMul, fontColor);
		}
	}
}
