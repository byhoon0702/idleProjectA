using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UserInfo
{
	public class UserPromoteAbilityData : UserInfoLevelSaveBase
	{

		public Grade grade;


		public override int defaultLevel => 0;

		public override double TotalCombatPower()
		{
			double total = 0;

			total += (Int32)grade * (Int32)grade * 10000;

			return total;
		}
	}

	public class UserPromoteAbilityInfo
	{
		public const Int32 PROMOTE_ABILITY_PRESET_COUNT = 5;

		/// <summary>
		/// 현재 지휘관 등급
		/// </summary>
		public Grade userGrade => userData.proAbil.grade;

		/// <summary>
		/// 어빌리티 슬롯 개수
		/// </summary>
		public Int32 abilitySlot => GetPromoteAbilitySlotCount(proAbil.userGrade);

		/// <summary>
		/// 현재 진급 레벨 버프
		/// </summary>
		public List<UserAbility> currentPromote => GetPromoteValue(proAbil.userGrade);



		/// <summary>
		/// 진급 능력 슬롯 개수
		/// </summary>
		public Int32 GetPromoteAbilitySlotCount(Grade _grade)
		{
			Int32 slot = DataManager.it.Get<UserGradeDataSheet>().Get(_grade).promoteAbilitySlot;

			return slot;
		}


		/// <summary>
		/// 진급 레벨 버프
		/// </summary>
		public List<UserAbility> GetPromoteValue(Grade _grade)
		{
			var sheet = DataManager.it.Get<UserGradeDataSheet>();
			var promoteData = sheet.Get(_grade);

			List<UserAbility> result = new List<UserAbility>();

			result.Add(new UserAbility(UserAbilityType.AttackPower, promoteData.attackPowerRatio));
			result.Add(new UserAbility(UserAbilityType.Hp, promoteData.hpUpRatio));

			return result;
		}
	}
}
