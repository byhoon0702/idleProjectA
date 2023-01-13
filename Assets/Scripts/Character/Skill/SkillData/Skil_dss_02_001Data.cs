using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 다수의 적에게 공격력의 {n}%의 피해를 {n}회 입히고 넉백 시킨다.
/// </summary>
public class Skil_dss_02_001Data : SkillBaseData
{
	/// <summary>
	/// 대미지 배율
	/// </summary>
	[Tooltip("대미지 배율")]
	[SerializeField] public float attackPowerMul = 1;

	/// <summary>
	/// 대미지 배율 레벨 데이터
	/// </summary>
	[Tooltip("대미지 배율 레벨 데이터")]
	[FourArithmetic]
	[SerializeField] public string attackPowerMulLevelData = "";

	/// <summary>
	/// 넉백 데이터
	/// </summary>
	[Tooltip("넉백 데이터")]
	[SerializeField] public KnockbackConditionData knockbackData = new KnockbackConditionData();
}


public class Skil_dss_02_001 : SkillBase
{
	public override string iconPath => "";

	public override float skillUseTime => 0;

	private Skil_dss_02_001Data skillData;

	private float totalAttackPowerMul;



	public Skil_dss_02_001(Skil_dss_02_001Data _skillData) : base(_skillData)
	{
		skillData = _skillData;
		fontColor = Color.magenta;
	}

	public override void Action()
	{
		base.Action();

		List<Character> targetList = SkillUtility.GetTargetCharacterNonAlloc(owner, skillData.targetingType);
		foreach (var target in targetList)
		{
			target.conditionModule.AddCondition(new KnockbackCondition(owner, skillData.knockbackData));
			SkillUtility.SimpleAttack(owner, target, owner.info.AttackPower() * totalAttackPowerMul, name, fontColor);
		}
	}

	public override void CalculateSkillLevelData(int _skillLevel)
	{
		totalAttackPowerMul = (float)FourArithmeticCalculator.Calculate(skillData.attackPowerMulLevelData, skillData.attackPowerMul, _skillLevel);

		VLog.SkillLog($"[스킬 초기화] {skillEditorLogTitle} atkPowerMul: {totalAttackPowerMul}");
	}
}
