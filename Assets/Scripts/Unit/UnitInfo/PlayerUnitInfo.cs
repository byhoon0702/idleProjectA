using UnityEngine;


public class PlayerUnitInfo : UnitInfo
{
	public override string resource
	{
		get
		{


			return "player_01_old";
		}
	}
	public override UnitAdvancementInfo upgradeInfo
	{
		get
		{
			if (data.upgradeInfoList == null)
			{
				return new UnitAdvancementInfo();
			}
			if (data.upgradeInfoList.Count == 0)
			{
				return new UnitAdvancementInfo();
			}
			int youthLevel = GameManager.UserDB.advancementContainer.Info.Level - 1;

			if (youthLevel < data.upgradeInfoList.Count)
			{
				return GameManager.UserDB.advancementContainer.Info.advancementInfo;
			}

			return data.upgradeInfoList[data.upgradeInfoList.Count - 1];
		}
	}

	public long defaultSkillTid
	{
		get
		{
			if (upgradeInfo == null || upgradeInfo.level == 0 || upgradeInfo.skillTid == 0)
			{
				return data.skillTid;
			}

			return upgradeInfo.skillTid;
		}
	}

	public int skillLevel = 5;
	public float searchRange
	{
		get
		{
			return GameManager.Config.PLAYER_TARGET_RANGE_CLOSE;
		}
	}

	public PlayerUnitInfo(Unit _owner, UnitData _data) : base(_owner, _data)
	{
		stats = GameManager.UserDB.UserStats;

		//GameManager.UserDB.advancementContainer.RemoveModifiers(stats, _owner);
		//GameManager.UserDB.advancementContainer.AddModifiers(stats, _owner);

		maxHp = stats.GetValue(StatsType.Hp);
		prevMaxHp = maxHp;
		hp = maxHp;


	}

	public override IdleNumber AttackPower()
	{
		IdleNumber basePower = stats.GetValue(StatsType.Atk);
		IdleNumber final = stats.GetValue(StatsType.Final_Damage_Buff);

		IdleNumber total = basePower;
		if (final > 0)
		{
			total = basePower * (1 + (final / 100f));
		}

		return total;
	}

	/// <summary>
	/// 틱당 체력회복량
	/// </summary>
	public override IdleNumber HPRecovery()
	{
		IdleNumber itemBuffRatio = stats.GetValue(StatsType.Hp_Recovery);//Inventory.it.abilityCalculator.GetAbilityValue(Ability.HpRecovery);

		return itemBuffRatio;
	}

	/// <summary>
	/// 공격속도
	/// </summary>
	public override float AttackSpeed()
	{

		float basicSpeed = stats.GetValue(StatsType.Atk_Speed);

		float total = basicSpeed;

		total = Mathf.Clamp(total, GameManager.Config.ATTACK_SPEED_MIN, GameManager.Config.ATTACK_SPEED_MAX);
		return total;
	}
	public void UpdateMaxHP(PlayerUnit player)
	{
		maxHp = stats.GetValue(StatsType.Hp);
		if (prevMaxHp < maxHp)
		{
			player.Hp += maxHp - prevMaxHp;

			prevMaxHp = maxHp;
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

		float total = stats.GetValue(StatsType.Crits_Chance).GetValueFloat();


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
		float total = 1 + stats.GetValue(StatsType.Crits_Damage).GetValueFloat();

		return total;
	}

	/// <summary>
	/// 이동속도
	/// </summary>
	/// <returns></returns>
	public override float MoveSpeed()
	{
		float basicSpeed = stats.GetValue(StatsType.Move_Speed) * 3f;
		float total = basicSpeed;

		return total;
	}
}
