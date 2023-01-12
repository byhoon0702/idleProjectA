using UnityEngine;

[System.Serializable]
public class KnockbackConditionData : ConditionDataBase
{
	public KnockbackConditionData()
	{

	}

	public override object Clone()
	{
		return new KnockbackConditionData();
	}

	public override string ToString()
	{
		return $"[Knockback]";
	}
}
public class KnockbackCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.Knockback;
	public override BuffType buffType => BuffType.Debuff;
	public override string iconPath => null;
	public override string effectPath => null;

	private KnockbackConditionData conditionData;
	private float distance;


	public KnockbackCondition(Character _attacker, KnockbackConditionData _conditionData)
		: base(_attacker, ConfigMeta.it.KNOCKBACK_DURATION)
	{
		conditionData = _conditionData;
		distance = ConfigMeta.it.KNOCKBACK_DISTANCE;
	}

	public override void Start()
	{
		base.Start();
		character.KnockbackStart();
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float finalVelX = Mathf.Lerp(0, distance, dt * 5);
		if (character is PlayerCharacter)
		{
			character.transform.Translate(Vector3.left * finalVelX);
		}
		else
		{
			character.transform.Translate(Vector3.right * finalVelX);
		}
		distance -= finalVelX;
	}
}
