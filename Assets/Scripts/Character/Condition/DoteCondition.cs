using System;
using UnityEngine;

[System.Serializable]
public class DoteConditionData : ConditionDataBase
{
	/// <summary>
	/// 초당 들어가는 대미지
	/// </summary>
	public float tickAttackPowerMul = 0.1f;

	/// <summary>
	/// 적용속성
	/// </summary>
	public ElementType elementType;


	public DoteConditionData()
	{

	}

	public DoteConditionData(ElementType _elementType, float _tickAttackPowerUpMul)
	{
		elementType = _elementType;
		tickAttackPowerMul = _tickAttackPowerUpMul;
	}

	public override object Clone()
	{
		return new DoteConditionData(elementType, tickAttackPowerMul);
	}

	public override string ToString()
	{
		return $"[Dote] tickAttackPowerMul: {tickAttackPowerMul}, element: {elementType}";
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

	public DoteCondition(Character _attacker, DoteConditionData _conditionData) : base(_attacker, 0)
	{
		conditionData = _conditionData;
	}

	public override void Start()
	{
		duration = ConditionUtility.GetDefaultDoteDuration(character.info.data.grade);
		base.Start();

		lastTickAttackPower = duration;
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		if (this.lastTickAttackPower - ConfigMeta.it.DOTE_TICK >= this.remainTime)
		{
			this.lastTickAttackPower -= ConfigMeta.it.DOTE_TICK;
			SkillUtility.SimpleAttack(attacker, character, attacker.info.AttackPower() * conditionData.tickAttackPowerMul, conditionName, Color.blue, false);
		}
	}
}
