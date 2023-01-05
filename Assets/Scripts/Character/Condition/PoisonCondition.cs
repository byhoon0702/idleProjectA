using UnityEngine;


public class PoisonConditionData
{
	/// <summary>
	/// 몇초에 한번
	/// </summary>
	public const float TICK = 1;


	/// <summary>
	/// 초당 들어가는 대미지
	/// </summary>
	public float tickDamageMul;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration;


	public PoisonConditionData(float _tickDamageMul, float _duration)
	{
		tickDamageMul = _tickDamageMul;
		duration = _duration;
	}
}

public class PoisonCondition : ConditionBase
{
	public override UnitCondition conditionType => UnitCondition.Poison;

	public override string iconPath => "";

	public override string effectPath => "";

	private PoisonConditionData conditionData;
	private float lastTickDamage;

	public PoisonCondition(Character _attacker, PoisonConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
		lastTickDamage = _conditionData.duration;
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		if (this.lastTickDamage - PoisonConditionData.TICK >= this.remainTime)
		{
			this.lastTickDamage -= PoisonConditionData.TICK;
			SkillUtility.SimpleDamage(attacker, character, attacker.info.DefaultDamage() * conditionData.tickDamageMul, Color.blue);
		}
	}
}
