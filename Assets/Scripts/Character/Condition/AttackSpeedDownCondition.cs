[System.Serializable]
public class AttackSpeedDownConditionData : ConditionDataBase
{
	/// <summary>
	/// 공격속도 증가비율
	/// </summary>
	public float ratio = 0.5f;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration = 5;


	public AttackSpeedDownConditionData()
	{

	}

	public AttackSpeedDownConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}

	public override object Clone()
	{
		return new AttackSpeedDownConditionData(ratio, duration);
	}
	public override string ToString()
	{
		return $"[AttackSpeedDown] ratio: {ratio}, duration: {duration}";
	}
}

public class AttackSpeedDownCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.AttackSpeedDown;
	public override BuffType buffType => BuffType.Debuff;

	public override string iconPath => "";

	public override string effectPath => "";

	private AttackSpeedDownConditionData conditionData;

	/// <summary>
	/// 공격속도 감소비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public AttackSpeedDownCondition(Character _attacker, AttackSpeedDownConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
