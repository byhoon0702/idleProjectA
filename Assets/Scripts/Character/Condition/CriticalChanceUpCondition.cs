public class CriticalChanceUpConditionData
{
	/// <summary>
	/// 크리티컬 확률 증가비율
	/// </summary>
	public float ratio;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration;

	public CriticalChanceUpConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}
}

public class CriticalChanceUpCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.CriticalChanceUp;

	public override string iconPath => "";

	public override string effectPath => "";


	private CriticalChanceUpConditionData conditionData;

	/// <summary>
	/// 크리티컬 확률 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public CriticalChanceUpCondition(Character _attacker, CriticalChanceUpConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
