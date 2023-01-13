[System.Serializable]
public class CriticalChanceUpConditionData : ConditionDataBase
{
	/// <summary>
	/// 크리티컬 확률 증가비율
	/// </summary>
	public float ratio = 0.5f;


	public CriticalChanceUpConditionData()
	{

	}

	public CriticalChanceUpConditionData(float _ratio)
	{
		ratio = _ratio;
	}

	public override object Clone()
	{
		return new CriticalChanceUpConditionData(ratio);
	}
	public override string ToString()
	{
		return $"[CriticalChanceUp] ratio: {ratio}";
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


	public CriticalChanceUpCondition(Character _attacker, CriticalChanceUpConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}
	public override void Start()
	{
		duration = ConditionUtility.GetDefaultBuffDuration(character.info.data.grade);
		base.Start();
	}
}
