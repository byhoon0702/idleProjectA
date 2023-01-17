using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UserInfo
{
	public static List<UserAbilityType> trainingTypes => DataManager.it.Get<UserTrainingDataSheet>().GetAbilityTypes();


	public class UserTrainingData : UserInfoLevelSaveBase
	{
		public override int defaultLevel => 1;


		public override double TotalCombatPower()
		{
			double total = 0;

			total += GetLevel(UserAbilityType.AttackPower) * 1000;
			total += GetLevel(UserAbilityType.Hp) * 1000;
			total += GetLevel(UserAbilityType.CriticalChance) * 1000;
			total += GetLevel(UserAbilityType.CriticalAttackPower) * 1000;

			return total;
		}
	}

	public class UserTrainingInfo
	{
		/// <summary>
		/// 현재 훈련 레벨
		/// </summary>
		public Int32 GetTrainingLevel(UserAbilityType _abilityType)
		{
			return userData.training.GetLevel(_abilityType);
		}

		/// <summary>
		/// 훈련 최대레벨
		/// </summary>
		public Int32 GetTrainingMaxLevel(UserAbilityType _abilityType)
		{
			var trainingData = DataManager.it.Get<UserTrainingDataSheet>().Get(_abilityType);
			if (trainingData == null)
			{
				VLog.LogError($"어빌리티 정보가 없음. {_abilityType}");
				return 1;
			}

			return trainingData.maxLevel;
		}

		/// <summary>
		/// 훈련 값
		/// </summary>
		public float GetTrainingValue(UserAbilityType _abilityType, Int32 _level)
		{
			var trainingData = DataManager.it.Get<UserTrainingDataSheet>().Get(_abilityType);
			if (trainingData == null)
			{
				VLog.LogError($"어빌리티 정보가 없음. {_abilityType}");
				return 0;
			}

			float outTotal = trainingData.defaultValue + (_level - 1) * trainingData.incValue;
			return outTotal;
		}

		/// <summary>
		/// 훈련 값(현재 어빌리티)
		/// </summary>
		public float GetCurrentTrainingValue(UserAbilityType _abilityType)
		{
			return GetTrainingValue(_abilityType, GetTrainingLevel(_abilityType));
		}

		/// <summary>
		/// 훈련 레벨업!
		/// </summary>
		public void LevelUpTraining(UserAbilityType _abilityType)
		{
			Int32 maxLevel = GetTrainingMaxLevel(_abilityType);
			Int32 currLevel = userData.training.GetLevel(_abilityType);

			if(currLevel < maxLevel)
			{
				userData.training.SetLevel(_abilityType, currLevel + 1);
			}

			CalculateTotalCombatPower();
		}

		/// <summary>
		/// 훈련 레벨업 비용
		/// '_level'로 올라가기 위한 필요 비용
		/// </summary>
		public IdleNumber GetTrainingLevelupConsumeCount(UserAbilityType _abilityType, Int32 _level)
		{
			var consumeInfos = DataManager.it.Get<UserTrainingConsumeDataSheet>().Get(_abilityType);

			for (Int32 i = 0 ; i < consumeInfos.Count ; i++)
			{
				if (consumeInfos[i].startLevel <= _level && _level <= consumeInfos[i].endLevel)
				{
					return consumeInfos[i].consume;
				}
			}

			// 여기로 들어오지 않게 데이터 검증을 미리 해야함
			VLog.LogError($"소비정보 찾지못함. training: {_abilityType}, lv: {_level}");
			return consumeInfos[consumeInfos.Count - 1].consume;
		}

		/// <summary>
		/// 현재 레벨의 훈련 레벨업 비용
		/// </summary>
		public IdleNumber GetCurrentTrainingLevelupConsumeCount(UserAbilityType _abilityType)
		{
			return GetTrainingLevelupConsumeCount(_abilityType, GetTrainingLevel(_abilityType));
		}
	}
}
