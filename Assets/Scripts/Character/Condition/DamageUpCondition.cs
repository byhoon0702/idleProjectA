﻿
[System.Serializable]
public class DamageUpConditionData : ConditionDataBase
{
	/// <summary>
	/// 피해증가량
	/// </summary>
	public float ratio = 0.5f;

	/// <summary>
	/// 시간
	/// </summary>
	public float duration = 5;


	public DamageUpConditionData()
	{

	}

	public DamageUpConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}

	public override object Clone()
	{
		return new DamageUpConditionData(ratio, duration);
	}

	public override string ToString()
	{
		return $"[DamageUp] ratio: {ratio}, duration: {duration}";
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


	public DamageUpCondition(Character _attacker, DamageUpConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
