using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 체력이 가장 높은 적에게 공격력의 {n}%의 피해를 {n}회 입히고 {n}% 확률로 {n}%의 추가 피해를 입힌다.
/// </summary>
public class Skil_dss_03_001Data : SkillBaseData
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
	/// 추가 피해확률
	/// </summary>
	[Tooltip("추가 피해확률")]
	public float extraAttackProbability = 0.5f;

	/// <summary>
	/// 추가 피해배율
	/// </summary>
	[Tooltip("추가 피해배율")]
	[SerializeField] public float extraAttackPowerMul = 0.5f;

	/// <summary>
	/// 피해량 증가 비율 레벨 데이터
	/// </summary>
	[Tooltip("피해량 증가 비율 레벨 데이터")]
	[FourArithmetic]
	[SerializeField] public string extraAttackPowerMulLevelData = "";
}


public class Skil_dss_03_001 : SkillBase
{
	public override string iconPath => "";

	public override float skillUseTime => 0;

	private Skil_dss_03_001Data skillData;


	private float totalAttackPowerMul;
	private float totalExtraAttackPowerMul;


	public Skil_dss_03_001(Skil_dss_03_001Data _skillData) : base(_skillData)
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
			IdleNumber totalAttackPower = target.info.AttackPower() * totalAttackPowerMul;

			bool extraAttackPower = SkillUtility.Cumulative(skillData.extraAttackProbability);
			if (extraAttackPower)
			{
				totalAttackPower += target.info.AttackPower() * totalExtraAttackPowerMul;
			}

			SkillUtility.SimpleAttack(owner, target, totalAttackPower, name, fontColor);
		}
	}

	public override void CalculateSkillLevelData(int _skillLevel)
	{
		totalAttackPowerMul = (float)FourArithmeticCalculator.Calculate(skillData.attackPowerMulLevelData, skillData.attackPowerMul, _skillLevel);
		totalExtraAttackPowerMul = (float)FourArithmeticCalculator.Calculate(skillData.extraAttackPowerMulLevelData, skillData.extraAttackPowerMul, _skillLevel);

		VLog.SkillLog($"[스킬 초기화] {skillEditorLogTitle} atkPowerMul: {totalAttackPowerMul}, totalExtraAttackPowerMul: {totalExtraAttackPowerMul}");
	}
}
