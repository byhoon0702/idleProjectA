using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class RewardUtil
{
	public static List<RuntimeData.RewardInfo> ReArrangReward(List<RuntimeData.RewardInfo> list)
	{
		List<RuntimeData.RewardInfo> _rewardList = new List<RuntimeData.RewardInfo>();
		Dictionary<long, RuntimeData.RewardInfo> dict = new Dictionary<long, RuntimeData.RewardInfo>();
		for (int i = 0; i < list.Count; i++)
		{
			var reward = list[i];
			if (reward == null)
			{
				continue;
			}
			if (list[i].Category == RewardCategory.RewardBox)
			{
				var _list = OpenRewardBox(reward);
				for (int ii = 0; ii < _list.Count; ii++)
				{
					var rr = _list[ii];
					if (dict.ContainsKey(rr.Tid))
					{
						dict[rr.Tid].AddCount(rr.fixedCount);
					}
					else
					{
						dict.Add(rr.Tid, rr);
					}
				}
			}
			else
			{
				if (dict.ContainsKey(reward.Tid))
				{
					dict[reward.Tid].AddCount(reward.fixedCount);
				}
				else
				{
					dict.Add(reward.Tid, reward);
				}

			}
		}
		foreach (var box_result in dict.Values)
		{
			_rewardList.Add(box_result);
		}

		return _rewardList;
	}

	public static List<RuntimeData.RewardInfo> OpenRewardBox(RuntimeData.RewardInfo info)
	{
		if (info == null)
		{
			return null;
		}

		if (info.Category != RewardCategory.RewardBox)
		{
			return null;
		}

		var rewardBoxdata = DataManager.Get<RewardBoxDataSheet>().Get(info.Tid);
		List<RuntimeData.RewardInfo> rewardInfos = new List<RuntimeData.RewardInfo>();

		for (int i = 0; i < rewardBoxdata.rewards.Count; i++)
		{
			RuntimeData.RewardInfo reward = new RuntimeData.RewardInfo(rewardBoxdata.rewards[i]);
			reward.UpdateCount();
			rewardInfos.Add(reward);
		}

		Dictionary<long, RuntimeData.RewardInfo> rewardDict = new Dictionary<long, RuntimeData.RewardInfo>();

		List<RuntimeData.RewardInfo> results = new List<RuntimeData.RewardInfo>();

		for (int i = 0; i < info.fixedCount.GetValueToInt(); i++)
		{
			var rewardList = RandomReward(rewardInfos, RandomLogic.RewardBox);
			for (int ii = 0; ii < rewardList.Count; ii++)
			{
				var reward = rewardList[ii].Clone();
				if (rewardDict.ContainsKey(reward.Tid))
				{
					rewardDict[reward.Tid].AddCount((IdleNumber)1);
				}
				else
				{
					rewardDict.Add(reward.Tid, reward);
				}

			}

		}

		foreach (var reward in rewardDict.Values)
		{
			results.Add(reward);
		}

		return results;
	}

	public static List<RuntimeData.RewardInfo> RandomReward(List<RuntimeData.RewardInfo> rewardList, System.Random r)
	{
		List<RuntimeData.RewardInfo> getReward = new List<RuntimeData.RewardInfo>();
		var chance = r.Next(0, RandomLogic.maxChance);

		int minChance = 0;
		for (int i = 0; i < rewardList.Count; i++)
		{
			var reward = rewardList[i];
			int maxChance = (int)(reward.Chance * 100);

			if ((reward.Chance == 100) || chance >= minChance && chance < minChance + maxChance)
			{
				getReward.Add(reward);
				break;
			}
			minChance += maxChance;

		}
		return getReward;

	}

}


public static class MathExtension
{
	public static Vector3 GetAngledVector3(this Vector3 vector, float angle, bool inverse = false)
	{
		float radian = angle * Mathf.Deg2Rad;
		Vector3 pos = Vector3.zero;

		if (inverse)
		{
			pos.x = (vector.x * Mathf.Cos(radian)) + (vector.y * Mathf.Sin(radian));
			pos.y = (vector.x * -Mathf.Sin(radian)) + (vector.y * Mathf.Cos(radian));
		}
		else
		{
			pos.x = (vector.x * Mathf.Cos(radian)) + (vector.y * -Mathf.Sin(radian));
			pos.y = (vector.x * Mathf.Sin(radian)) + (vector.y * Mathf.Cos(radian));
		}


		pos.z = 0;

		return pos;
	}

}
public static class RenderExtension
{

	private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
	{
		//RectTransformUtility.RectangleContainsScreenPoint()
		Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
		Vector3[] objectCorners = new Vector3[4];
		rectTransform.GetWorldCorners(objectCorners);

		int visibleCorners = 0;
		Vector3 tempScreenSpaceCorner; // Cached
		for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
		{
			tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); // Transform world space position of corner to screen space
			if (screenBounds.Contains(tempScreenSpaceCorner)) // If the corner is inside the screen
			{
				visibleCorners++;
			}
		}

