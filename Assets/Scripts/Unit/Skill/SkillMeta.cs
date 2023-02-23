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
			return $"{Application.dataPath}/AssetFolder/Resources/Data/Json/SkillRaw/";
		}
	}
	public static string presetClassPath
	{
		get
		{
			return $"{Application.dataPath}/Scripts/Unit/SKill/SkillData/";
		}
	}

	public Dictionary<Int64/*Tid*/, SkillBaseData> dic = new Dictionary<Int64, SkillBaseData>();

	public void LoadData()
	{
		dic.Clear();

		// 저장되어 있는 데이터 불러오기
		TextAsset[] skillAssets = Resources.LoadAll<TextAsset>("Data/Json/SkillRaw");

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

public enum UnitCondition
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
	/// <summary>
	/// 스턴
	/// </summary>
	Stun,
}

public enum SkillBuffType
{
	AttackPower,
}


public enum TargetingType
{
	/// <summary>
	/// 현재 타게팅하고 있는 적
	/// </summary>
	Default,

	/// <summary>
	/// 적군의 중앙
	/// </summary>
	Center,

	/// <summary>
	/// 체력 가장 많은 적
	/// </summary>
	HighestHP,

	/// <summary>
	/// 체력 가장 적은 적
	/// </summary>
	lowestHP,

	/// <summary>
	/// 랜덤
	/// </summary>
	Random,
}



public enum DirectionType
{
	Up,
	Down,
	Left,
	Right
}
