using UnityEngine;

public class CharacterInfo
{
	public Character owner;
	public UnitData data;
	public UnitData rawData;
	public ControlSide controlSide;
	public ClassData jobData;
	public RaceData raceData;
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

	public CharacterInfo(Character _owner, UnitData _data, ControlSide _controlSide)
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
		if (DataManager.it.Get<ClassDataSheet>() != null)
		{
			jobData = DataManager.it.Get<ClassDataSheet>().Get(data.classTid);
		}

		if (DataManager.it.Get<RaceDataSheet>() != null)
		{
			raceData = DataManager.it.Get<RaceDataSheet>().Get(data.raceTid);
		}

	}

	public IdleNumber AttackPower(bool _random = true)
	{
		float conditionTotalRatio = owner.conditionModule.ability.attackPowerUpRatio - owner.conditionModule.ability.attackPowerDownRatio;
		float multifly = 1 + conditionTotalRatio;

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
	public float AttackSpeedMul()
	{
		float total = 1 + owner.conditionModule.ability.attackSpeedUpRatio - owner.conditionModule.ability.attackSpeedDownRatio;

		total = Mathf.Clamp(total, ConfigMeta.it.ATTACK_SPEED_MIN, ConfigMeta.it.ATTACK_SPEED_MAX);

		return total;
	}

	/// <summary>
	/// 받는 피해량
	/// </summary>
	/// <returns></returns>
	public float DamageMul()
	{
		float conditionTotalRatio = owner.conditionModule.ability.damageUpRatio - owner.conditionModule.ability.damageDownRatio;
		float total = 1 + conditionTotalRatio;

		total = Mathf.Clamp(total, ConfigMeta.it.MIN_DAMAGE_MUL, ConfigMeta.it.MAX_DAMAGE_MUL);

		return total;
	}

	/// <summary>
	/// 크리티컬 발동여부. true면 크리티컬로 처리하면 됨
	/// </summary>
	public bool IsCritical()
	{
		float total = CriticalChanceRatio();
		return SkillUtility.Cumulative(total);
	}

	public float attackTime
	{
		get
		{
			return 1;
		}
	}

	public float CriticalChanceRatio()
	{
		float conditionTotalRatio = owner.conditionModule.ability.criticalChanceUpRatio - owner.conditionModule.ability.criticalChanceDownRatio;
		float total = jobData.criticalRate + conditionTotalRatio;


		if (total > ConfigMeta.it.CRITICAL_CHANCE_MAX_RATIO)
		{
			total = ConfigMeta.it.CRITICAL_CHANCE_MAX_RATIO;
		}

		return total;
	}

	/// <summary>
	/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
	/// </summary>
	public float CriticalDamageMultifly()
	{
		float total = 1 + jobData.criticalPowerRate;

		return total;
	}

	/// <summary>
	/// 이동속도
	/// </summary>
	/// <returns></returns>
	public float MoveSpeed()
	{
		float conditionTotalRatio = owner.conditionModule.ability.moveSpeedUpRatio - owner.conditionModule.ability.moveSpeedDownRatio;
		float mul = 1 + conditionTotalRatio;

		float total = jobData.moveSpeed * mul;

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
			case CharacterCondition.Stun:
				if (data.rankType == RankType.BOSS || data.rankType == RankType.FINISH_GEM)
				{
					return false;
				}
				break;
		}

		return true;
	}
}
