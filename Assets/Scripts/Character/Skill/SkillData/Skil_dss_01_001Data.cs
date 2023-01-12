using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다수의 적에게 공격력의 {n}%의 피해를 {n}회 입히고 {n}초 동안 대상의 받는 피해량을 {n}% 증가시킨다.
/// </summary>
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
	/// 공격횟수
	/// </summary>
	[Tooltip("공격횟수")]
	public Int32 attackCount = 1;

	/// <summary>
	/// 공격횟수 레벨 데이터
	/// </summary>
	[Tooltip("공격횟수 레벨 데이터")]
	[FourArithmetic]
	[SerializeField] public string attackCountLevelData = "";

	/// <summary>
	/// 피해량 증가 데이터
	/// </summary>
	[Tooltip("피해량 증가 데이터")]
	[SerializeField] public DamageUpConditionData damageUpConditionData = new DamageUpConditionData();

	/// <summary>
	/// 피해량 증가 시간 레벨 데이터
	/// </summary>
	[Tooltip("피해량 증가 시간 레벨 데이터")]
	[FourArithmetic]
	[SerializeField] public string damageUpConditionDurationLevelData = "";

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
	private Int32 totalAttackCount;
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
			for(Int32 i =0 ; i< totalAttackCount ; i++)
			{
				SkillUtility.SimpleAttack(owner, target, target.info.AttackPower() * totalAttackPowerMul, name, fontColor);
			}
		}
	}

	public override void CalculateSkillLevelData(int _skillLevel)
	{
		totalAttackPowerMul = (float)FourArithmeticCalculator.Calculate(skillData.attackPowerMulLevelData, skillData.attackPowerMul, _skillLevel);
		totalAttackCount = (Int32)FourArithmeticCalculator.Calculate(skillData.attackCountLevelData, skillData.attackCount, _skillLevel);

		totalDamageUpConditionData = (DamageUpConditionData)skillData.damageUpConditionData.Clone();

		totalDamageUpConditionData.duration = (float)FourArithmeticCalculator.Calculate(skillData.damageUpConditionDurationLevelData, skillData.attackPowerMul, _skillLevel);
		totalDamageUpConditionData.ratio = (float)FourArithmeticCalculator.Calculate(skillData.damageUpConditionRatioLevelData, skillData.attackPowerMul, _skillLevel);

		VLog.SkillLog($"{skillEditorLogTitle} atkPowerMul: {totalAttackPowerMul}, atkCount: {totalAttackCount}, {totalDamageUpConditionData}");
	}
}
