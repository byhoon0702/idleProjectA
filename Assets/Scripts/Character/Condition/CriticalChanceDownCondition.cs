public class CriticalChanceDownConditionData
{
	/// <summary>
	/// 크리티컬 확률 감소 비율
	/// </summary>
	public float ratio;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration;

	public CriticalChanceDownConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}
}

public class CriticalChanceDownCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.CriticalChanceDown;

	public override string iconPath => "";

	public override string effectPath => "";


	private CriticalChanceDownConditionData conditionData;

	/// <summary>
	/// 크리티컬 확률 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public CriticalChanceDownCondition(Character _attacker, CriticalChanceDownConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
