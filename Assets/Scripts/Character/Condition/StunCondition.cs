[System.Serializable]
public class StunConditionData : ConditionDataBase
{
	public StunConditionData()
	{

	}

	public override object Clone()
	{
		return new StunConditionData();
	}

	public override string ToString()
	{
		return $"[Stun]";
	}
}

public class StunCondition : ConditionBase
{

	public override CharacterCondition conditionType => CharacterCondition.Stun;
	public override BuffType buffType => BuffType.Debuff;

	public override string iconPath => "";

	public override string effectPath => "";

	private StunConditionData conditionData;


	public StunCondition(Character _attacker, StunConditionData _conditionData) : base(_attacker, ConfigMeta.it.STUN_DURATION)
	{
		conditionData = _conditionData;
	}

	public override void Start()
	{
		base.Start();
		character.StunStart();
	}
}
