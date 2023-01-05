public class DamageDownConditionData
{
	/// <summary>
	/// 피해감소량
	/// </summary>
	public float ratio;

	/// <summary>
	/// 감소시간
	/// </summary>
	public float duration;

	public DamageDownConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}	
}

public class DamageDownCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.DamageDown;

	public override string iconPath => "";

	public override string effectPath => "";

	private DamageDownConditionData conditionData;

	/// <summary>
	/// 피해 감소량
	/// </summary>
	public float ratio => conditionData.ratio;


	public DamageDownCondition(Character _attacker, DamageDownConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
