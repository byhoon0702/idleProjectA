using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ReducedDefenseConditionData
{
	/// <summary>
	/// 피해감소량
	/// </summary>
	public float ratio;

	/// <summary>
	/// 감소시간
	/// </summary>
	public float duration;

	public ReducedDefenseConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}	
}

public class ReducedDefenseCondition : ConditionBase
{

	public override UnitCondition conditionType => UnitCondition.ReducedDefense;

	public override string iconPath => "";

	public override string effectPath => "";

	private ReducedDefenseConditionData conditionData;

	/// <summary>
	/// 방어력 감소량
	/// </summary>
	public float ratio => conditionData.ratio;


	public ReducedDefenseCondition(Character _attacker, ReducedDefenseConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
