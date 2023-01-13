
[System.Serializable]
public class DamageUpConditionData : ConditionDataBase
{
	/// <summary>
	/// 피해증가량
	/// </summary>
	public float ratio = 0.5f;


	public DamageUpConditionData()
	{

	}

	public DamageUpConditionData(float _ratio)
	{
		ratio = _ratio;
	}

	public override object Clone()
	{
		return new DamageUpConditionData(ratio);
	}

	public override string ToString()
	{
		return $"[DamageUp] ratio: {ratio}";
	}
}

public class DamageUpCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.DamageUp;
	public override BuffType buffType => BuffType.Debuff;

	public override string iconPath => "";

	public override string effectPath => "";

	private DamageUpConditionData conditionData;

	/// <summary>
	/// 피해증가량
	/// </summary>
	public float ratio => conditionData.ratio;


	public DamageUpCondition(Character _attacker, DamageUpConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}

	public override void Start()
	{
		duration = ConditionUtility.GetDefaultDebuffDuration(character.info.data.grade);
		base.Start();
	}
}
