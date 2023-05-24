using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StageMonsterStatsInfo
{
	public string hp;
	public string attackPower;
	public float attackSpeed;
	public float moveSpeed;
}
[System.Serializable]
public class DialougeInfo
{
	public long tid;
	public float time;
}
[System.Serializable]
public class RandomDialougeInfo
{
	public List<long> tid;
	public float time;
}


[System.Serializable]
public class DungeonMonsterData
{
	public long tid;
	public bool isBoss;
	public bool isEvent;
	/// <summary>
	/// 적 처치 보상
	/// </summary>
	public Reward[] killRewardList;
	public DialougeInfo cutsceneDialogue;
	public RandomDialougeInfo randomDialogue;
}

[System.Serializable]
public class Reward
{
	/// <summary>
	/// 아이템 Tid
	/// </summary>
	public long tid;
	/// <summary>
	/// 등장 스테이지 계산용
	/// </summary>
	public int appearStage;
	/// <summary>
	/// 확률
	/// </summary>
	public float probability;
	/// <summary>
	/// 수량
	/// </summary>
	public string count;
}

[System.Serializable]
public class StageData
{

	public int stageNumber;
	/// <summary>
	/// 레벨 가중치 (스테이지 번호 + levelweight)
	/// </summary>
	public float levelWeight;
	/// <summary>
	/// 보스 가중치 (보스 가중치 기본 + bossWeight)
	/// </summary>
	public float bossWeight;
	/// <summary>
	/// 스폰 리스트 덮어쓰기
	/// </summary>
	public List<DungeonMonsterData> overrideSpawnList;
}


[System.Serializable]
public class DungeonStageData : BaseData
{
	public string name;
	/// <summary>
	/// 스테이지 타입
	/// </summary>
	public StageType stageType;
	/// <summary>
	/// 지역 번호
	/// </summary>
	public int areaNumber;

	/// <summary>
	/// 스테이지 난이도(일반만 씀)
	/// </summary>
	public StageDifficulty difficulty;

	#region 스테이지 종료 조건
	/// <summary>
	/// 일반 몹 스폰 숫자
	/// </summary>
	public int enemyCountLimit;

	/// <summary>
	/// 보스 몹 스폰 숫자
	/// </summary>
	public int bossCountLimit;

	/// <summary>
	/// 시간 제한
	/// </summary>
	public float timelimit;
	#endregion
	/// <summary>
	/// 보스 포함 화면에 뿌려질 숫자
	/// </summary>
	public int enemyDisplayCount;

	/// <summary>
	/// 스테이지 클리어 보상
	/// </summary>
	public Reward[] stageRewardList;

	/// <summary>
	/// 스테이지 별 적 능력치 가중치
	/// </summary>
	public StageMonsterStatsInfo monsterStats;

	/// <summary>
	/// 적 스폰 리스트
	/// </summary>
	public List<DungeonMonsterData> spawnList;
	/// <summary>
	/// 스테이지 정보
	/// </summary>
	public StageData[] stageData;
}

[System.Serializable]
public class DungeonStageDataSheet : DataSheetBase<DungeonStageData>
{


}
