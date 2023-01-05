using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StunConditionData
{
	/// <summary>
	/// 스턴시간
	/// </summary>
	public float duration;


	public StunConditionData(float _duration)
	{
		duration = _duration;
	}	
}

public class StunCondition : ConditionBase
{

	public override UnitCondition conditionType => UnitCondition.Stun;

	public override string iconPath => "";

	public override string effectPath => "";

	private StunConditionData conditionData;

	public StunCondition(Character _attacker, StunConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}

	public override void Start()
	{
		base.Start();
		character.StunStart();
	}
}
