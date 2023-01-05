using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CriticalUpConditionData
{
	/// <summary>
	/// 크리티컬 확률 증가비율
	/// </summary>
	public float ratio;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration;

	public CriticalUpConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}
}

public class CriticalUpCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.CriticalUp;

	public override string iconPath => "";

	public override string effectPath => "";


	private CriticalUpConditionData conditionData;

	/// <summary>
	/// 크리티컬 확률 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public CriticalUpCondition(Character _attacker, CriticalUpConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
