
[System.Serializable]
public class MoveSpeedUpConditionData : ConditionDataBase
{
	/// <summary>
	/// 이동속도 증가비율
	/// </summary>
	public float ratio = 1;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration = 5;


	public MoveSpeedUpConditionData()
	{

	}

	public MoveSpeedUpConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}

	public override object Clone()
	{
		return new AttackSpeedUpConditionData(ratio, duration);
	}

	public override string ToString()
	{
		return $"[MoveSpeedUp] ratio: {ratio}, duration: {duration}";
	}
}

public class MoveSpeedUpCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.MoveSpeedUp;
	public override BuffType buffType => BuffType.Buff;

	public override string iconPath => "";

	public override string effectPath => "";

	private MoveSpeedUpConditionData conditionData;

	/// <summary>
	/// 이동속도 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public MoveSpeedUpCondition(Character _attacker, MoveSpeedUpConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
