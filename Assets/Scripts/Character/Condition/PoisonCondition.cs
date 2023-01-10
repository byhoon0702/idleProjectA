using UnityEngine;

[System.Serializable]
public class PoisonConditionData
{
	/// <summary>
	/// 몇초에 한번
	/// </summary>
	public const float TICK = 1;


	/// <summary>
	/// 초당 들어가는 대미지
	/// </summary>
	public float tickAttackPowerMul = 0.1f;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration = 5;


	public PoisonConditionData()
	{

	}

	public PoisonConditionData(float _tickAttackPowerUpMul, float _duration)
	{
		tickAttackPowerMul = _tickAttackPowerUpMul;
		duration = _duration;
	}
}

public class PoisonCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.Poison;

	public override string iconPath => "";

	public override string effectPath => "";

	private PoisonConditionData conditionData;
	private float lastTickAttackPower;

	public PoisonCondition(Character _attacker, PoisonConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
		lastTickAttackPower = _conditionData.duration;
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		if (this.lastTickAttackPower - PoisonConditionData.TICK >= this.remainTime)
		{
			this.lastTickAttackPower -= PoisonConditionData.TICK;
			SkillUtility.SimpleAttack(attacker, character, attacker.info.AttackPower() * conditionData.tickAttackPowerMul, conditionName, Color.blue, false);
		}
	}
}
