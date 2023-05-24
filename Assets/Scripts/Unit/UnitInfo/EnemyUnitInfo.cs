using UnityEngine;

public class EnemyUnitInfo : UnitInfo
{
	public int skillLevel = 5;
	public int unitLevel = 1;
	public float searchRange
	{
		get
		{
			if (owner.isBoss)
			{
				return GameManager.Config.BOSS_TARGET_RANGE_CLOSE;
			}
			else
			{
				return GameManager.Config.ENEMY_TARGET_RANGE_CLOSE;
			}
		}
	}



	public EnemyUnitInfo(Unit _owner, UnitData _data, StageInfo _stageInfo) : base(_owner, _data)
	{
		//unitLevel = _stageInfo.StageLv;

		CalculateBaseAttackPowerAndHp(_stageInfo);

		//SetProjectile(data.skillEffectTidNormal);
	}

	public override IdleNumber AttackPower()
	{


		IdleNumber total = attackPower;//= stats[Ability.Attackpower].GetValue() * multifly;

		return total;
	}

	/// <summary>
	/// 공격속도
	/// </summary>
	public override float AttackSpeed()
	{

		float total = 1;

		total = Mathf.Clamp(total, GameManager.Config.ATTACK_SPEED_MIN, GameManager.Config.ATTACK_SPEED_MAX);

		return total;
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

		float total = rawStatus[Ability.CriticalChance].value.GetValueFloat();


		if (total > GameManager.Config.CRITICAL_CHANCE_MAX_RATIO)
		{
			total = GameManager.Config.CRITICAL_CHANCE_MAX_RATIO;
		}

		return total;
	}

	/// <summary>
	/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
	/// </summary>
	public override float CriticalDamageMultifly()
	{
		float total = 1 + rawStatus[Ability.CriticalDamage].value.GetValueFloat();

		return total;
	}

	/// <summary>
	/// 이동속도
	/// </summary>
	/// <returns></returns>
	public override float MoveSpeed()
	{
		return 1;

	}



	protected void CalculateBaseAttackPowerAndHp(StageInfo _stageInfo)
	{
		maxHp = _stageInfo.UnitHP(owner.isBoss);
		attackPower = _stageInfo.UnitAttackPower(owner.isBoss);
		hp = maxHp;
	}
}
