using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skil_dss_01_001Data : SkillBaseData
{
	/// <summary>
	/// 대미지 증가배율
	/// </summary>
	[Tooltip("대미지 증가배율")]
	public float attackPowerMul = 1;

	/// <summary>
	/// 대미지 배율 레벨 데이터
	/// </summary>
	[Tooltip("대미지 배율 레벨 데이터")]
	[FourArithmetic]
	[SerializeField] public string attackPowerMulLevelData = "";

	/// <summary>
	/// 피해량 증가 데이터
	/// </summary>
	[Tooltip("피해량 증가 데이터")]
	[SerializeField] public DamageUpConditionData damageUpConditionData = new DamageUpConditionData();

	/// <summary>
	/// 피해량 증가 비율 레벨 데이터
	/// </summary>
	[Tooltip("피해량 증가 비율 레벨 데이터")]
	[FourArithmetic]
	[SerializeField] public string damageUpConditionRatioLevelData = "";
}

public class Skil_dss_01_001 : SkillBase
{
	public override string iconPath => "";

	public override float skillUseTime => 0;
	private Skil_dss_01_001Data skillData;

	private float totalAttackPowerMul;
	private DamageUpConditionData totalDamageUpConditionData = new DamageUpConditionData();


	public Skil_dss_01_001(Skil_dss_01_001Data _skillData) : base(_skillData)
	{
		skillData = _skillData;
	}

	public override void Action()
	{
		base.Action();
		List<Character> targetList = SkillUtility.GetTargetCharacterNonAlloc(owner, skillData.targetingType);
		foreach (var target in targetList)
		{
			target.conditionModule.AddCondition(new DamageUpCondition(owner, totalDamageUpConditionData));
			SkillUtility.SimpleAttack(owner, target, target.info.AttackPower() * totalAttackPowerMul, name, fontColor);
		}
	}

	public override void CalculateSkillLevelData(int _skillLevel)
	{
		totalAttackPowerMul = (float)FourArithmeticCalculator.Calculate(skillData.attackPowerMulLevelData, skillData.attackPowerMul, _skillLevel);

		totalDamageUpConditionData = (DamageUpConditionData)skillData.damageUpConditionData.Clone();
		totalDamageUpConditionData.ratio = (float)FourArithmeticCalculator.Calculate(skillData.damageUpConditionRatioLevelData, skillData.attackPowerMul, _skillLevel);

		VLog.SkillLog($"[스킬 초기화] {skillEditorLogTitle} atkPowerMul: {totalAttackPowerMul}, {totalDamageUpConditionData}");
	}
}
