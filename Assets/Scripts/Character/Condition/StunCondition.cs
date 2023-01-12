[System.Serializable]
public class StunConditionData : ConditionDataBase
{
	/// <summary>
	/// 스턴시간
	/// </summary>
	public float duration = 5;


	public StunConditionData()
	{

	}

	public StunConditionData(float _duration)
	{
		duration = _duration;
	}

	public override object Clone()
	{
		return new StunConditionData(duration);
	}

	public override string ToString()
	{
		return $"[Stun] duration: {duration}";
	}
}

public class StunCondition : ConditionBase
{

	public override CharacterCondition conditionType => CharacterCondition.Stun;
	public override BuffType buffType => BuffType.Debuff;

	public override string iconPath => "";

	public override string effectPath => "";

	private StunConditionData conditionData;

	public StunCondition(Character _attacker, StunConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}

	public override void Start()
	{
		base.Start();
		character.StunStart();
	}
}
