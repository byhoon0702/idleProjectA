using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillMeta : ScriptableObject
{
	public static string filePath
	{
		get
		{
			return $"{Application.dataPath}/AssetFolder/Resources/Json/Skill/";
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

			return GameManager.it.skillMeta;
		}
	}

	public Serina_sk1Data serina_Sk1Data;
	public Landrock_sk1Data landrock_Sk1Data;
	public Mirfiana_sk1Data mirfiana_Sk1Data;
	public Haru_sk1Data haru_Sk1Data;
	public Gilius_sk1Data gilius_Sk1Data;

	[NonSerialized] public Dictionary<string, SkillBaseData> dic = new Dictionary<string, SkillBaseData>();

	public void LoadData()
	{
		dic.Clear();

		// 기본 데이터 초기화
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



		// 저장되어 있는 데이터 불러오기
		HashSet<string> matched = new HashSet<string>();
		TextAsset[] skillAssets = Resources.LoadAll<TextAsset>("Json/Skill");

		foreach(var asset in skillAssets)
		{
			foreach (var data in dic)
			{
				if (asset.name == $"{data.Key}Data")
				{
					JsonUtility.FromJsonOverwrite(asset.text, data.Value);
					matched.Add(data.Key);
				}
			}
		}


		// 스킬정보에 관련된 json파일이 없는 경우 경고 띄워주기
		foreach(var data in dic)
		{
			if(matched.Contains(data.Key) == false)
			{
				VLog.SkillLogWarning($"스킬정보가 담긴 json파일 찾지 못함. 기본값 사용됨. {data.Key}Data");
			}
		}
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
