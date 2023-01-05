public class AttackSpeedDownConditionData
{
	/// <summary>
	/// 공격속도 증가비율
	/// </summary>
	public float ratio;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration;

	public AttackSpeedDownConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}
}

public class AttackSpeedDownCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.AttackSpeedDown;

	public override string iconPath => "";

	public override string effectPath => "";

	private AttackSpeedDownConditionData conditionData;

	/// <summary>
	/// 공격속도 감소비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public AttackSpeedDownCondition(Character _attacker, AttackSpeedDownConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
