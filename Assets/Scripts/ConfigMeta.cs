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

			return VGameManager.it.config;
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

	/// <summary>
	/// 전방의 적 체크시 범위(m)
	/// </summary>
	[Tooltip("전방의 적 체크시 범위(m)")]
	[SerializeField] public float TARGET_SEARCH_FRONT_ENEMY_RANGE = 2;

	/// <summary>
	/// 다수의 적 체크시 범위(m)
	/// </summary>
	[Tooltip("다수의 적 체크시 범위(m)")]
	[SerializeField] public float TARGET_SEARCH_MANY_ENEMY_RANGE = 4;

	/// <summary>
	/// 넉백시간
	/// </summary>
	[Tooltip("넉백시간")]
	[SerializeField] public float KNOCKBACK_DURATION = 0.5f;

	/// <summary>
	/// 넉백거리
	/// </summary>
	[Tooltip("넉백거리(m)")]
	[SerializeField] public float KNOCKBACK_DISTANCE = 1;
}
