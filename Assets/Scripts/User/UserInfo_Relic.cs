using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static partial class UserInfo
{
	public static List<UserAbilityType> relicTypes => DataManager.it.Get<UserRelicDataSheet>().GetAbilityTypes();


	public class RelicSave : UserInfoLevelSaveBase
	{
		public override int defaultLevel => 0;

		public override double TotalCombatPower()
		{
			double total = 0;

			total += GetLevel(UserAbilityType.AttackPower) * 1000;
			total += GetLevel(UserAbilityType.Hp) * 1000;
			total += GetLevel(UserAbilityType.CriticalChance) * 1000;
			total += GetLevel(UserAbilityType.CriticalAttackPower) * 1000;
			total += GetLevel(UserAbilityType.MoveSpeed) * 1000;
			total += GetLevel(UserAbilityType.AttackSpeed) * 1000;
			total += GetLevel(UserAbilityType.SkillAttackPower) * 1000;
			total += GetLevel(UserAbilityType.BossAttackPower) * 1000;
			total += GetLevel(UserAbilityType.WarriorHp) * 1000;
			total += GetLevel(UserAbilityType.ArcherCriticalChance) * 1000;
			total += GetLevel(UserAbilityType.SpearManCriticalAttackPower) * 1000;
			total += GetLevel(UserAbilityType.WizardAttackPower) * 1000;

			return total;
		}
	}

	public class RelicInfo
	{
		/// <summary>
		/// 현재 유물 레벨
		/// </summary>
		public Int32 GetRelicLevel(UserAbilityType _abilityType)
		{
			return userData.relic.GetLevel(_abilityType);
		}

		/// <summary>
		/// 유물 최대레벨
		/// </summary>
		public Int32 GetRelicMaxLevel(UserAbilityType _abilityType)
		{
			var relicData = DataManager.it.Get<UserRelicDataSheet>().Get(_abilityType);
			if (relicData == null)
			{
				VLog.LogError($"어빌리티 정보가 없음. {_abilityType}");
				return 1;
			}

			return relicData.maxLevel;
		}
		
		/// <summary>
		/// 유물 값
		/// </summary>
		public float GetRelicValue(UserAbilityType _abilityType, Int32 _level)
		{
			var relicData = DataManager.it.Get<UserRelicDataSheet>().Get(_abilityType);
			if (relicData == null)
			{
				VLog.LogError($"어빌리티 정보가 없음. {_abilityType}");
				return 0;
			}

			float outTotal = _level * relicData.incValue;
			return outTotal;
		}

		/// <summary>
		/// 유물 값(현재 어빌리티)
		/// </summary>
		public float GetCurrentRelicValue(UserAbilityType _abilityType)
		{
			return GetRelicValue(_abilityType, GetRelicLevel(_abilityType));
		}

		/// <summary>
		/// 유물 레벨업!
		/// </summary>
		public void LevelUpRelic(UserAbilityType _abilityType)
		{
			Int32 maxLevel = GetRelicMaxLevel(_abilityType);
			Int32 currLevel = userData.relic.GetLevel(_abilityType);

			if (currLevel < maxLevel)
			{
				userData.relic.SetLevel(_abilityType, currLevel + 1);
			}

			CalculateTotalCombatPower();
		}

		/// <summary>
		/// 유물 레벨업 비용
		/// '_level'로 올라가기 위한 필요 비용
		/// </summary>
		public void GetRelicLevelupConsumeCount(UserAbilityType _abilityType, Int32 _level, out Int64 _consumeTid, out Int32 _consumeCount)
		{
			var consumeInfo = DataManager.it.Get<UserRelicDataSheet>().Get(_abilityType);

			if(consumeInfo != null)
			{
				_consumeTid = consumeInfo.consumeItem;
				_consumeCount = consumeInfo.consumePoint;
			}
			else
			{
				_consumeTid = 0;
				_consumeCount = 0;
			}
		}

		/// <summary>
		/// 현재 레벨의 유물 레벨업 비용
		/// </summary>
		public void GetCurrentRelicLevelupConsumeCount(UserAbilityType _abilityType, out Int64 _consumeTid, out Int32 _consumeCount)
		{
			GetRelicLevelupConsumeCount(_abilityType, GetRelicLevel(_abilityType), out _consumeTid, out _consumeCount);
		}

		/// <summary>
		/// 유물 레벨업 확률
		/// </summary>
		public float GetLevelupRatio(UserAbilityType _abilityType, Int32 _level)
		{
			var sheet = DataManager.it.Get<UserRelicDataSheet>().Get(_abilityType);

			return sheet.startUpgradeRatio - ((_level - 1) * sheet.decreaseSuccessRatio);
		}

		/// <summary>
		/// 유물 레벨업 확률(현재레벨)
		/// </summary>
		public float GetLevelupRatio(UserAbilityType _abilityType)
		{
			var sheet = DataManager.it.Get<UserRelicDataSheet>().Get(_abilityType);
			Int32 currLevel = GetRelicLevel(_abilityType);

			return sheet.startUpgradeRatio - ((currLevel - 1) * sheet.decreaseSuccessRatio);
		}
	}
}
