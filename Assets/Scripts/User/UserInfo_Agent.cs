using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UserInfo
{
	public class UserAgentData : UserInfoLevelSaveBase
	{
		public override int defaultLevel => 1;


		public override double TotalCombatPower()
		{
			double total = GetLevel(UserAbilityType.Agent) * 10000;

			return total;
		}
	}

	public class UserAgentInfo
	{
		/// <summary>
		/// 현재 배치가능한 병사 수
		/// </summary>
		public Int32 currUnitCount => GetUnitCount(GetAgentLevel());




		/// <summary>
		/// 현재 보급소 레벨
		/// </summary>
		public Int32 GetAgentLevel()
		{
			return userData.agent.GetLevel(UserAbilityType.Agent);
		}

		/// <summary>
		/// 보급소 최대레벨
		/// </summary>
		public Int32 GetAgentMaxLevel()
		{
			var sheet = DataManager.it.Get<UserRelicDataSheet>();
			Int32 maxLevel = userData.agent.defaultLevel;

			foreach(var data in sheet.infos)
			{
				maxLevel = Mathf.Max(data.maxLevel, maxLevel);
			}

			return maxLevel;
		}

		/// <summary>
		/// 보급소 버프 값(계산식 반영)
		/// </summary>
		public List<UserAbility> GetAgentValue(Int32 _level)
		{
			var sheet = DataManager.it.Get<UserAgentDataSheet>();
			if (sheet.GetByLevel(_level) == null) // 현재 레벨 데이터가 있는지만 판단
			{
				VLog.LogError($"보급소 관련 정보가 없음. lv: {_level}");
				return new List<UserAbility>();
			}

			double goldTotalRatio = 0;
			double expTotalRatio = 0;
			double itemTotalRatio = 0;

			foreach(var data in sheet.infos)
			{
				if(data.level <= _level)
				{
					goldTotalRatio += data.goldUpValue;
					expTotalRatio += data.expUpValue;
					itemTotalRatio += data.itemUpValue;
				}
			}


			List<UserAbility> result = new List<UserAbility>();

			result.Add(new UserAbility(UserAbilityType.GoldUp, goldTotalRatio));
			result.Add(new UserAbility(UserAbilityType.ExpUp, expTotalRatio));
			result.Add(new UserAbility(UserAbilityType.ItemUp, itemTotalRatio));

			return result;
		}   

		/// <summary>
		/// 보급소 현재 버프 값
		/// </summary>
		public List<UserAbility> GetCurrentAgentValue()
		{
			return GetAgentValue(GetAgentLevel());
		}

		/// <summary>
		/// 보급소 레벨업!
		/// </summary>
		public void LevelUpAgent()
		{
			Int32 currLevel = userData.agent.GetLevel(UserAbilityType.Agent);
			if(GetAgentMaxLevel() == currLevel)
			{
				VLog.LogError($"레벨업 불가능(만렙), {currLevel}");
				return;
			}


			userData.agent.SetLevel(UserAbilityType.Agent, currLevel + 1);

			CalculateTotalCombatPower();
		}

		/// <summary>
		/// 레벨을 올리기위한 조건(_level의 달성조건)
		/// </summary>
		public void GetLevelupCondition(Int32 _level, out Int32 _needLevel, out IdleNumber _consumeGold)
		{
			var sheet = DataManager.it.Get<UserAgentDataSheet>();

			var agentData = sheet.GetByLevel(_level);
			if(agentData == null)
			{
				_needLevel = -1;
				_consumeGold = new IdleNumber();

				VLog.LogError($"보급소 레벨정보 찾지 못함. {_level}");
				return;
			}

			_needLevel = agentData.needUserLevel;
			_consumeGold = agentData.consumeGoldCount;
		}

		/// <summary>
		/// 배치가능한 병사 수
		/// </summary>
		public Int32 GetUnitCount(Int32 _level)
		{
			var sheet = DataManager.it.Get<UserAgentDataSheet>();

			var agentData = sheet.GetByLevel(_level);
			if (agentData == null)
			{
				VLog.LogError($"보급소 레벨정보 찾지 못함. {_level}");
				return 1;
			}

			return agentData.unitCount;
		}
	}
}
