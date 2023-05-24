using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public static class ExtensionMethod
{
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

	public static string ToUIString(this StatsType _state)
	{
		switch (_state)
		{
			case StatsType.Atk:
				return "공격력";
			case StatsType.Hp:
				return "체력";
			case StatsType.Atk_Speed:
				return "공격 속도";
			case StatsType.Crits_Chance:
				return "치명타 확률";
			case StatsType.Crits_Damage:
				return "치명타 피해";
			case StatsType.Super_Crits_Chance:
				return "회심의 일격 확률";
			case StatsType.Super_Crits_Damage:
				return "회심의 일격 피해";
			case StatsType.Move_Speed:
				return "이동속도";
			case StatsType.Hp_Recovery:
				return "회복력";
			case StatsType.Skill_Cooltime:
				return "스킬 쿨타임";
			case StatsType.Skill_Damage:
				return "스킬 피해";
			case StatsType.Mob_Damage_Buff:
				return "몬스터 추가 피해";
			case StatsType.Boss_Damage_Buff:
				return "보스 몬스터 추가 피해";
			case StatsType.Atk_Buff:
				return "공격력 증폭";
			case StatsType.Hp_Buff:
				return "회복력 증폭";
			case StatsType.Gold_Buff:
				return "골드 추가 획득";
			case StatsType.EXP_Buff:
				return "경험치 추가 획득";
			case StatsType.Item_Buff:
				return "아이템 추가 획득";
			case StatsType.Final_Damage_Buff:
				return "최종 피해";
			case StatsType.Hyper_Atk:
				return "하이퍼 모드 공격력";
			case StatsType.Hyper_Hp:
				return "하이퍼 모드 체력";
			case StatsType.Hyper_Atk_Speed:
				return "하이퍼 모드 공격 속도";
			case StatsType.Hyper_Move_Speed:
				return "하이퍼 모드 이동 속도";
			case StatsType.Hyper_Duration:
				return "하이퍼 모드 지속 시간";
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
