using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 다수의 적에게 공격력의 {n}%의 피해를 입히고 {n}초 동안 {n}%만큼 지속 피해(속성)를 입힌다.
/// </summary>
public class Skil_dss_04_001Data : SkillBaseData
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
	/// 도트 대미지 정보
	/// </summary>
	[Tooltip("도트 대미지 정보")]
	public DoteConditionData doteData = new DoteConditionData();

	/// <summary>
	/// 피해량 증가 비율 레벨 데이터
	/// </summary>
	[Tooltip("도트 대미지 증가 비율 레벨 데이터")]
	[FourArithmetic]
	[SerializeField] public string doteDataRatioLevelData = "";
}


public class Skil_dss_04_001 : SkillBase
{
	public override string iconPath => "";

	public override float skillUseTime => 0;

	private Skil_dss_04_001Data skillData;

	private float totalAttackPowerMul;
	private DoteConditionData totalDoteData = new DoteConditionData();


	public Skil_dss_04_001(Skil_dss_04_001Data _skillData) : base(_skillData)
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
			target.conditionModule.AddCondition(new DoteCondition(owner, totalDoteData));
			SkillUtility.SimpleAttack(owner, target, target.info.AttackPower() * totalAttackPowerMul, name, fontColor);
		}
	}
	public override void CalculateSkillLevelData(int _skillLevel)
	{
		totalAttackPowerMul = (float)FourArithmeticCalculator.Calculate(skillData.attackPowerMulLevelData, skillData.attackPowerMul, _skillLevel);

		totalDoteData = (DoteConditionData)skillData.doteData.Clone();
		totalDoteData.tickAttackPowerMul = (float)FourArithmeticCalculator.Calculate(skillData.doteDataRatioLevelData, skillData.attackPowerMul, _skillLevel);

		VLog.SkillLog($"{skillEditorLogTitle} totalAttackPowerMul: {totalAttackPowerMul}, {totalDoteData}");
	}
}
