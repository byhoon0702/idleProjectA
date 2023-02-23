using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfigMeta : ScriptableObject
{
	private static ConfigMeta instance;
	public static ConfigMeta it
	{
		get
		{
			if (Application.isPlaying == false)
			{
				return null;
			}

			if(instance == null)
			{
				instance = ScriptableObject.CreateInstance<ConfigMeta>();
			}

			return instance;
		}
	}

	public static string filePath
	{
		get
		{
			return $"{Application.dataPath}/AssetFolder/Resources/Data/Json/";
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
	/// 스턴시간
	/// </summary>
	[Tooltip("넉백시간")]
	[SerializeField] public float STUN_DURATION = 3;

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
	/// 버프 적용시간(s)
	/// </summary>
	[Tooltip("버프 적용시간(s)")]
	[SerializeField] public float BUFF_DURATION = 2;

	/// <summary>
	/// 타겟 검색 주기
	/// </summary>
	[Tooltip("타겟 검색 주기")]
	[SerializeField] public float TARGET_SEARCH_DELAY = 0.1f;

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

	/// <summary>
	/// 코어 능력 갱신할때 coreabilpoint 소비 기본값
	/// </summary>
	[Tooltip("코어 능력 갱신할때 coreabilpoint 소비 기본값")]
	[SerializeField] public long CORE_ABILITY_DEFAULT_CONSUME_COUNT = 10;


	/// <summary>
	/// 리필(리셋)형 아이템 업데이트 사이클(s)
	/// </summary>
	[Tooltip("리필(리셋)형 아이템 업데이트 사이클(s)")]
	[SerializeField] public float REFILL_UPDATE_CYCLE = 10;

	/// <summary>
	/// 아군 근거리 공격범위
	/// </summary>
	[Tooltip("아군 근거리 공격범위")]
	[SerializeField] public float PLAYER_TARGET_RANGE_CLOSE = 1.5f;

	/// <summary>
	/// 아군 원거리 공격범위
	/// </summary>
	[Tooltip("아군 원거리 공격범위")]
	[SerializeField] public float PLAYER_TARGET_RANGE_FAR = 6;

	/// <summary>
	/// 적 근거리 공격범위
	/// </summary>
	[Tooltip("적 근거리 공격범위")]
	[SerializeField] public float ENEMY_TARGET_RANGE_CLOSE = 1.5f;

	/// <summary>
	/// 적 원거리 공격범위
	/// </summary>
	[Tooltip("적 원거리 공격범위")]
	[SerializeField] public float ENEMY_TARGET_RANGE_FAR = 6;

	/// <summary>
	/// 보스 근거리 공격범위
	/// </summary>
	[Tooltip("보스 근거리 공격범위")]
	[SerializeField] public float BOSS_TARGET_RANGE_CLOSE = 3.0f;

	/// <summary>
	/// 보스 원거리 공격범위
	/// </summary>
	[Tooltip("보스 원거리 공격범위")]
	[SerializeField] public float BOSS_TARGET_RANGE_FAR = 6;

	/// <summary>
	/// 일반 스테이지 젠 주기
	/// </summary>
	[Tooltip("일반 스테이지 젠 주기")]
	[SerializeField] public float NORMAL_DUNGEON_SPAWN_CYCLE = 10.0f;

	/// <summary>
	/// 플레이어 캐릭터 초당 체력회복량
	/// </summary>
	[Tooltip("플레이어 캐릭터 체력회복량 속도(s)")]
	[SerializeField] public float PLAYER_HP_RECOVERY_CYCLE = 1f;

	/// <summary>
	/// 범위형 스킬 터지는 위치(캐릭터 기준 전방 a미터)
	/// </summary>
	[Tooltip(" 스킬 터지는 위치(캐릭터 기준 전방 a미터)")]
	[SerializeField] public float RANGE_SKILL_POSITION = 6f;

	/// <summary>
	/// 범위형 스킬의 범위
	/// </summary>
	[Tooltip("범위형 스킬의 범위")]
	[SerializeField] public float RANGE_SKILL_RADIUS = 6f;


	/// <summary>
	/// 골드 상자 일일획득 가능수치
	/// </summary>
	[Tooltip("골드 상자 일일획득 가능수치")]
	[SerializeField] public int DAILY_GOLD_BOX_LIMIT = 5;

	/// <summary>
	/// 골드 광고상자의 최소등장주기
	/// </summary>
	[Tooltip("골드 광고상자의 최소등장주기")]
	[SerializeField] public float MINIMUM_GOLD_AD_FREQUENCY = 300f;

	/// <summary>
	/// 골드 광고상자의 최소등장주기
	/// </summary>
	[Tooltip("골드 광고상자의 최대등장주기")]
	[SerializeField] public float MAXIMUM_GOLD_AD_FREQUENCY = 3600f;

	/// <summary>
	/// 다이아 상자 일일획득 가능수치
	/// </summary>
	[Tooltip("다이아 상자 일일획득 가능수치")]
	[SerializeField] public int DAILY_DIA_BOX_LIMIT = 5;

	/// <summary>
	/// 다이아 광고상자의 최소등장주기
	/// </summary>
	[Tooltip("다이아 광고상자의 최소등장주기")]
	[SerializeField] public float MINIMUM_DIA_AD_FREQUENCY = 300f;

	/// <summary>
	/// 다이아 광고상자의 최소등장주기
	/// </summary>
	[Tooltip("다이아 광고상자의 최대등장주기")]
	[SerializeField] public float MAXIMUM_DIA_AD_FREQUENCY = 4200f;

	/// <summary>
	/// 유닛 레벨업 경험치증가량
	/// </summary>
	[Tooltip("유닛 레벨업 경험치증가량")]
	[SerializeField] public int UNIT_LEVELUP_EXP = 15;

}