		return visibleCorners;
	}
	public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
	{
		return CountCornersVisibleFrom(rectTransform, camera) == 4; // True if all 4 corners are visible
	}
	public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
	{
		return CountCornersVisibleFrom(rectTransform, camera) > 0; // True if any corners are visible
	}
}

public static class ExtensionMethod
{
	public static bool NickNameValidate(this string nickName, out string message)
	{
		message = "";
		if (nickName.IsNullOrEmpty())
		{
			message = PlatformManager.Language["str_ui_input_nickname"];
			return false;
		}
		string checker = Regex.Replace(nickName, @"[^a-zA-Z0-9가-힣]", "", RegexOptions.Singleline);

		if (checker.Equals(nickName) == false)
		{
			message = PlatformManager.Language["str_ui_contain_special_character"];
			return false;
		}
		if (nickName.Length > 12)
		{
			message = PlatformManager.Language["str_ui_nickname_length_over"];
			return false;
		}
		return true;
	}
	public static void ChangeLayer(this GameObject go, int layer)
	{
		go.layer = layer;
		for (int i = 0; i < go.transform.childCount; i++)
		{
			Transform child = go.transform.GetChild(i);
			ChangeLayer(child.gameObject, layer);
		}
	}



	public static void SetButtonEvent(this Button button, UnityAction buttonEvent)
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(buttonEvent);
	}
	public static void SetButtonEvent(this UIEconomyButton button, ButtonRepeatEvent buttonEvent, UnityAction onClick = null)
	{
		button.SetEvent(buttonEvent, onClick);
	}

	public static bool HasParameter(this Animator animator, string name)
	{
		for (int i = 0; i < animator.parameterCount; i++)
		{
			if (animator.GetParameter(i).name == name)
			{
				return true;
			}
		}
		return false;

	}
	public static Transform FindDeepChild(this Transform aParent, string aName)
	{
		Queue<Transform> queue = new Queue<Transform>();
		queue.Enqueue(aParent);
		while (queue.Count > 0)
		{
			var c = queue.Dequeue();
			if (c.name == aName)
				return c;
			foreach (Transform t in c)
				queue.Enqueue(t);
		}
		return null;
	}

	public static bool IsRegistered(this System.Delegate dele, System.Delegate added)
	{
		if (dele == null)
		{
			return false;
		}

		var list = dele.GetInvocationList();
		for (int i = 0; i < list.Length; i++)
		{
			if (list[i].Equals(added))
			{
				return true;
			}
		}

		return false;
	}

	public static bool IsNull(this object in_obj)
	{
		return in_obj == null;
	}

	public static bool NotNull(this object in_obj)
	{
		return in_obj != null;
	}

	public static bool HasStringValue(this string in_str)
	{
		return !string.IsNullOrEmpty(in_str);
	}

	public static bool EmptyStringValue(this string in_str)
	{
		return string.IsNullOrEmpty(in_str);
	}

	public static bool IsNullOrWhiteSpace(this string in_str)
	{
		if (in_str == null)
		{
			return true;
		}

		return string.IsNullOrEmpty(in_str.Trim());
	}

	public static int IntCompare(this string in_str, long in_comp_value)
	{
		long result = 0L;
		long.TryParse(in_str, out result);
		return result.CompareTo(in_comp_value);
	}

	public static List<int> IndexOfAll(this string in_str, char in_match_char)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < in_str.Length; i++)
		{
			if (in_str[i] == in_match_char)
			{
				list.Add(i);
			}
		}

		return list;
	}
	// Start is called before the first frame update
	public static bool HaveParam(this string in_str)
	{
		return (false == in_str.IsNullOrEmpty() && false == in_str.Equals("0"));
	}
	public static bool IsNullOrEmpty(this string s)
	{
		return string.IsNullOrEmpty(s);
	}

	public static void Restart(this ParticleSystem ps)
	{
		ps.Clear(true);
		//ps.Simulate(0, true, true);
		ps.Play(true);
	}

	public static string ToUIString(this StatusEffect type)
	{
		switch (type)
		{
			case StatusEffect.STUN:
				return "str_skill_stun";
		}

		return "";
	}
	public static Color GradeColor(this Grade grade)
	{
		switch (grade)
		{
			case Grade.D:
				return new Color32(255, 255, 255, 255);
			case Grade.C:
				return new Color32(52, 149, 26, 255);
			case Grade.B:
				return new Color32(28, 176, 255, 255);
			case Grade.A:
				return new Color32(255, 12, 255, 255);
			case Grade.S:
				return new Color32(255, 255, 51, 255);
			case Grade.SS:
				return new Color32(245, 180, 3, 255);
			case Grade.SSS:
				return new Color32(255, 18, 36, 255);
		}
		return Color.white;
	}

	public static string GradeString(this Grade grade)
	{
		switch (grade)
		{
			case Grade.D:
				return "D 등급";
			case Grade.C:
				return "C 등급";
			case Grade.B:
				return "B 등급";
			case Grade.A:
				return "A 등급";
			case Grade.S:
				return "S 등급";
			case Grade.SS:
				return "SS 등급";
			case Grade.SSS:
				return "SSS 등급";
		}

		return "등급 없음";
	}

	public static string ToUIString(this ContentType type)
	{
		return PlatformManager.Language[$"str_ui_contents_{type.ToString().ToLower()}"];

	}
	public static string ToUIString(this StatsType _state)
	{
		switch (_state)
		{
			case StatsType.Atk:
				return PlatformManager.Language["str_ui_status_info_atk"];
			case StatsType.Hp:
				return PlatformManager.Language["str_ui_status_info_hp"];
			case StatsType.Atk_Speed:
				return PlatformManager.Language["str_ui_status_info_atk_speed"];
			case StatsType.Crits_Chance:
				return PlatformManager.Language["str_ui_status_info_crits_chance"];
			case StatsType.Crits_Damage:
				return PlatformManager.Language["str_ui_status_info_crits_damage"];
			case StatsType.Super_Crits_Chance:
				return PlatformManager.Language["str_ui_status_info_super_crits_chance"];
			case StatsType.Super_Crits_Damage:
				return PlatformManager.Language["str_ui_status_info_super_crits_damage"];
			case StatsType.Move_Speed:
				return PlatformManager.Language["str_ui_status_info_skill_move_speed"];
			case StatsType.Hp_Recovery:
				return PlatformManager.Language["str_ui_status_info_hp_recovery"];
			case StatsType.Skill_Cooltime:
				return PlatformManager.Language["str_ui_status_info_skill_cooldown"];
			case StatsType.Skill_Damage:
				return PlatformManager.Language["str_ui_status_info_skill_damage"];
			case StatsType.Mob_Damage_Buff:
				return PlatformManager.Language["str_ui_status_info_skill_mob_damage"];
			case StatsType.Boss_Damage_Buff:
				return PlatformManager.Language["str_ui_status_info_skill_boss_damage"];
			case StatsType.Atk_Buff:
				return "공격력 증폭";
			case StatsType.Hp_Buff:
				return "회복력 증폭";
			case StatsType.Buff_Gain_Gold:
				return PlatformManager.Language["str_ui_status_info_skill_gain_gold"];
			case StatsType.Buff_Gain_Exp:
				return PlatformManager.Language["str_ui_status_info_skill_gain_exp"];
			case StatsType.Buff_Gain_Item:
				return PlatformManager.Language["str_ui_status_info_skill_gain_item"];
			case StatsType.Final_Damage_Buff:
				return PlatformManager.Language["str_ui_status_info_skill_total_damage"];
			case StatsType.Damage_Reduce:
				return "데미지 감소";

			default:
				return $"{_state.ToString()}";
		}
	}
}

