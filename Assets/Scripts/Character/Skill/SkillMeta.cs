using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class SkillMeta
{
	public static string jsonFilePath
	{
		get
		{
			return $"{Application.dataPath}/AssetFolder/Resources/Json/Skill/";
		}
	}
	public static string presetClassPath
	{
		get
		{
			return $"{Application.dataPath}/Scripts/Character/SKill/SkillData/";
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

			return VGameManager.it.skillMeta;
		}
	}

	public Dictionary<Int64/*Tid*/, SkillBaseData> dic = new Dictionary<Int64, SkillBaseData>();

	public void LoadData()
	{
		dic.Clear();

		// 저장되어 있는 데이터 불러오기
		TextAsset[] skillAssets = Resources.LoadAll<TextAsset>("Json/Skill");

		foreach (var asset in skillAssets)
		{
			// 프리셋명(클래스 추출)
			SkillBaseData presetInfo = ScriptableObject.CreateInstance<SkillBaseData>();
			SkillBaseData skillBase;
			JsonUtility.FromJsonOverwrite(asset.text, presetInfo);

			try
			{
				var type = System.Type.GetType(presetInfo.skillPreset);
				var classObject = ScriptableObject.CreateInstance(type);
				JsonUtility.FromJsonOverwrite(asset.text, classObject);

				skillBase = classObject as SkillBaseData;
			}
			catch (Exception e)
			{
				VLog.SkillLogError($"스킬 초기화 실패. Json : {asset.name}\n{e}");
				GameObject.Destroy(presetInfo);
				continue; ;
			}

			dic.Add(presetInfo.tid, skillBase);

			GameObject.Destroy(presetInfo);
		}
	}
}

public enum BuffType
{
	/// <summary>
	/// 버프
	/// </summary>
	Buff,

	/// <summary>
	/// 디버프
	/// </summary>
	Debuff
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
}

public enum TargetingType
{
	/// <summary>
	/// 현재 타게팅하고 있는 적
	/// </summary>
	Default,

	/// <summary>
	/// 아군 전체
	/// </summary>
	FriendlyAll,

	/// <summary>
	/// 리스폰되어 있는 전체 적
	/// </summary>
	EnemyAll,

	/// <summary>
	/// 전방의 적
	/// </summary>
	FrontEnemy,

	/// <summary>
	/// 다수의 적
	/// </summary>
	ManyEnemy,

	/// <summary>
	/// 체력이 가장 많은 적
	/// </summary>
	HighestHPEnemy,
}
