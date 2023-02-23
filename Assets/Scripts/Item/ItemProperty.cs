using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProperty : ItemBase
{
	public const int DEFAULT_LEVEL = 0;
	public UserPropertyData propertyData;

	public override int MaxLevel => propertyData.maxLevel;




	public override VResult Setup(InstantItem _instantItem)
	{
		VResult vResult = base.Setup(_instantItem);

		if (vResult.Fail())
		{
			return vResult;
		}

		vResult = SetupMetaData();
		if (vResult.Fail())
		{
			return vResult;
		}

		return vResult.SetOk();
	}

	private VResult SetupMetaData()
	{
		VResult vResult = new VResult();

		propertyData = DataManager.Get<UserPropertyDataSheet>().Get(data.userPropertyTid);

		if (propertyData == null)
		{
			return vResult.SetFail(VResultCode.NO_META_DATA, $"UserPropertyDataSheet. tid: {data.tid}, userPropertyTid: {data.userTrainingTid}");
		}

		return vResult.SetOk();
	}

	/// <summary>
	/// 사용중인 프로퍼티 포인트
	/// </summary>
	/// <returns></returns>
	public int GetUsingPropertyPoint()
	{
		int total = propertyData.consumePoint * Level;

		return total;
	}

	public bool Levelupable()
	{
		return IsMaxLv == false;
	}

	public void ResetLevel()
	{
		instantItem.level = DEFAULT_LEVEL;
	}

	public override string ToString()
	{
		return $"[{ItemName}({Tid})], lv: {Level})";
	}
}

//public static partial class UserInfo
//{
//	public const Int32 PROPERTY_PRESET_COUNT = 5;
//	public static List<AbilityType> propertyList => DataManager.Get<UserPropertyDataSheet>().GetAbilityTypes();
//
//	public class PropertySave : UserInfoLevelSaveBase
//	{
//		public override int defaultLevel => 1;
//
//
//		public override double TotalCombatPower()
//		{
//			double total = 0;
//
//			total += GetLevel(AbilityType.AttackPower) * 1000;
//			total += GetLevel(AbilityType.Hp) * 1000;
//			total += GetLevel(AbilityType.CriticalChance) * 1000;
//			total += GetLevel(AbilityType.CriticalAttackPower) * 1000;
//			total += GetLevel(AbilityType.MoveSpeed) * 1000;
//			total += GetLevel(AbilityType.AttackSpeed) * 1000;
//			total += GetLevel(AbilityType.SkillAttackPower) * 1000;
//			total += GetLevel(AbilityType.BossAttackPower) * 1000;
//
//			return total;
//		}
//	}
//
//
//	public class PropertyInfo
//	{
//		/// <summary>
//		/// 특성 포인트 총합
//		/// </summary>
//		public Int32 totalPoint => UserDataCalculator.GetPropertyPoint(UserLv);
//		/// <summary>
//		/// 사용할 수 있는(남아있는) 포인트 
//		/// </summary>
//		public Int32 remainPoint => GetRemainPoint(selectedPreset);
//		/// <summary>
//		/// 선택된 프리셋
//		/// </summary>
//		public Int32 selectedPreset
//		{
//			get
//			{
//				return userData.selectedPropIndex;
//			}
//			set
//			{
//				userData.selectedPropIndex = Mathf.Clamp(value, 0, PROPERTY_PRESET_COUNT);
//			}
//		}
//
//
//
//		/// <summary>
//		/// 현재 특성 레벨
//		/// </summary>
//		public Int32 GetCurrentPropertyLevel(AbilityType _abilityType)
//		{
//			return GetPropertyLevel(_abilityType, selectedPreset);
//		}
//
//		/// <summary>
//		/// 특성 레벨
//		/// </summary>
//		public Int32 GetPropertyLevel(AbilityType _abilityType, Int32 _presetIndex)
//		{
//			return userData.props[_presetIndex].GetLevel(_abilityType);
//		}
//
//		/// <summary>
//		/// 특성 최대레벨
//		/// </summary>
//		public Int32 GetPropertyMaxLevel(AbilityType _abilityType)
//		{
//			var abilityData = DataManager.Get<UserPropertyDataSheet>().Get(_abilityType);
//			if (abilityData == null)
//			{
//				VLog.LogError($"특성 정보가 없음. {_abilityType}");
//				return 1;
//			}
//
//			return abilityData.maxLevel;
//		}
//
//		/// <summary>
//		/// 특성 값
//		/// </summary>
//		public float GetPropertyValue(AbilityType _abilityType, Int32 _level)
//		{
//			var propertyData = DataManager.Get<UserPropertyDataSheet>().Get(_abilityType);
//			if (propertyData == null)
//			{
//				VLog.LogError($"특성 정보가 없음. {_abilityType}");
//				return 0;
//			}
//
//			float outTotal = _level * propertyData.incValue;
//			return outTotal;
//		}
//
//		/// <summary>
//		/// 특성 값(현재 어빌리티)
//		/// </summary>
//		public float GetCurrentPropertyValue(AbilityType _abilityType)
//		{
//			return GetPropertyValue(_abilityType, GetCurrentPropertyLevel(_abilityType));
//		}
//
//		/// <summary>
//		/// 특성 레벨업!
//		/// </summary>
//		public void LevelUpProperty(AbilityType _abilityType)
//		{
//			Int32 maxLevel = GetPropertyMaxLevel(_abilityType);
//			Int32 currLevel = userData.currProp.GetLevel(_abilityType);
//
//			if (currLevel < maxLevel)
//			{
//				userData.currProp.SetLevel(_abilityType, currLevel + 1);
//			}
//
//			CalculateTotalCombatPower();
//		}
//
//		/// <summary>
//		/// 특성 레벨업 비용
//		/// '_level'로 올라가기 위한 필요 비용
//		/// </summary>
//		public Int32 GetPropertyLevelupConsumeCount(AbilityType _abilityType)
//		{
//			var consumeInfo = DataManager.Get<UserPropertyDataSheet>().Get(_abilityType);
//
//			return consumeInfo.consumePoint;
//		}
//
//		/// <summary>
//		/// 특성레벨업 가능한 포인트 계산
//		/// </summary>
//		public Int32 GetRemainPoint(Int32 _presetIndex)
//		{
//			Int32 total = 0;
//			var sheet = DataManager.Get<UserPropertyDataSheet>();
//
//			foreach (var data in userData.props[_presetIndex].saveData)
//			{
//				total += data.value.GetValueToInt() * sheet.Get(data.tid).consumePoint;
//			}
//
//			return totalPoint - total;
//		}
//
//		/// <summary>
//		/// 특성정보 초기화
//		/// </summary>
//		public void ResetProperty(Int32 _presetIndex)
//		{
//			userData.props[_presetIndex] = new PropertySave();
//		}
//	}
//}
//
