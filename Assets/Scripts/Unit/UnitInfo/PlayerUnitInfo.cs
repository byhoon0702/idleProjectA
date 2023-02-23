using UnityEngine;


public class PlayerUnitInfo : UnitInfo
{
	public int skillLevel = 5;
	public float searchRange
	{
		get
		{
			return ConfigMeta.it.PLAYER_TARGET_RANGE_CLOSE;
		}
	}

	public PlayerUnitInfo(Unit _owner, UnitData _data) : base(_owner, _data)
	{
		CalculateBaseAttackPowerAndHp();

		maxHP = CalculateMaxHP();
		hp = maxHP;
		//status["Hp"].value = maxHP;

		SetProjectile(data.skillEffectTidNormal);
	}

	public override IdleNumber AttackPower()
	{
		// 공식 (★★★ 변경시 같이 업데이트해주세요 ★★★)
		//(unitPower) * (합 버프) * (blessingBuffRatio) * (hyperBonus)

		// 아이템 장착(무기, 악세, 방어구)버프, 아이템 보유버프
		IdleNumber itemBuffRatio = Inventory.it.abilityCalculator.GetAbilityValue(Stats.Attackpower);

		// 드라이브 개조버프
		IdleNumber driveBuffRatio = new IdleNumber();

		// 회로 연결 버프
		IdleNumber chainBuffRatio = new IdleNumber();

		// 도감 버프
		IdleNumber bookBuffRatio = new IdleNumber();

		// 특성, 특성 시너지 버프
		IdleNumber specificityRatio = new IdleNumber();

		// 용사의 기록 버프
		IdleNumber recordBuffRatio = new IdleNumber();

		// 서포트 시스템 버프
		IdleNumber supportBuffRatio = new IdleNumber();

		// 광고 보너스 버프
		IdleNumber adBuffRatio = new IdleNumber();

		// 상태이상 적용
		IdleNumber conditionTotalRatio = new IdleNumber(owner.conditionModule.ability.attackPowerUpRatio - owner.conditionModule.ability.attackPowerDownRatio);

		// 축복 보너스 버프
		float blessingBuffMul = 1;

		// 하이퍼모드 보너스
		float hyperBonusMul = 1 + UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, Stats.Attackpower);


		// 합 연산
		IdleNumber multifly = (IdleNumber)1 + itemBuffRatio + driveBuffRatio + chainBuffRatio + bookBuffRatio + specificityRatio + recordBuffRatio + supportBuffRatio + adBuffRatio + conditionTotalRatio;
		// 곱 연산
		multifly = multifly * blessingBuffMul * hyperBonusMul;

		IdleNumber total = rawAttackPower * multifly;

		return total;
	}

	/// <summary>
	/// 최대체력은 실시간으로 계산하지 않는다.
	/// (아이템 장착등의 변경이 있거나, 어빌리티를 올렸을때만 체크해줄 예정)
	/// </summary>
	public IdleNumber CalculateMaxHP()
	{
		// 공식 (★★★ 변경시 같이 업데이트해주세요 ★★★)
		//(hp) * (합 버프)

		// 아이템 장착(무기, 악세, 방어구)버프, 아이템 보유버프
		IdleNumber itemBuffRatio = Inventory.it.abilityCalculator.GetAbilityValue(Stats.Hp);

		// 드라이브 개조버프
		IdleNumber driveBuffRatio = new IdleNumber();

		// 회로 연결 버프
		IdleNumber chainBuffRatio = new IdleNumber();

		// 도감 버프
		IdleNumber bookBuffRatio = new IdleNumber();

		// 특성, 특성 시너지 버프
		IdleNumber specificityRatio = new IdleNumber();

		// 용사의 기록 버프
		IdleNumber recordBuffRatio = new IdleNumber();

		// 서포트 시스템 버프
		IdleNumber supportBuffRatio = new IdleNumber();

		// 광고 보너스 버프
		IdleNumber adBuffRatio = new IdleNumber();

		// 합 연산
		IdleNumber multifly = (IdleNumber)1 + itemBuffRatio + driveBuffRatio + chainBuffRatio + bookBuffRatio + specificityRatio + recordBuffRatio + supportBuffRatio + adBuffRatio;

		IdleNumber total = rawHp * multifly;

		return total;
	}

	/// <summary>
	/// 틱당 체력회복량
	/// </summary>
	public override IdleNumber HPRecovery()
	{
		IdleNumber itemBuffRatio = Inventory.it.abilityCalculator.GetAbilityValue(Stats.HpRecovery);

		return itemBuffRatio;
	}

	/// <summary>
	/// 공격속도
	/// </summary>
	public override float AttackSpeedMul()
	{
		float condition = owner.conditionModule.ability.attackSpeedUpRatio - owner.conditionModule.ability.attackSpeedDownRatio;
		float hyperBonusRatio = UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, Stats.AttackSpeed);
		float total = status[Stats.AttackSpeed].value.GetValueFloat() + condition + hyperBonusRatio;

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



	public PlayerAttackType GetAttackType()
	{
		if (TripleAttack())
		{
			return PlayerAttackType.TripleAttack;
		}
		else if (DoubleAttack())
		{
			return PlayerAttackType.DoubleAttack;
		}
		else
		{
			return PlayerAttackType.Attack;
		}
	}

	private bool TripleAttack()
	{
		//IdleNumber itemBuffRatio = Inventory.it.abilityCalculator.GetAbilityValue(Stats.TripleAttack) * 0.01f;
		//bool result = SkillUtility.Cumulative(itemBuffRatio.GetValueFloat());
		return false;
		//return result;
	}

	private bool DoubleAttack()
	{
		return false;
		//IdleNumber itemBuffRatio = Inventory.it.abilityCalculator.GetAbilityValue(Stats.DoubleAttack) * 0.01f;
		//bool result = SkillUtility.Cumulative(itemBuffRatio.GetValueFloat());

		//return result;
	}

	public override float CriticalChanceRatio()
	{
		float conditionTotalRatio = owner.conditionModule.ability.criticalChanceUpRatio - owner.conditionModule.ability.criticalChanceDownRatio;
		float hyperBonusRatio = UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, Stats.CriticalChance);
		float total = data.criticalRate + conditionTotalRatio + hyperBonusRatio;


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
		float hyperBonusRatio = UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, Stats.Movespeed);
		float mul = 1 + conditionTotalRatio + hyperBonusRatio;

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