public static class ListUtil
{
	public static List<ELEM_TYPE> shuffleList<ELEM_TYPE>(List<ELEM_TYPE> src_list)
	{
		List<ELEM_TYPE> list = new List<ELEM_TYPE>();
		System.Random random = new System.Random();
		int num = 0;
		List<ELEM_TYPE> list2 = new List<ELEM_TYPE>(src_list);
		while (list2.Count > 0)
		{
			num = random.Next(0, list2.Count);
			list.Add(list2[num]);
			list2.RemoveAt(num);
		}

		return list;
	}

	public static ELEM_TYPE popList<ELEM_TYPE>(this List<ELEM_TYPE> list, int index = 0)
	{
		ELEM_TYPE result = list[index];
		list.RemoveAt(index);
		return result;
	}

	public static bool TryFirstOrDefault<T>(this IEnumerable<T> source, out T value)
	{
		value = default(T);
		using IEnumerator<T> enumerator = source.GetEnumerator();
		if (enumerator.MoveNext())
		{
			value = enumerator.Current;
			return true;
		}

		return false;
	}
}

public static class ConditionCheck
{
	public static bool IsFulFillCondition(this OpenCondition condition, out string message)
	{
		message = "";

#if UNITY_EDITOR
		if (PlatformManager.ConfigMeta.CheckContent == false)
		{
			return true;
		}
#endif

		switch (condition.type)
		{
			case ConditionType.CONTENT:

				message = $"{condition.content.ToUIString()} 오픈 필요";
				return PlatformManager.UserDB.contentsContainer.IsOpen(condition.content);

			case ConditionType.USELEVEL:
				{
					bool isOpen = PlatformManager.UserDB.userInfoContainer.userInfo.UserLevel >= condition.parameter;
					message = $"{condition.parameter} 달성 필요";
					return isOpen;
				}
			case ConditionType.QUEST:
				{
					var questInfo = PlatformManager.UserDB.questContainer.MainQuestList.Find(x => x.Tid == condition.tid);

					if (questInfo == null)
					{
						return true;
					}
					message = $"{PlatformManager.Language[questInfo.rawData.questTitle]} 클리어 필요";

					bool isOpen = questInfo.progressState == QuestProgressState.END;

					return isOpen;
				}
			case ConditionType.GUIDE:
				break;
			case ConditionType.STAGE:
				{
					long dungeonTid = condition.tid;
					int stageNumber = condition.parameter;

					var stage = PlatformManager.UserDB.stageContainer.GetNormalStage(stageNumber);
					message = $"STAGE {stageNumber} {stage.StageName} 클리어 필요";
					var isOpen = stage.isClear;
					return isOpen;
				}
			case ConditionType.DATETIME:
				{
					if (System.DateTime.TryParse(condition.dateTime, out System.DateTime conditionDate))
					{
						return (TimeManager.Instance.UtcNow - conditionDate).TotalSeconds > 0;
					}
					else
					{
						return true;
					}
				}
			default:
				return true;
		}
		return true;
	}
	public static bool IsPassFulfill(this RequirementInfo info, out string message)
	{
		message = "";
		if (PlatformManager.UserDB == null)
		{
			message = "사용자 데이터 정의가 되지 않음";
			return false;
		}


		switch (info.type)
		{
			case RequirementType.NORMAL_STAGE:
				{
					//long dungeonTid = info.parameter1;
					int stageNumber = info.parameter2;

					var stage = PlatformManager.UserDB.stageContainer.LastPlayedNormalStage();

					message = $"STAGE {stageNumber} {stage.StageName} 도달시 획득 가능";
					if (stage == null)
					{
						return false;
					}

					return stageNumber <= stage.StageNumber;
				}
			case RequirementType.USERLEVEL:
				{
					int requiredLevel = Mathf.FloorToInt(info.parameter2);
					var userlevel = PlatformManager.UserDB.userInfoContainer.userInfo.UserLevel;
					message = $"{requiredLevel} 달성시 획득 가능";
					return userlevel >= requiredLevel;
				}
			case RequirementType.DAILYKILLCOUNT:
				{
					int required = info.parameter2;
					message = $"{required} 달성시 획득 가능";
					return PlatformManager.UserDB.userInfoContainer.dailyKillCount >= required;
				}

			case RequirementType.NONE:
				return true;
		}

		return false;
	}


