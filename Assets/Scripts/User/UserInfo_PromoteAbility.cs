﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public static partial class UserInfo
{
	public class PromoteAbilitySave : UserInfoValueSaveBase
	{
		public Grade grade;

		public override double TotalCombatPower()
		{
			double total = 0;

			total += (Int32)grade * (Int32)grade * 10000;

			return total;
		}

		public override double GetValue(UserAbilityType _ability)
		{
			return 1;
		}

		public override void SetValue(UserAbilityType _ability, double _value)
		{
		}

		public UserAbility GetGradeAbility(Grade _grade)
		{
			Int32 index = (Int32)_grade;

			// 슬롯 개수가 부족하면 새로생성
			if(saveData.Count <= index)
			{
				var gradeSheet = DataManager.it.Get<UserGradeDataSheet>();
				var gradeList = gradeSheet.GetGradeList();

				for (Int32 i= saveData.Count ; i<gradeList.Count ; i++)
				{
					saveData.Add(new UserInfoValueSaveData());
				}
			}


			// 데이터가 없는경우는 아직 능력치 선택이 되지 않은것임
			if(saveData[index] == null || saveData[index].tid == 0)
			{
				return null;
			}


			var sheet = DataManager.it.Get<UserAbilityInfoDataSheet>();
			var data = sheet.Get(saveData[index].tid);
			if(data == null)
			{
				VLog.LogError($"[PromoteAbility] Invalid Tid: {saveData[index].tid}");
				return null;
			}

			UserAbility ability = new UserAbility();

			ability.type = data.ability;
			ability.value = saveData[index].value;

			return ability;
		}

		public void SetGradeAbility(Grade _grade, UserAbility _userAbility)
		{
			Int32 index = (Int32)_grade;

			// 슬롯 개수가 부족하면 새로생성
			if (saveData.Count <= index)
			{
				var gradeSheet = DataManager.it.Get<UserGradeDataSheet>();
				var gradeList = gradeSheet.GetGradeList();

				for (Int32 i = saveData.Count ; i < gradeList.Count ; i++)
				{
					saveData.Add(new UserInfoValueSaveData());
				}
			}

			var sheet = DataManager.it.Get<UserAbilityInfoDataSheet>();
			var data = sheet.GetTid(_userAbility.type);
			saveData[index] = new UserInfoValueSaveData(data, _userAbility.value);
		}
	}

	public class PromoteAbilityInfo
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

		/// <summary>
		/// grade로 가기 위한 필요 지휘관 레벨
		/// </summary>
		/// <returns></returns>
		public Int32 GetGradeUpNeedLevel(Grade _grade)
		{
			var sheet = DataManager.it.Get<UserGradeDataSheet>().Get(_grade);

			return sheet.needLevel;
		}

		/// <summary>
		/// 진급레벨 업
		/// </summary>
		public void LevelupGrade()
		{
			switch (userGrade)
			{
				case Grade.D:
					userData.proAbil.grade = Grade.C;
					break;
				case Grade.C:
					userData.proAbil.grade = Grade.B;
					break;
				case Grade.B:
					userData.proAbil.grade = Grade.A;
					break;
				case Grade.A:
					userData.proAbil.grade = Grade.S;
					break;
				case Grade.S:
					userData.proAbil.grade = Grade.SS;
					break;
				case Grade.SS:
					userData.proAbil.grade = Grade.SSS;
					break;
			}
		}












		/* ///////////////////////////////////////////////////////////
		 * 여기 밑으로는 진급 능력 슬롯
		 *////////////////////////////////////////////////////////////

		/// <summary>
		/// 슬롯의 어빌리티 능력치 가져옴
		/// </summary>
		public UserAbility GetAbility(Grade _slotGrade)
		{
			return userData.proAbil.GetGradeAbility(_slotGrade);
		}

		/// <summary>
		/// 슬롯의 어빌리티 능력을 랜덤하게 셋팅함
		/// </summary>
		/// <param name="_slotGrade"></param>
		public void SetRandomAbility(Grade _slotGrade)
		{
			UserAbility randomAbility = GenerateAbility();
			userData.proAbil.SetGradeAbility(_slotGrade, randomAbility);
		}

		/// <summary>
		/// 진급 능력 데이터 랜덤 생성
		/// </summary>
		public UserAbility GenerateAbility()
		{
			Grade grade = RandomGrade();
			UserAbilityType resultAbility = RandomAbility();

			GetAbilityValueRange(grade, resultAbility, out var minValue, out var maxValue);
			float resultValue = RandomAbilityValue(minValue, maxValue);

			return new UserAbility(resultAbility, resultValue);
		}

		/// <summary>
		/// 랜덤 등급
		/// </summary>
		/// <returns></returns>
		public Grade RandomGrade()
		{
			var probabilitySheet = DataManager.it.Get<UserPromoteAbilityProbabilityDataSheet>();

			float value = UnityEngine.Random.Range(0, 1.0f);
			float accum = 0;
			for (Int32 i = probabilitySheet.infos.Count - 1 ; i >= 0 ; i--)
			{
				accum += probabilitySheet.infos[i].probability;
				if (accum >= value)
				{
					return probabilitySheet.infos[i].grade;
				}
			}

			return Grade.D;
		}

		/// <summary>
		/// 랜덤 어빌리티
		/// </summary>
		public UserAbilityType RandomAbility()
		{
			var abilitySheet = DataManager.it.Get<UserPromoteAbilityDataSheet>();
			var abilityTypes = abilitySheet.GetAbilityTypes();

			UserAbilityType result = abilityTypes[Random.Range(0, abilityTypes.Count)];

			return result;
		}

		/// <summary>
		/// 어빌리티값 범위
		/// </summary>
		public void GetAbilityValueRange(Grade _grade, UserAbilityType _ability, out float _minValue, out float _maxValue)
		{
			var abilityInfo = DataManager.it.Get<UserPromoteAbilityDataSheet>().GetByAbilityType(_ability);
			var probabilitySheet = DataManager.it.Get<UserPromoteAbilityProbabilityDataSheet>();

			float rangeMin;
			float rangeMax = probabilitySheet.Get(_grade).rangeMax;

			if(_grade == Grade.D)
			{
				rangeMin = 0;
			}
			else
			{
				rangeMin = probabilitySheet.Get((Grade)((Int32)_grade - 1)).rangeMax;
			}

			_minValue = abilityInfo.min + (abilityInfo.max - abilityInfo.min) * rangeMin;
			_maxValue = abilityInfo.min + (abilityInfo.max - abilityInfo.min) * rangeMax;
		}

		/// <summary>
		/// 범위 내의 랜덤한 어빌리티 값
		/// </summary>
		public float RandomAbilityValue(float _minValue, float _maxValue)
		{
			float resultValue = Random.Range((Int32)(_minValue * 100), (Int32)(_maxValue * 100)) * 0.01f;

			return resultValue;
		}

		/// <summary>
		/// 수치에 맞는 grade
		/// </summary>
		public Grade GetAbilityGrade(UserAbilityType _ability, float _value)
		{
			var gradeList = DataManager.it.Get<UserGradeDataSheet>().GetGradeList();
			foreach (var grade in gradeList)
			{
				GetAbilityValueRange(grade, _ability, out var min, out var max);

				if(min <= _value && _value < max)
				{
					return grade;
				}
			}

			return Grade.D;
		}

		/// <summary>
		/// 진급 능력 갱신 비용(메달)
		/// </summary>
		public Int64 ConsumeMedalCount(Int32 _lockCount)
		{
			return ConfigMeta.it.PROMOTE_ABILITY_DEFAULT_CONSUME_COUNT * (1 + _lockCount);
		}
	}
}
