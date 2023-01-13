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


	public AttackSpeedUpConditionData()
	{

	}

	public AttackSpeedUpConditionData(float _ratio)
	{
		ratio = _ratio;
	}

	public override object Clone()
	{
		return new AttackSpeedUpConditionData(ratio);
	}
	public override string ToString()
	{
		return $"[AttackSpeedUp] ratio: {ratio}";
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


	public AttackSpeedUpCondition(Character _attacker, AttackSpeedUpConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}

	public override void Start()
	{
		duration = ConditionUtility.GetDefaultBuffDuration(character.info.data.grade);
		base.Start();
	}
}
