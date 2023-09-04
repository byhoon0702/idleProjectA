using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum QuestProgressState
{
	/// <summary>
	/// 진행 불가
	/// </summary>
	NONE = 0,
	/// <summary>
	/// 진행 가능
	/// </summary>
	ACTIVE,
	/// <summary>
	/// 진행 중
	/// </summary>
	ONPROGRESS,
	/// <summary>
	/// 목표 달성
	/// </summary>
	COMPLETE,
	/// <summary>
	/// 완료 (보상받기)
	/// </summary>
	END,
}

public enum QuestType
{
	MAIN,
	DAILY,
	REPEAT,
}

public enum QuestGoalType
{
	/// <summary>
	/// 컨텐츠 진입
	/// </summary>
	/// //!
	CONTENTS_ENTER = 1,
	/// <summary>
	/// 몬스터 사냥
	/// </summary>
	/// //!
	MONSTER_HUNT = 2,

	/// <summary>
	/// 특정 스테이지 클리어
	/// </summary>
	/// //!
	STAGE_CLEAR = 11,
	/// <summary>
	/// 스테이지 클리어 횟수
	/// </summary>
	STAGE_CLEAR_COUNT = 12,

	/// <summary>
	/// 재화 모음
	/// </summary>
	GATHER_CURRENCY = 20,
	/// <summary>
	/// 재화 소모
	/// </summary>
	CONSUME_CURRENCY = 21,

	/// <summary>
	/// 유저레벨 업
	/// </summary>
	/// //!
	USER_LEVELUP = 30,
	/// <summary>
	/// 유저레벨 달성
	/// </summary>
	/// //!
	USER_LEVEL = 31,

	/// <summary>
	/// 능력치 레벨업
	/// </summary>
	/// //!
	ABILITY_LEVELUP = 40,
	/// <summary>
	/// 능력치 레벨 달성
	/// </summary>
	/// //!
	ABILITY_LEVEL = 41,

	/// <summary>
	/// 장비 소환
	/// </summary>
	/// //!
	SUMMON_EQUIP = 50,
	//아이템(장비) 장착

	//장비 강화
	//!
	LEVELUP_WEAPON = 51,
	//장비 돌파
	//!
	BREAKTHROUGH_WEAPON = 52,
	//장비 진화 (아직없음)
	EVOLUTION_WEPON = 53,
	//장비 승급
	//!
	UPGRADE_WEAPON = 54,

	/// <summary>
	/// 펫 소환
	/// </summary>
	/// //!
	SUMMON_PET = 60,
	//!
	SUMMON_SKILL = 62,
	//!
	SUMMON_RELIC = 64,

	/// <summary>
	/// 플레이 시간
	/// </summary>
	PLAY_TIME = 100,

	/// <summary>
	/// 전체 플레이시간
	/// </summary>

	/// !
	TOTAL_PLAY_TIME = 101,

	/// <summary>
	/// 일일 미션 완료
	/// </summary>
	/// !
	DAILY_COMPLETE = 200,

	//!
	EQUIP_ITEM = 301,
	//코스튬 장착
	//!
	EQUIP_COSTUME = 302,
	//펫 장착
	//!
	EQUIP_PET = 303,
	//스킬 장착
	//!
	EQUIP_SKILL = 304,

	//!
	LEVEL_VETERANCY = 307,

	//!
	LEVELUP_SKILL = 305,
	//!
	LEVELUP_RELIC = 306,

	//!
	LEVELUP_PET = 308,
	//!
	LEVELUP_AWAKENRUNE = 309,
	//!
	LEVELUP_AWAKENING = 310,

	//!
	EVOLUTION_SKILL = 311,
	//!
	EVOLUTION_PET = 312,

	ACTIVATE_BUFF = 313,
	ACTIVATE_AUTO_SKILL = 314,
	ACTIVATE_AUTO_AWAKEN = 315,

	DUNGEON_CLEAR = 320,
	TOWER_CLEAR = 321,
	GUARDIAN_CLEAR = 322,

	ADVANCEMENT = 330,
	EQUIP_COSTUME_HYPER = 340,
}

[System.Serializable]
public class QuestData : BaseData
{
	public string questName;
	public string questTitle;
	public QuestType questType;
	public QuestGoalType goalType;

	public long goalTid;
	public string goalValue;

	public List<ChanceReward> rewards;
	public bool useGuide;
}

[System.Serializable]
public class QuestDataSheet : DataSheetBase<QuestData>
{
	public List<QuestData> GetDataByType(QuestType type)
	{
		return infos.FindAll(x => x.questType == type);
	}
}
