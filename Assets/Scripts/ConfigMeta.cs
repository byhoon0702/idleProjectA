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

	/// <summary>
	/// 스턴시간
	/// </summary>
	[Tooltip("스턴 시간(s)")]
	[SerializeField] public float STUN_DURATION = 3;

	/// <summary>
	/// D ~ SS등급 버프시간
	/// </summary>
	[Tooltip(" D ~ SS등급 버프시간(s)")]
	[SerializeField] public float DSS_BUFF_DURATION = 2;

	/// <summary>
	/// SSS등급 버프시간
	/// </summary>
	[Tooltip("SSS등급 버프시간(s)")]
	[SerializeField] public float SSS_BUFF_DURATION = 6;

	/// <summary>
	/// D ~ SS등급 디버프시간
	/// </summary>
	[Tooltip("D ~ SS등급 디버프시간(s)")]
	[SerializeField] public float DSS_DEBUFF_DURATION = 2;

	/// <summary>
	/// SSS등급 디버프시간
	/// </summary>
	[Tooltip("SSS등급 디버프시간(s)")]
	[SerializeField] public float SSS_DEBUFF_DURATION = 5;

	/// <summary>
	/// D ~ SS등급 지속피해시간
	/// </summary>
	[Tooltip("D ~ SS등급 지속피해시간(s)")]
	[SerializeField] public float DSS_DOTE_DURATION = 2;

	/// <summary>
	/// SSS등급 지속피해시간
	/// </summary>
	[Tooltip("SSS등급 지속피해시간(s)")]
	[SerializeField] public float SSS_DOTE_DURATION = 5;

	/// <summary>
	/// 도트 들어가는 틱
	/// </summary>
	[Tooltip("도트 들어가는 틱(s)")]
	[SerializeField] public float DOTE_TICK = 1;


	/// <summary>
	/// 리절트코드 타이틀 기본 텍스트
	/// </summary>
	[Tooltip("리절트코드 타이틀 기본 텍스트")]
	[SerializeField] public string RESULT_CODE_DEFAULT_TITLE_TEXT = "Info";

	/// <summary>
	/// 리절트코드 타이틀 오류 기본 텍스트
	/// </summary>
	[Tooltip("리절트코드 타이틀 오류 기본 텍스트")]
	[SerializeField] public string RESULT_CODE_ERROR_TITLE_TEXT = "Error";

	/// <summary>
	/// 리절트코드 OK버튼 기본 텍스트
	/// </summary>
	[Tooltip("리절트코드 OK버튼 기본 텍스트")]
	[SerializeField] public string RESULT_CODE_DEFAULT_OK_TEXT = "Ok";

	/// <summary>
	/// 리절트코드 CANCEL버튼 기본 텍스트
	/// </summary>
	[Tooltip("리절트코드 CANCEL버튼 기본 텍스트")]
	[SerializeField] public string RESULT_CODE_DEFAULT_CANCEL_TEXT = "Cancel";

	/// <summary>
	/// [진급] 레벨당 공격력상승 비율
	/// </summary>
	[Tooltip("[진급] 레벨당 공격력상승 비율")]
	[SerializeField] public float PROMOTE_ATTACK_POWER_PER_LEVEL_RATIO = 0.01f;

	/// <summary>
	/// [진급] 레벨당 체력상승 비율
	/// </summary>
	[Tooltip("[진급] 레벨당 체력상승 비율")]
	[SerializeField] public float PROMOTE_HP_PER_LEVEL_RATIO = 0.02f;

	/// <summary>
	/// [진급] 레벨당 치명타피해 상승 비율
	/// </summary>
	[Tooltip("[진급] 레벨당 치명타피해 상승 비율")]
	[SerializeField] public float PROMOTE_CRITICAL_ATTACK_POWER_PER_LEVEL_RATIO = 0.01f;

	/// <summary>
	/// [진급] 레벨당 스킬피해 상승 비율
	/// </summary>
	[Tooltip("[진급] 레벨당 스킬피해 비율")]
	[SerializeField] public float PROMOTE_SKILL_ATTACK_POWER_PER_LEVEL_RATIO = 0.005f;

	/// <summary>
	/// [진급] 레벨당 보스피해 상승 비율
	/// </summary>
	[Tooltip("[진급] 레벨당 보스피해 상승 비율")]
	[SerializeField] public float PROMOTE_BOSS_ATTACK_POWER_PER_LEVEL_RATIO = 0.005f;
}
