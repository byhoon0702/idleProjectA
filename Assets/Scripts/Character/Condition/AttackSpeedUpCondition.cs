using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AttackSpeedUpConditionData
{
	/// <summary>
	/// 공격속도 증가비율
	/// </summary>
	public float ratio;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration;

	public AttackSpeedUpConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}
}

public class AttackSpeedUpCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.AttackSpeedUp;

	public override string iconPath => "";

	public override string effectPath => "";

	private AttackSpeedUpConditionData conditionData;

	/// <summary>
	/// 공격속도 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public AttackSpeedUpCondition(Character _attacker, AttackSpeedUpConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
