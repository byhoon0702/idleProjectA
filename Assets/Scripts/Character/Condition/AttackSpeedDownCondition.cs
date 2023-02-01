[System.Serializable]
public class AttackSpeedDownConditionData : ConditionDataBase
{
	/// <summary>
	/// 공격속도 증가비율
	/// </summary>
	public float ratio = 0.5f;


	public AttackSpeedDownConditionData()
	{

	}

	public AttackSpeedDownConditionData(float _ratio)
	{
		ratio = _ratio;
	}

	public override object Clone()
	{
		return new AttackSpeedDownConditionData(ratio);
	}
	public override string ToString()
	{
		return $"[AttackSpeedDown] ratio: {ratio}";
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


	public AttackSpeedDownCondition(UnitBase _attacker, AttackSpeedDownConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}
	public override void Start()
	{
		duration = ConditionUtility.GetDefaultDebuffDuration(character.info.data.grade);
		base.Start();
	}
}
