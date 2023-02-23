[System.Serializable]
public class AttackPowerUpConditionData : ConditionDataBase
{
	/// <summary>
	/// 공격력 증가비율
	/// </summary>
	public float ratio = 0.5f;



	public AttackPowerUpConditionData()
	{

	}

	public AttackPowerUpConditionData(float _ratio)
	{
		ratio = _ratio;
	}

	public override object Clone()
	{
		return new AttackPowerUpConditionData(ratio);
	}
	public override string ToString()
	{
		return $"[AttackPowerUp] ratio: {ratio}";
	}
}

public class AttackPowerUpCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.AttackPowerUp;
	public override BuffType buffType => BuffType.Buff;
	public override string iconPath => "";

	public override string effectPath => "";

	private AttackPowerUpConditionData conditionData;

	/// <summary>
	/// 공격력 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public AttackPowerUpCondition(UnitBase _attacker, AttackPowerUpConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}
	public override void Start()
	{
		duration = ConfigMeta.it.BUFF_DURATION;
		base.Start();
	}
}
