using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DamageUpConditionData
{
	/// <summary>
	/// 공격력 증가비율
	/// </summary>
	public float ratio;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration;

	public DamageUpConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}
}

public class DamageUpCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.DamageUp;
	public override string iconPath => "";

	public override string effectPath => "";

	private DamageUpConditionData conditionData;

	/// <summary>
	/// 공격력 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public DamageUpCondition(Character _attacker, DamageUpConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
