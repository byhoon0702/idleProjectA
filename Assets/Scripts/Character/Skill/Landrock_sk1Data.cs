using System;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 전방의 적 다수를 넉백시키며 공격력의 112%만큼 피해를 입히고 4초 동안 대상의 받는 방어력을 5% 증가시킨다.
/// </summary>
[Serializable]
public class Landrock_sk1Data : SkillBaseData
{
	/// <summary>
	/// 넉백 데이터
	/// </summary>
	[Tooltip("넉백 데이터")]
	[SerializeField] public KnockbackConditionData knockbackData = new KnockbackConditionData();

	/// <summary>
	/// 방어감소 데이터
	/// </summary>
	[Tooltip("방어감소 데이터")]
	[SerializeField] public DamageDownConditionData damageDownConditionData = new DamageDownConditionData();

	/// <summary>
	/// 공격범위
	/// </summary>
	[Tooltip("공격범위")]
	[SerializeField] public float attackRange = 5;

	/// <summary>
	/// 대미지 배율
	/// </summary>
	[Tooltip("대미지 배율")]
	[SerializeField] public float attackPowerMul = 1;
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

		var targetList = SkillUtility.GetCharacterRangeNonAlloc(owner.transform.position, skillData.attackRange, searchList);
		foreach (var target in targetList)
		{
			target.conditionModule.AddCondition(new KnockbackCondition(owner, skillData.knockbackData));
			target.conditionModule.AddCondition(new DamageDownCondition(owner, skillData.damageDownConditionData));
			SkillUtility.SimpleAttack(owner, target, owner.info.AttackPower() * skillData.attackPowerMul, name, fontColor);
		}
	}
}
