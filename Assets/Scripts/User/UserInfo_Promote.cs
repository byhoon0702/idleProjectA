using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UserInfo
{
	public class UserPromoteData : UserInfoLevelSaveBase
	{
		public static List<UserAbilityType> relicTypes = new List<UserAbilityType>()
		{
			UserAbilityType.AttackPower,
			UserAbilityType.Hp,
			UserAbilityType.CriticalAttackPower,
			UserAbilityType.SkillAttackPower,
			UserAbilityType.BossAttackPower
		};

		public override int defaultLevel => 0;


		public override double TotalCombatPower()
		{
			double total = 0;

			total += GetLevel(UserAbilityType.AttackPower) * 1000;
			total += GetLevel(UserAbilityType.Hp) * 1000;
			total += GetLevel(UserAbilityType.CriticalAttackPower) * 1000;
			total += GetLevel(UserAbilityType.SkillAttackPower) * 1000;
			total += GetLevel(UserAbilityType.BossAttackPower) * 1000;

			return total;
		}
	}

	public class UserPromoteInfo
	{
		/// <summary>
		/// 레벨업 가능한 최대레벨
		/// </summary>
		public Int32 promoteMaxLevel => GetPromoteMaxLevel(proAbil.userGrade);



		/// <summary>
		/// 현재 진급능력 레벨
		/// </summary>
		public Int32 GetPromoteLevel(UserAbilityType _abilityType)
		{
			return userData.promo.GetLevel(_abilityType);
		}

		/// <summary>
		/// 진급능력 최대레벨
		/// </summary>
		public Int32 GetPromoteMaxLevel(Grade _grade)
		{
			Int32 level = DataManager.it.Get<UserGradeDataSheet>().Get(_grade).abilityLevel;

			return level;
		}

		/// <summary>
		/// 진급능력 버프
		/// </summary>
		public List<UserAbility> GetPromoteValue()
		{
			float attackPowerRatio = userData.promo.GetLevel(UserAbilityType.AttackPower) * ConfigMeta.it.PROMOTE_ATTACK_POWER_PER_LEVEL_RATIO;
			float hpUpRatio = userData.promo.GetLevel(UserAbilityType.Hp) * ConfigMeta.it.PROMOTE_HP_PER_LEVEL_RATIO;
			float criticalAttackPowerRatio = userData.promo.GetLevel(UserAbilityType.CriticalAttackPower) * ConfigMeta.it.PROMOTE_CRITICAL_ATTACK_POWER_PER_LEVEL_RATIO;
			float skillAttackPowerRatio = userData.promo.GetLevel(UserAbilityType.SkillAttackPower) * ConfigMeta.it.PROMOTE_SKILL_ATTACK_POWER_PER_LEVEL_RATIO;
			float bossAttackPowerRatio = userData.promo.GetLevel(UserAbilityType.BossAttackPower) * ConfigMeta.it.PROMOTE_BOSS_ATTACK_POWER_PER_LEVEL_RATIO;


			List<UserAbility> result = new List<UserAbility>();

			result.Add(new UserAbility(UserAbilityType.AttackPower, attackPowerRatio));
			result.Add(new UserAbility(UserAbilityType.Hp, hpUpRatio));
			result.Add(new UserAbility(UserAbilityType.CriticalAttackPower, criticalAttackPowerRatio));
			result.Add(new UserAbility(UserAbilityType.SkillAttackPower, skillAttackPowerRatio));
			result.Add(new UserAbility(UserAbilityType.BossAttackPower, bossAttackPowerRatio));

			return result;
		}

		/// <summary>
		/// 진급능력 레벨업 비용
		/// '_level'로 올라가기 위한 필요 비용
		/// </summary>
		public IdleNumber GetPromoteLevelupConsumeCount(Int32 _level)
		{
			var consumeInfos = DataManager.it.Get<UserPromoteDataSheet>().GetByLevel(_level);

			return consumeInfos.consumeLightDustCount;
		}
	}
}
