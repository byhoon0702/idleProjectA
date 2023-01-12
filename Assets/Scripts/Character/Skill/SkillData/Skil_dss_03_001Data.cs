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
	private Int32 totalAttackCount;
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
			for (Int32 i = 0 ; i < totalAttackCount ; i++)
			{
				SkillUtility.SimpleAttack(owner, target, target.info.AttackPower() * totalAttackPowerMul, name, fontColor);

				bool addDamage = SkillUtility.Cumulative(skillData.extraAttackProbability);
				if(addDamage)
				{
					SkillUtility.SimpleAttack(owner, target, target.info.AttackPower() * totalExtraAttackPowerMul, name, fontColor);
				}
			}
		}
	}

	public override void CalculateSkillLevelData(int _skillLevel)
	{
		totalAttackPowerMul = (float)FourArithmeticCalculator.Calculate(skillData.attackPowerMulLevelData, skillData.attackPowerMul, _skillLevel);
		totalAttackCount = (Int32)FourArithmeticCalculator.Calculate(skillData.attackCountLevelData, skillData.attackCount, _skillLevel);
		totalExtraAttackPowerMul = (float)FourArithmeticCalculator.Calculate(skillData.extraAttackPowerMulLevelData, skillData.extraAttackPowerMul, _skillLevel);

		VLog.SkillLog($"{skillEditorLogTitle} atkPowerMul: {totalAttackPowerMul}, atkCount: {totalAttackCount}, totalExtraAttackPowerMul: {totalExtraAttackPowerMul}");
	}
}
