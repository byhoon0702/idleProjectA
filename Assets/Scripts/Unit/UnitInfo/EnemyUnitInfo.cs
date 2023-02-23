using UnityEngine;

public class EnemyUnitInfo : UnitInfo
{
	public int skillLevel = 5;
	public float searchRange
	{
		get
		{
			if (owner.isBoss)
			{
				return ConfigMeta.it.BOSS_TARGET_RANGE_CLOSE;
			}
			else
			{
				return ConfigMeta.it.ENEMY_TARGET_RANGE_CLOSE;
			}
		}
	}

	public EnemyUnitInfo(Unit _owner, UnitData _data, int _level = 1) : base(_owner, _data)
	{


		CalculateBaseAttackPowerAndHp(_level);

		SetProjectile(data.skillEffectTidNormal);
	}

	public override IdleNumber AttackPower()
	{
		float conditionTotalRatio = owner.conditionModule.ability.attackPowerUpRatio - owner.conditionModule.ability.attackPowerDownRatio;
		float multifly = 1 + conditionTotalRatio;

		IdleNumber total = attackPower * multifly;

		return total;
	}

	/// <summary>
	/// 공격속도
	/// </summary>
	public override float AttackSpeedMul()
	{
		float condition = owner.conditionModule.ability.attackSpeedUpRatio - owner.conditionModule.ability.attackSpeedDownRatio;
		float total = 1 + condition;

		total = Mathf.Clamp(total, ConfigMeta.it.ATTACK_SPEED_MIN, ConfigMeta.it.ATTACK_SPEED_MAX);

		return total;
	}

	/// <summary>
	/// 크리티컬 발동여부. true면 크리티컬로 처리하면 됨
	/// </summary>
	public override CriticalType IsCritical()
	{
		float total = CriticalChanceRatio();
		bool isCritical = SkillUtility.Cumulative(total);
		bool isCriticalX2 = SkillUtility.Cumulative(total);

		if (isCriticalX2 && isCritical)
		{
			return CriticalType.CriticalX2;
		}
		else if (isCritical)
		{
			return CriticalType.Critical;
		}
		else
		{
			return CriticalType.Normal;
		}
	}

	public float attackTime
	{
		get
		{
			return 1;
		}
	}

	public override float CriticalChanceRatio()
	{
		float conditionTotalRatio = owner.conditionModule.ability.criticalChanceUpRatio - owner.conditionModule.ability.criticalChanceDownRatio;
		float total = data.criticalRate + conditionTotalRatio;


		if (total > ConfigMeta.it.CRITICAL_CHANCE_MAX_RATIO)
		{
			total = ConfigMeta.it.CRITICAL_CHANCE_MAX_RATIO;
		}

		return total;
	}

	/// <summary>
	/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
	/// </summary>
	public override float CriticalDamageMultifly()
	{
		float total = 1 + data.criticalPowerRate;

		return total;
	}

	/// <summary>
	/// 이동속도
	/// </summary>
	/// <returns></returns>
	public override float MoveSpeed()
	{
		float conditionTotalRatio = owner.conditionModule.ability.moveSpeedUpRatio - owner.conditionModule.ability.moveSpeedDownRatio;
		float mul = 1 + conditionTotalRatio;

		IdleNumber i = status[Stats.Movespeed].value + (IdleNumber)1;
		float total = (float)i.Value;

		return total;
	}

	/// <summary>
	/// true면 컨디션 적용 안됨
	/// </summary>
	public bool ConditionApplicable(ConditionBase _condition)
	{
		switch (_condition.conditionType)
		{
			case UnitCondition.Knockback:
				//if (data.rankType == RankType.BOSS)
				{
					return false;
				}
				break;
		}

		return true;
	}
}
