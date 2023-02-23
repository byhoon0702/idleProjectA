using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class StunConditionData : ConditionDataBase
{
	public StunConditionData()
	{

	}

	public override object Clone()
	{
		return new StunConditionData();
	}

	public override string ToString()
	{
		return $"[Stun]";
	}
}


/// <summary>
/// 스턴 상태이상.
/// 유닛이 일정시간동안 행동불가
/// </summary>
[System.Serializable]
public class StunCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.Stun;
	public override BuffType buffType => BuffType.Debuff;
	public override string iconPath => null;
	public override string effectPath => null;

	private StunConditionData stunData;

	public StunCondition(UnitBase _attacker, StunConditionData _conditionData) : base(_attacker, ConfigMeta.it.STUN_DURATION)
	{
		stunData = _conditionData;
	}

	public override void Start()
	{
		base.Start();
		unit.StunStart();
	}

	public override void Finish()
	{
		base.Finish();
		unit.StunFinish();
	}
}
