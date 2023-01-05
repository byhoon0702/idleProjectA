using System.Collections.Generic;
using UnityEngine;



public class Landrock_sk1Data : SkillBaseData
{
	/// <summary>
	/// 넉백 데이터
	/// </summary>
	public KnockbackConditionData knockbackData;

	/// <summary>
	/// 방어감소 데이터
	/// </summary>
	public DamageDownConditionData damageDownConditionData;

	/// <summary>
	/// 공격범위
	/// </summary>
	public float attackRange;

	/// <summary>
	/// 대미지 배율
	/// </summary>
	public float attackPowerMul;


	/// <summary>
	/// 전방의 적 다수를 넉백시키며 공격력의 112%만큼 피해를 입히고 4초 동안 대상의 받는 방어력을 5% 증가시킨다.
	/// </summary>
	public Landrock_sk1Data(KnockbackConditionData _knockbackData, DamageDownConditionData _damageDownData, float _attackRange, float _attackPowerMul, float _cooltime) : base(_cooltime)
	{
		knockbackData = _knockbackData;
		damageDownConditionData = _damageDownData;
		attackRange = _attackRange;
		attackPowerMul = _attackPowerMul;
	}
}


public class Landrock_sk1 : SkillBase
{

	public override string iconPath => "";

	public override float skillUseTime => 0;

	Landrock_sk1Data skillData;


	public Landrock_sk1(Landrock_sk1Data _skillData) : base(_skillData)
	{
		skillData = _skillData;
		fontColor = Color.magenta;
	}

	public override void Action()
	{
		base.Action();

		List<Character> searchList;
		if (owner is PlayerCharacter)
		{
			searchList = CharacterManager.it.GetEnemyCharacters();
		}
		else
		{
			searchList = CharacterManager.it.GetPlayerCharacters();
		}

		var targetList = SkillUtility.GetCharacterRange(owner.transform.position, skillData.attackRange, searchList, false);
		foreach (var target in targetList)
		{
			target.conditionModule.AddCondition(new KnockbackCondition(owner, skillData.knockbackData));
			target.conditionModule.AddCondition(new DamageDownCondition(owner, skillData.damageDownConditionData));
			SkillUtility.SimpleAttack(owner, target, owner.info.AttackPower() * skillData.attackPowerMul, fontColor);
		}
	}
}
