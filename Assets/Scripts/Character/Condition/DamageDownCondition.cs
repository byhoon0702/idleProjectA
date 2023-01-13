[System.Serializable]
public class DamageDownConditionData : ConditionDataBase
{
	/// <summary>
	/// 피해감소량
	/// </summary>
	public float ratio = 0.5f;


	public DamageDownConditionData()
	{

	}

	public DamageDownConditionData(float _ratio)
	{
		ratio = _ratio;
	}

	public override object Clone()
	{
		return new DamageDownConditionData(ratio);
	}
	public override string ToString()
	{
		return $"[DamageDown] ratio: {ratio}";
	}
}

public class DamageDownCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.DamageDown;
	public override BuffType buffType => BuffType.Buff;

	public override string iconPath => "";

	public override string effectPath => "";

	private DamageDownConditionData conditionData;

	/// <summary>
	/// 피해 감소량
	/// </summary>
	public float ratio => conditionData.ratio;


	public DamageDownCondition(Character _attacker, DamageDownConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}
	public override void Start()
	{
		duration = ConditionUtility.GetDefaultBuffDuration(character.info.data.grade);
		base.Start();
	}
}
