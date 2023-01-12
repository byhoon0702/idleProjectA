using System;
using UnityEngine;

[System.Serializable]
public class DoteConditionData : ConditionDataBase
{
	/// <summary>
	/// 몇초에 한번
	/// </summary>
	public float tick = 1;

	/// <summary>
	/// 초당 들어가는 대미지
	/// </summary>
	public float tickAttackPowerMul = 0.1f;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration = 5;

	/// <summary>
	/// 적용속성
	/// </summary>
	public ElementType elementType;


	public DoteConditionData()
	{

	}

	public DoteConditionData(ElementType _elementType, float _tickAttackPowerUpMul, float _tick, float _duration)
	{
		elementType = _elementType;
		tick = _tick;
		tickAttackPowerMul = _tickAttackPowerUpMul;
		duration = _duration;
	}

	public override object Clone()
	{
		return new DoteConditionData(elementType, tickAttackPowerMul, tick, duration);
	}

	public override string ToString()
	{
		return $"[Dote] tick: {tick}, tickAttackPowerMul: {tickAttackPowerMul}, duration: {duration}, element: {elementType}";
	}
}

public class DoteCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.Dote;
	public override BuffType buffType => BuffType.Debuff;

	public override string iconPath => "";

	public override string effectPath => "";

	private DoteConditionData conditionData;
	private float lastTickAttackPower;

	public DoteCondition(Character _attacker, DoteConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
		lastTickAttackPower = _conditionData.duration;
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		if (this.lastTickAttackPower - conditionData.tick >= this.remainTime)
		{
			this.lastTickAttackPower -= conditionData.tick;
			SkillUtility.SimpleAttack(attacker, character, attacker.info.AttackPower() * conditionData.tickAttackPowerMul, conditionName, Color.blue, false);
		}
	}
}
