[System.Serializable]
public class CriticalChanceUpConditionData : ConditionDataBase
{
	/// <summary>
	/// 크리티컬 확률 증가비율
	/// </summary>
	public float ratio = 0.5f;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration = 5;


	public CriticalChanceUpConditionData()
	{

	}

	public CriticalChanceUpConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}

	public override object Clone()
	{
		return new CriticalChanceUpConditionData(ratio, duration);
	}
	public override string ToString()
	{
		return $"[CriticalChanceUp] ratio: {ratio}, duration: {duration}";
	}
}

public class CriticalChanceUpCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.CriticalChanceUp;
	public override BuffType buffType => BuffType.Buff;

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
