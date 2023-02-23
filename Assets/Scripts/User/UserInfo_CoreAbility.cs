using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public class CoreAbilitySaveData
{
	public Int64 tid;
	public IdleNumber value;


	public CoreAbilitySaveData()
	{

	}

	public CoreAbilitySaveData(long _tid, IdleNumber _value)
	{
		tid = _tid;
		value = _value;
	}
}


public static partial class UserInfo
{
	[Serializable]
	public class CoreAbilitySave
	{
		public List<CoreAbilitySaveData> saveData = new List<CoreAbilitySaveData>();
		public bool[] lockState = new bool[CORE_ABILITY_COUNT];


		public double TotalCombatPower()
		{
			double total = 0;
			return total;
		}

		public bool IsLock(Grade _grade)
		{
			return lockState[(int)_grade];
		}

		public void SetLock(Grade _grade, bool _value)
		{
			lockState[(int)_grade] = _value;
		}

		public AbilityInfo GetGradeAbility(Grade _grade)
		{
			Int32 index = (Int32)_grade;

			// 슬롯 개수가 부족하면 새로생성
			if (saveData.Count <= index)
			{
				var gradeSheet = DataManager.Get<UserGradeDataSheet>();
				var gradeList = gradeSheet.GetGradeList();

				for (Int32 i = saveData.Count; i < gradeList.Count; i++)
				{
					saveData.Add(new CoreAbilitySaveData());
				}
			}


			// 데이터가 없는경우는 아직 능력치 선택이 되지 않은것임
			if (saveData[index] == null || saveData[index].tid == 0)
			{
				return null;
			}


			var sheet = DataManager.Get<AbilityInfoDataSheet>();
			var data = sheet.Get(saveData[index].tid);
			if (data == null)
			{
				VLog.LogError($"[Core Ability] Invalid Tid: {saveData[index].tid}");
				return null;
			}

			AbilityInfo ability = new AbilityInfo();

			ability.type = data.ability;
			ability.value = saveData[index].value;

			return ability;
		}

		public void SetGradeAbility(Grade _grade, AbilityInfo _userAbility)
		{
			Int32 index = (Int32)_grade;

			// 슬롯 개수가 부족하면 새로생성
			if (saveData.Count <= index)
			{
				var gradeSheet = DataManager.Get<UserGradeDataSheet>();
				var gradeList = gradeSheet.GetGradeList();

				for (Int32 i = saveData.Count; i < gradeList.Count; i++)
				{
					saveData.Add(new CoreAbilitySaveData());
				}
			}

			var sheet = DataManager.Get<AbilityInfoDataSheet>();
			var data = sheet.GetTid(_userAbility.type);
			saveData[index] = new CoreAbilitySaveData(data, _userAbility.value);
		}
	}

	public class CoreAbilityInfo
	{
		public CoreAbilityGenerator abilityGenerator = new CoreAbilityGenerator();

		/// <summary>
		/// 현재 지휘관 등급
		/// </summary>
		public Grade userGrade => userData.userGrade;

		/// <summary>
		/// 슬롯의 어빌리티 능력치 가져옴
		/// </summary>
		public AbilityInfo GetAbility(Grade _slotGrade)
		{
			return userData.coreAbil.GetGradeAbility(_slotGrade);
		}

		public bool GetAbilityLock(Grade _slotGrade)
		{
			return userData.coreAbil.IsLock(_slotGrade);
		}

		public void SetAbilityLock(Grade _slotGrade, bool _value)
		{
			userData.coreAbil.SetLock(_slotGrade, _value);
		}

		/// <summary>
		/// 슬롯의 어빌리티 능력을 랜덤하게 셋팅함
		/// </summary>
		/// <param name="_slotGrade"></param>
		public void SetRandomAbility(Grade _slotGrade)
		{
			AbilityInfo randomAbility = abilityGenerator.GenerateAbility();
			userData.coreAbil.SetGradeAbility(_slotGrade, randomAbility);
		}

		/// <summary>
		/// 수치에 맞는 grade
		/// </summary>
		public Grade GetAbilityGrade(Stats _ability, IdleNumber _value)
		{
			var gradeList = DataManager.Get<UserGradeDataSheet>().GetGradeList();
			foreach (var grade in gradeList)
			{
				abilityGenerator.GetAbilityValueRange(grade, _ability, out var min, out var max);

				if (min <= _value && _value < max)
				{
					return grade;
				}
			}

			return Grade.D;
		}

		/// <summary>
		/// 코어 능력 갱신 비용(coreabilitypoint)
		/// </summary>
		public Int64 ConsumeMedalCount(Int32 _lockCount)
		{
			return ConfigMeta.it.CORE_ABILITY_DEFAULT_CONSUME_COUNT * (1 + _lockCount);
		}
	}
}
