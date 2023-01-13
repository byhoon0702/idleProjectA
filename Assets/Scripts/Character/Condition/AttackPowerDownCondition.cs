﻿[System.Serializable]
public class AttackPowerDownConditionData : ConditionDataBase
{
	/// <summary>
	/// 공격력 감소비율
	/// </summary>
	public float ratio = 0.5f;


	public AttackPowerDownConditionData()
	{

	}

	public AttackPowerDownConditionData(float _ratio)
	{
		ratio = _ratio;
	}

	public override object Clone()
	{
		return new AttackPowerDownConditionData(ratio);
	}
	public override string ToString()
	{
		return $"[AttackPowerDown] ratio: {ratio}";
	}
}

public class AttackPowerDownCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.AttackPowerDown;
	public override BuffType buffType => BuffType.Debuff;
	public override string iconPath => "";

	public override string effectPath => "";

	private AttackPowerDownConditionData conditionData;

	/// <summary>
	/// 공격력 감소비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public AttackPowerDownCondition(Character _attacker, AttackPowerDownConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}
	public override void Start()
	{
		duration = ConditionUtility.GetDefaultDebuffDuration(character.info.data.grade);
		base.Start();
	}
}
