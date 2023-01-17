using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UserInfo
{
	public const Int32 PROPERTY_PRESET_COUNT = 5;
	public static List<UserAbilityType> propertyList => DataManager.it.Get<UserPropertyDataSheet>().GetAbilityTypes();

	public class UserPropertyData : UserInfoLevelSaveBase
	{
		public override int defaultLevel => 1;


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

			return total;
		}
	}


	public class UserPropertyInfo
	{
		/// <summary>
		/// 특성 포인트 총합
		/// </summary>
		public Int32 totalPoint => UserDataCalculator.GetPropertyPoint(userLv);
		/// <summary>
		/// 사용할 수 있는(남아있는) 포인트 
		/// </summary>
		public Int32 remainPoint => GetRemainPoint(selectedPreset);
		/// <summary>
		/// 선택된 프리셋
		/// </summary>
		public Int32 selectedPreset
		{
			get
			{
				return userData.selectedPropIndex;
			}
			set
			{
				userData.selectedPropIndex = Mathf.Clamp(value, 0, PROPERTY_PRESET_COUNT);
			}
		}



		/// <summary>
		/// 현재 특성 레벨
		/// </summary>
		public Int32 GetCurrentPropertyLevel(UserAbilityType _abilityType)
		{
			return GetPropertyLevel(_abilityType, selectedPreset);
		}

		/// <summary>
		/// 특성 레벨
		/// </summary>
		public Int32 GetPropertyLevel(UserAbilityType _abilityType, Int32 _presetIndex)
		{
			return userData.props[_presetIndex].GetLevel(_abilityType);
		}

		/// <summary>
		/// 특성 최대레벨
		/// </summary>
		public Int32 GetPropertyMaxLevel(UserAbilityType _abilityType)
		{
			var abilityData = DataManager.it.Get<UserPropertyDataSheet>().Get(_abilityType);
			if (abilityData == null)
			{
				VLog.LogError($"특성 정보가 없음. {_abilityType}");
				return 1;
			}

			return abilityData.maxLevel;
		}

		/// <summary>
		/// 특성 값
		/// </summary>
		public float GetPropertyValue(UserAbilityType _abilityType, Int32 _level)
		{
			var propertyData = DataManager.it.Get<UserPropertyDataSheet>().Get(_abilityType);
			if (propertyData == null)
			{
				VLog.LogError($"특성 정보가 없음. {_abilityType}");
				return 0;
			}

			float outTotal = _level * propertyData.incValue;
			return outTotal;
		}

		/// <summary>
		/// 특성 값(현재 어빌리티)
		/// </summary>
		public float GetCurrentPropertyValue(UserAbilityType _abilityType)
		{
			return GetPropertyValue(_abilityType, GetCurrentPropertyLevel(_abilityType));
		}

		/// <summary>
		/// 특성 레벨업!
		/// </summary>
		public void LevelUpProperty(UserAbilityType _abilityType)
		{
			Int32 maxLevel = GetPropertyMaxLevel(_abilityType);
			Int32 currLevel = userData.currProp.GetLevel(_abilityType);

			if (currLevel < maxLevel)
			{
				userData.currProp.SetLevel(_abilityType, currLevel + 1);
			}

			CalculateTotalCombatPower();
		}

		/// <summary>
		/// 특성 레벨업 비용
		/// '_level'로 올라가기 위한 필요 비용
		/// </summary>
		public Int32 GetPropertyLevelupConsumeCount(UserAbilityType _abilityType)
		{
			var consumeInfo = DataManager.it.Get<UserPropertyDataSheet>().Get(_abilityType);

			return consumeInfo.consumePoint;
		}

		/// <summary>
		/// 특성레벨업 가능한 포인트 계산
		/// </summary>
		public Int32 GetRemainPoint(Int32 _presetIndex)
		{
			Int32 total = 0;
			var sheet = DataManager.it.Get<UserPropertyDataSheet>();

			foreach (var data in userData.props[_presetIndex].saveData)
			{
				total += data.value * sheet.Get(data.tid).consumePoint;
			}

			return totalPoint - total;
		}

		/// <summary>
		/// 특성정보 초기화
		/// </summary>
		public void ResetProperty(Int32 _presetIndex)
		{
			userData.props[_presetIndex] = new UserPropertyData();
		}
	}
}
