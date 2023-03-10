
[System.Serializable]
public class MoveSpeedDownConditionData : ConditionDataBase
{
	/// <summary>
	/// 이동속도 감소비율
	/// </summary>
	public float ratio = 0.5f;

	public MoveSpeedDownConditionData()
	{

	}

	public MoveSpeedDownConditionData(float _ratio)
	{
		ratio = _ratio;
	}

	public override object Clone()
	{
		return new MoveSpeedDownConditionData(ratio);
	}

	public override string ToString()
	{
		return $"[MoveSpeedDown] ratio: {ratio}";
	}
}

public class MoveSpeedDownCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.MoveSpeedDown;
	public override BuffType buffType => BuffType.Debuff;

	public override string iconPath => "";

	public override string effectPath => "";

	private MoveSpeedDownConditionData conditionData;

	/// <summary>
	/// 이동속도 감소비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public MoveSpeedDownCondition(UnitBase _attacker, MoveSpeedDownConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}

	public override void Start()
	{
		duration = ConfigMeta.it.BUFF_DURATION;
		base.Start();
	}
}
