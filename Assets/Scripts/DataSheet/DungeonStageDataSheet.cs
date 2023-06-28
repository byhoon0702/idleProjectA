using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
	/// <summary>	
	/// 적 처치 보상
	/// </summary>
	public DialougeInfo cutsceneDialogue;
	public RandomDialougeInfo randomDialogue;
}

[System.Serializable]
public class StageReward
{
	public List<Reward> itemRewards;
}
[System.Serializable]
public class StageItemReward
{
	public float chance;
	public long rewardBox;
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
public class DungeonEnemyInfo
{

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

	/// <summary>
	/// 보스 포함 화면에 뿌려질 숫자
	/// </summary>
	public int enemyDisplayCount;

	public int spawnPerWave;
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


	public DungeonEnemyInfo enemyInfo;
	/// <summary>
	/// 스테이지 클리어 보상
	/// </summary>
	public List<Reward> stageReward;

	/// <summary>
	/// 스테이지 몬스터 보상
	/// </summary>
	public List<Reward> monsterReward;

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
