﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KnockbackConditionData
{
	/// <summary>
	/// 넉백시간
	/// </summary>
	public float duration;

	/// <summary>
	/// 넉백거리
	/// </summary>
	public float distance;

	public KnockbackConditionData(float _duration, float _distance)
	{
		duration = _duration;
		distance = _distance;
	}
}
public class KnockbackCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.Knockback;
	public override string iconPath => null;
	public override string effectPath => null;

	private KnockbackConditionData conditionData;
	private float distance;


	public KnockbackCondition(Character _attacker, KnockbackConditionData _conditionData)
		: base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
		distance = conditionData.distance;
	}

	public override void Start()
	{
		base.Start();
		character.KnockbackStart();
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float finalVelX = Mathf.Lerp(0, distance, dt * 5);
		if (character is PlayerCharacter)
		{
			character.transform.Translate(Vector3.left * finalVelX);
		}
		else
		{
			character.transform.Translate(Vector3.right * finalVelX);
		}
		distance -= finalVelX;
	}
}