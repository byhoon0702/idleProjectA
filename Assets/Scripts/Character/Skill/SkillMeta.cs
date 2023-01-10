using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillMeta : ScriptableObject
{
	public static string filePath
	{
		get
		{
			return $"{Application.dataPath}/AssetFolder/Resources/Json/";
		}
	}

	public static string fileName
	{
		get
		{
			return $"Skill.json";
		}
	}

	public static SkillMeta it
	{
		get
		{
			if (Application.isPlaying == false)
			{
				return null;
			}

			return GameManager.it.skillDictionary;
		}
	}

	[SerializeField] public Serina_sk1Data serina_Sk1Data;
	[SerializeField] public Landrock_sk1Data landrock_Sk1Data;
	[SerializeField] public Mirfiana_sk1Data mirfiana_Sk1Data;
	[SerializeField] public Haru_sk1Data haru_Sk1Data;
	[SerializeField] public Gilius_sk1Data gilius_Sk1Data;

	public bool initialized;


	public Dictionary<string, SkillBaseData> dic = new Dictionary<string, SkillBaseData>();

	public void CreateDictionary()
	{
		dic.Clear();

		serina_Sk1Data = ScriptableObject.CreateInstance<Serina_sk1Data>();
		landrock_Sk1Data = ScriptableObject.CreateInstance<Landrock_sk1Data>();
		mirfiana_Sk1Data = ScriptableObject.CreateInstance<Mirfiana_sk1Data>();
		haru_Sk1Data = ScriptableObject.CreateInstance<Haru_sk1Data>();
		gilius_Sk1Data = ScriptableObject.CreateInstance<Gilius_sk1Data>();

		dic.Add(typeof(Serina_sk1).ToString(), serina_Sk1Data);
		dic.Add(typeof(Landrock_sk1).ToString(), landrock_Sk1Data);
		dic.Add(typeof(Mirfiana_sk1).ToString(), mirfiana_Sk1Data);
		dic.Add(typeof(Haru_sk1).ToString(), haru_Sk1Data);
		dic.Add(typeof(Gilius_sk1).ToString(), gilius_Sk1Data);
	}
}

public enum CharacterCondition
{
	/// <summary>
	/// 공격력 증가
	/// </summary>
	AttackPowerUp,
	/// <summary>
	/// 공격력 감소
	/// </summary>
	AttackPowerDown,

	/// <summary>
	/// 공격속도 증가
	/// </summary>
	AttackSpeedUp,
	/// <summary>
	/// 공격속도 감소
	/// </summary>
	AttackSpeedDown,

	/// <summary>
	/// 크리티컬확률 증가
	/// </summary>
	CriticalChanceUp,
	/// <summary>
	/// 크리티컬확률 감소
	/// </summary>
	CriticalChanceDown,

	/// <summary>
	/// 피해량 증가
	/// </summary>
	DamageUp,
	/// <summary>
	/// 피해량 감소
	/// </summary>
	DamageDown,

	/// <summary>
	/// 이동속도 증가
	/// </summary>
	MoveSpeedUp,
	/// <summary>
	/// 이동속도 감소
	/// </summary>
	MoveSpeedDown,

	/// <summary>
	/// 넉백
	/// </summary>
	Knockback,
	/// <summary>
	/// 스턴
	/// </summary>
	Stun,
	/// <summary>
	/// 독
	/// </summary>
	Poison,
}


public static class SkillTidDictionary // 추후 테이블로 교체할거 같으면 이걸로
{
	public static Dictionary<Int64, string> dic = new Dictionary<Int64, string>()
	{
		{ 1001, typeof(Serina_sk1).ToString() },
		{ 1002, typeof(Landrock_sk1).ToString() },
		{ 1003, typeof(Mirfiana_sk1).ToString() },
		{ 1004, typeof(Haru_sk1).ToString() },
		{ 1005, typeof(Gilius_sk1).ToString() },
	};

	public static string GetSkillName(Int64 _tid)
	{
		return dic[_tid];
	}

	public static Int64 GetSkillTid(string skillName)
	{
		foreach (var skill in dic)
		{
			if (skill.Value == skillName)
			{
				return skill.Key;
			}
		}

		return 0;
	}
}
