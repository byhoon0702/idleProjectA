using System;
using UnityEngine;

[Serializable]
public class AttackSpeedUpConditionData : ConditionDataBase
{
	/// <summary>
	/// 공격속도 증가비율
	/// </summary>
	[Tooltip("공격속도 증가비율")]
	[SerializeField] public float ratio = 0.5f;

	/// <summary>
	/// 지속시간
	/// </summary>
	[Tooltip("지속시간")]
	[SerializeField] public float duration = 5;


	public AttackSpeedUpConditionData()
	{

	}

	public AttackSpeedUpConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}

	public override object Clone()
	{
		return new AttackSpeedUpConditionData(ratio, duration);
	}
	public override string ToString()
	{
		return $"[AttackSpeedUp] ratio: {ratio}, duration: {duration}";
	}
}

public class AttackSpeedUpCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.AttackSpeedUp;
	public override BuffType buffType => BuffType.Buff;

	public override string iconPath => "";

	public override string effectPath => "";

	private AttackSpeedUpConditionData conditionData;

	/// <summary>
	/// 공격속도 증가비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public AttackSpeedUpCondition(Character _attacker, AttackSpeedUpConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
