
[System.Serializable]
public class MoveSpeedUpConditionData : ConditionDataBase
{
	/// <summary>
	/// 이동속도 증가비율
	/// </summary>
	public float ratio = 1;


	public MoveSpeedUpConditionData()
	{

	}

	public MoveSpeedUpConditionData(float _ratio)
	{
		ratio = _ratio;
	}

	public override object Clone()
	{
		return new AttackSpeedUpConditionData(ratio);
	}

	public override string ToString()
	{
		return $"[MoveSpeedUp] ratio: {ratio}";
	}
}

public class MoveSpeedUpCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.MoveSpeedUp;
	public override BuffType buffType => BuffType.Buff;

	public override string iconPath => "";

	public override string effectPath => "";

	private MoveSpeedUpConditionData conditionData;

	/// <summary>
	/// 이동속도 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public MoveSpeedUpCondition(UnitBase _attacker, MoveSpeedUpConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}

	public override void Start()
	{
		duration = ConfigMeta.it.BUFF_DURATION;
		base.Start();
	}
}
