
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
	public override CharacterCondition conditionType => CharacterCondition.MoveSpeedDown;
	public override BuffType buffType => BuffType.Debuff;

	public override string iconPath => "";

	public override string effectPath => "";

	private MoveSpeedDownConditionData conditionData;

	/// <summary>
	/// 이동속도 감소비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public MoveSpeedDownCondition(Character _attacker, MoveSpeedDownConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}

	public override void Start()
	{
		duration = ConditionUtility.GetDefaultDebuffDuration(character.info.data.grade);
		base.Start();
	}
}
