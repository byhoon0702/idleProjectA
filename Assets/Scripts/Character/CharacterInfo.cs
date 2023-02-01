using UnityEngine;

public class UnitInfo
{
	public virtual IdleNumber AttackPower(bool _random = true)
	{
		return new IdleNumber();
	}

	/// <summary>
	/// 공격속도
	/// </summary>
	public virtual float AttackSpeedMul()
	{
		return 1;
	}

	public virtual IdleNumber HPRecovery()
	{
		return new IdleNumber();
	}

	/// <summary>
	/// 크리티컬 발동여부
	/// </summary>
	public virtual CriticalType IsCritical()
	{
		return CriticalType.Normal;
	}


	public virtual float CriticalChanceRatio()
	{
		return 1;
	}

	public virtual float CriticalX2ChanceRatio()
	{
		return 1;
	}

	/// <summary>
	/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
	/// </summary>
	public virtual float CriticalDamageMultifly()
	{
		return 1;
	}


	/// <summary>
	/// 크리티컬X2 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
	/// </summary>
	public virtual float CriticalX2DamageMultifly()
	{
		return 1;
	}

	/// <summary>
	/// 이동속도
	/// </summary>
	/// <returns></returns>
	public virtual float MoveSpeed()
	{
		return 1;
	}
}

public class CharacterInfo : UnitInfo
{
	public Unit owner;
	public UnitData data;
	public UnitData rawData;
	public ControlSide controlSide;

	/// <summary>
	/// UI표시용
	/// </summary>
	public string charName => data.name;
	public IdleNumber hp;
	public IdleNumber attackPower;

	public IdleNumber rawHp;
	public IdleNumber rawAttackPower;
	/// <summary>
	/// 캐릭터 ID를 같이 표시하는용(디버깅용)
	/// </summary>
	public string charNameAndCharId => $"{data.name}({owner.charID})";

	public int skillLevel = 5;
	public float searchRange
	{
		get
		{
			if (data.attackType == AttackType.MELEE)
			{
				if (controlSide == ControlSide.PLAYER)
				{
					return ConfigMeta.it.PLAYER_TARGET_RANGE_CLOSE;
				}
				else
				{
					if (owner is BossCharacter)
					{
						return ConfigMeta.it.BOSS_TARGET_RANGE_CLOSE;
					}
					else
					{
						return ConfigMeta.it.ENEMY_TARGET_RANGE_CLOSE;
					}
				}
			}
			else
			{
				if (controlSide == ControlSide.PLAYER)
				{
					return ConfigMeta.it.PLAYER_TARGET_RANGE_FAR;
				}
				else
				{
					if (owner is BossCharacter)
					{
						return ConfigMeta.it.BOSS_TARGET_RANGE_FAR;
					}
					else
					{
						return ConfigMeta.it.ENEMY_TARGET_RANGE_FAR;
					}
				}
			}
		}
	}

	public CharacterInfo(Unit _owner, UnitData _data, ControlSide _controlSide)
	{
		owner = _owner;

		rawData = _data;
		data = _data.Clone();

		hp = (IdleNumber)data.hp;
		attackPower = (IdleNumber)data.attackPower;

		rawHp = (IdleNumber)rawData.hp;
		rawAttackPower = (IdleNumber)rawData.attackPower;
		InitDatas();
		controlSide = _controlSide;
	}

	void InitDatas()
	{
		//if (DataManager.it.Get<ClassDataSheet>() != null)
		//{
		//	jobData = DataManager.it.Get<ClassDataSheet>().Get(data.classTid);
		//}
	}

	public override IdleNumber AttackPower(bool _random = true)
	{
		float conditionTotalRatio = owner.conditionModule.ability.attackPowerUpRatio - owner.conditionModule.ability.attackPowerDownRatio;
		float hyperBonus = UnitGlobal.it.hyperModule.GetHyperAbility(owner, AbilityType.AttackPower);
		float multifly = 1 + conditionTotalRatio + hyperBonus;

		IdleNumber total = attackPower * multifly;

		if (_random)
		{
			total += total * Random.Range(-ConfigMeta.it.ATTACK_POWER_RANGE, ConfigMeta.it.ATTACK_POWER_RANGE);
		}

		return total;
	}

	/// <summary>
	/// 공격속도
	/// </summary>
	public override float AttackSpeedMul()
	{
		float condition = owner.conditionModule.ability.attackSpeedUpRatio - owner.conditionModule.ability.attackSpeedDownRatio;
		float hyperBonus = UnitGlobal.it.hyperModule.GetHyperAbility(owner, AbilityType.AttackSpeed);
		float total = 1 + condition + hyperBonus;

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
		float hyperBonus = UnitGlobal.it.hyperModule.GetHyperAbility(owner, AbilityType.CriticalChance);
		float total = data.criticalRate + conditionTotalRatio + hyperBonus;


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
		float hyperBonus = UnitGlobal.it.hyperModule.GetHyperAbility(owner, AbilityType.MoveSpeed);
		float mul = 1 + conditionTotalRatio + hyperBonus;

		float total = data.moveSpeed * mul;

		return total;
	}

	/// <summary>
	/// true면 컨디션 적용 안됨
	/// </summary>
	public bool ConditionApplicable(ConditionBase _condition)
	{
		switch (_condition.conditionType)
		{
			case CharacterCondition.Knockback:
				if (data.rankType == RankType.BOSS)
				{
					return false;
				}
				break;
		}

		return true;
	}
}
