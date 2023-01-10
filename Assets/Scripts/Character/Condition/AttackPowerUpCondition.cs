[System.Serializable]
public class AttackPowerUpConditionData
{
	/// <summary>
	/// 공격력 증가비율
	/// </summary>
	public float ratio = 0.5f;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration = 5;



	public AttackPowerUpConditionData()
	{

	}

	public AttackPowerUpConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}
}

public class AttackPowerUpCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.AttackPowerUp;
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