	public static bool IsRequirementFulfill(this RequirementInfo info, out string message)
	{
		message = "";
		if (PlatformManager.UserDB == null)
		{
			message = "사용자 데이터 정의가 되지 않음";
			return false;
		}

		switch (info.type)
		{
			case RequirementType.NORMAL_STAGE:
				{
					//long dungeonTid = info.parameter1;
					int stageNumber = info.parameter2;

					var stage = PlatformManager.UserDB.stageContainer.GetNormalStage(stageNumber);

					message = $"STAGE {stageNumber} {stage.Name} 클리어";
					if (stage == null)
					{
						return false;
					}

					return stage.isClear;
				}
			case RequirementType.USERLEVEL:
				{
					int requiredLevel = Mathf.FloorToInt(info.parameter2);
					var userlevel = PlatformManager.UserDB.userInfoContainer.userInfo.UserLevel;
					message = $"{requiredLevel} 이상 해제";
					return userlevel >= requiredLevel;
				}

			case RequirementType.BASESKILL:
				{
					long tid = info.parameter1;
					int baseSkillLevel = info.parameter2;
					var baseSkillInfo = PlatformManager.UserDB.skillContainer.FindSKill(tid);

					message = $"{baseSkillInfo.ItemName} {baseSkillLevel} 필요";

					if (baseSkillInfo == null)
					{
						return false;
					}

					return baseSkillInfo.Level >= baseSkillLevel;
				}
			case RequirementType.ADVANCEMENT:
				{
					int step = info.parameter2;
					message = "승급 부족";
					return PlatformManager.UserDB.advancementContainer.AdvancementLevel >= step;
				}
			case RequirementType.MONSTERKILL:
				{
					int required = info.parameter2;
					return true;
				}
			case RequirementType.DAILYKILLCOUNT:
				{
					int required = info.parameter2;
					message = "일일 몬스터 처치 수 부족";
					return PlatformManager.UserDB.userInfoContainer.dailyKillCount >= required;
				}

			case RequirementType.NONE:
				return true;
		}

		return false;
	}

}
