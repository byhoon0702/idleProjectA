﻿[System.Serializable]
public class CriticalChanceDownConditionData : ConditionDataBase
{
	/// <summary>
	/// 크리티컬 확률 감소 비율
	/// </summary>
	public float ratio = 0.5f;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration = 5;


	public CriticalChanceDownConditionData()
	{

	}

	public CriticalChanceDownConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}

	public override object Clone()
	{
		return new CriticalChanceDownConditionData(ratio, duration);
	}
	public override string ToString()
	{
		return $"[CriticalChanceDown] ratio: {ratio}, duration: {duration}";
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


	public CriticalChanceDownCondition(Character _attacker, CriticalChanceDownConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
