public class AttackPowerUpConditionData
{
	/// <summary>
	/// 공격력 증가비율
	/// </summary>
	public float ratio;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration;

	public AttackPowerUpConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}
}

public class AttackPowerUpCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.AttackPowerUp;
	public override string iconPath => "";

	public override string effectPath => "";

	private AttackPowerUpConditionData conditionData;

	/// <summary>
	/// 공격력 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public AttackPowerUpCondition(Character _attacker, AttackPowerUpConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
