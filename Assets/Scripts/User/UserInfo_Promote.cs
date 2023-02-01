using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UserInfo
{
	public static List<AbilityType> promoteTypes = new List<AbilityType>()
	{
		AbilityType.AttackPower,
		AbilityType.Hp,
		AbilityType.CriticalAttackPower,
		AbilityType.SkillAttackPower,
		AbilityType.BossAttackPower
	};

	public class HyperModeSave : UserInfoLevelSaveBase
	{
		public override int defaultLevel => 0;


		public override double TotalCombatPower()
		{
			double total = 0;

			total += GetLevel(AbilityType.AttackPower) * 1000;
			total += GetLevel(AbilityType.Hp) * 1000;
			total += GetLevel(AbilityType.CriticalAttackPower) * 1000;
			total += GetLevel(AbilityType.SkillAttackPower) * 1000;
			total += GetLevel(AbilityType.BossAttackPower) * 1000;

			return total;
		}
	}

	public class HyperModeInfo
	{
		/// <summary>
		/// 레벨업 가능한 최대레벨
		/// </summary>
		public Int32 promoteMaxLevel => GetPromoteMaxLevel(proAbil.userGrade);



		/// <summary>
		/// 현재 진급 레벨
		/// </summary>
		public Int32 GetPromoteLevel(AbilityType _abilityType)
		{
			return userData.promo.GetLevel(_abilityType);
		}

		/// <summary>
		/// 진급 최대레벨
		/// </summary>
		public Int32 GetPromoteMaxLevel(Grade _grade)
		{
			Int32 level = DataManager.it.Get<HyperModeDataSheet>().Get(_grade).abilityLevel;

			return level;
		}

		/// <summary>
		/// 진급 버프
		/// </summary>
		public List<AbilityInfo> GetPromoteValue()
		{
			float attackPowerRatio = userData.promo.GetLevel(AbilityType.AttackPower) * ConfigMeta.it.PROMOTE_ATTACK_POWER_PER_LEVEL_RATIO;
			float hpUpRatio = userData.promo.GetLevel(AbilityType.Hp) * ConfigMeta.it.PROMOTE_HP_PER_LEVEL_RATIO;
			float criticalAttackPowerRatio = userData.promo.GetLevel(AbilityType.CriticalAttackPower) * ConfigMeta.it.PROMOTE_CRITICAL_ATTACK_POWER_PER_LEVEL_RATIO;
			float skillAttackPowerRatio = userData.promo.GetLevel(AbilityType.SkillAttackPower) * ConfigMeta.it.PROMOTE_SKILL_ATTACK_POWER_PER_LEVEL_RATIO;
			float bossAttackPowerRatio = userData.promo.GetLevel(AbilityType.BossAttackPower) * ConfigMeta.it.PROMOTE_BOSS_ATTACK_POWER_PER_LEVEL_RATIO;


			List<AbilityInfo> result = new List<AbilityInfo>();

			result.Add(new AbilityInfo(AbilityType.AttackPower, new IdleNumber(attackPowerRatio)));
			result.Add(new AbilityInfo(AbilityType.Hp, new IdleNumber(hpUpRatio)));
			result.Add(new AbilityInfo(AbilityType.CriticalAttackPower, new IdleNumber(criticalAttackPowerRatio)));
			result.Add(new AbilityInfo(AbilityType.SkillAttackPower, new IdleNumber(skillAttackPowerRatio)));
			result.Add(new AbilityInfo(AbilityType.BossAttackPower, new IdleNumber(bossAttackPowerRatio)));

			return result;
		}

		/// <summary>
		/// 진급 레벨업!
		/// </summary>
		public void LevelUpPromote(AbilityType _abilityType)
		{
			Int32 maxLevel = promoteMaxLevel;
			Int32 currLevel = userData.promo.GetLevel(_abilityType);

			if (currLevel < maxLevel)
			{
				userData.promo.SetLevel(_abilityType, currLevel + 1);
			}

			CalculateTotalCombatPower();
		}


		/// <summary>
		/// 진급 레벨업 비용
		/// '_level'로 올라가기 위한 필요 비용
		/// </summary>
		public IdleNumber GetPromoteLevelupConsumeCount(Int32 _level)
		{
			var consumeInfos = DataManager.it.Get<HyperModeAbilityDataSheet>().GetByLevel(_level);

			return consumeInfos.consumeLightDustCount;
		}
	}
}
