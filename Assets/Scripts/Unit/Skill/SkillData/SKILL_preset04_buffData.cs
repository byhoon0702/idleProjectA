using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 버프형
/// </summary>
public class SKILL_preset04_buffData : SkillBaseData
{
	/// <summary>
	/// 버프 타입
	/// </summary>
	[Tooltip("버프 타입")]
	public SkillBuffType skillBuffType;

	/// <summary>
	/// 버프값 비율
	/// </summary>
	[Tooltip("대미지 증가비율")]
	public float buffValueRatio = 1;

	/// <summary>
	/// 버프값 비율 계산식
	/// </summary>
	[FourArithmetic]
	[Tooltip("버프값 비율 계산식")]
	public string buffValueOperator;
}

public class SKILL_preset04_buff : SkillBase
{
	public override float skillUseTime => 1;

	private SKILL_preset04_buffData skillData;

	private float totalRatio;



	public SKILL_preset04_buff(SKILL_preset04_buffData _skillData) : base(_skillData)
	{
		skillData = _skillData;
		fontColor = Color.magenta;
	}

	public override void Action()
	{
		owner.conditionModule.AddCondition(new AttackPowerUpCondition(owner, new AttackPowerUpConditionData(totalRatio)));
		base.Action();
	}

	public override void CalculateSkillLevelData(int _skillLevel)
	{
		totalRatio = (float)FourArithmeticCalculator.Calculate(skillData.buffValueOperator, skillData.buffValueRatio, _skillLevel);
	}
}
