[System.Serializable]
public class CriticalChanceDownConditionData : ConditionDataBase
{
	/// <summary>
	/// 크리티컬 확률 감소 비율
	/// </summary>
	public float ratio = 0.5f;


	public CriticalChanceDownConditionData()
	{

	}

	public CriticalChanceDownConditionData(float _ratio)
	{
		ratio = _ratio;
	}

	public override object Clone()
	{
		return new CriticalChanceDownConditionData(ratio);
	}
	public override string ToString()
	{
		return $"[CriticalChanceDown] ratio: {ratio}";
	}
}

public class CriticalChanceDownCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.CriticalChanceDown;
	public override BuffType buffType => BuffType.Debuff;

	public override string iconPath => "";

	public override string effectPath => "";


	private CriticalChanceDownConditionData conditionData;

	/// <summary>
	/// 크리티컬 확률 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public CriticalChanceDownCondition(UnitBase _attacker, CriticalChanceDownConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}
	public override void Start()
	{
		duration = ConditionUtility.GetDefaultDebuffDuration(character.info.data.grade);
		base.Start();
	}
}
