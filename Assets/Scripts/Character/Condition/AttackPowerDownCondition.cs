﻿public class AttackPowerDownConditionData
{
	/// <summary>
	/// 공격력 감소비율
	/// </summary>
	public float ratio;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration;


	public AttackPowerDownConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}
}

public class AttackPowerDownCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.AttackPowerDown;
	public override string iconPath => "";

	public override string effectPath => "";

	private AttackPowerDownConditionData conditionData;

	/// <summary>
	/// 공격력 감소비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public AttackPowerDownCondition(Character _attacker, AttackPowerDownConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}