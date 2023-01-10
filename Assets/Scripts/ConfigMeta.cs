using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfigMeta : ScriptableObject
{
	public static ConfigMeta it
	{
		get
		{
			if (Application.isPlaying == false)
			{
				return null;
			}

			return GameManager.it.config;
		}
	}

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
			return $"Config.json";
		}
	}

	/// <summary>
	/// 랜덤 대미지 범위
	/// </summary>
	[Tooltip("랜덤 대미지 범위")]
	[SerializeField] public float ATTACK_POWER_RANGE = 0.1f;

	/// <summary>
	/// 공격속도 최소치
	/// </summary>
	[Tooltip("공격속도 최소치")]
	[SerializeField] public float ATTACK_SPEED_MIN = 0.1f;

	/// <summary>
	/// 공격속도 최대치
	/// </summary>
	[Tooltip("공격속도 최대치")]
	[SerializeField] public float ATTACK_SPEED_MAX = 3;

	/// <summary>
	/// 크리티컬 확률 최대치
	/// </summary>
	[Tooltip("크리티컬 확률 최대치")]
	[SerializeField] public float CRITICAL_CHANCE_MAX_RATIO = 0.8f;

	/// <summary>
	/// 받는 피해 최소량
	/// </summary>
	[Tooltip("받는 피해 최소량")]
	[SerializeField] public float MIN_DAMAGE_MUL = 0.01f;

	/// <summary>
	/// 받는 피해 최대량
	/// </summary>
	[Tooltip("받는 피해 최대량")]
	[SerializeField] public float MAX_DAMAGE_MUL = 5;
}
