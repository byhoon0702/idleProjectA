﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MoveSpeedUpConditionData
{
	/// <summary>
	/// 이동속도 증가비율
	/// </summary>
	public float ratio;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration;

	public MoveSpeedUpConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}
}

public class MoveSpeedUpCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.MoveSpeedUp;

	public override string iconPath => "";

	public override string effectPath => "";

	private MoveSpeedUpConditionData conditionData;

	/// <summary>
	/// 이동속도 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public MoveSpeedUpCondition(Character _attacker, MoveSpeedUpConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}