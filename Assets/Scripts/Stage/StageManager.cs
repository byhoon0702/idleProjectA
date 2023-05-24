using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


[System.Serializable]
public class StageRuleDictionary : SerializableDictionary<StageType, StageRule>
{

}

public class StageManager : MonoBehaviour
{
	private static StageManager instance;
	public static StageManager it => instance;


	private StageInfo currentStage = null;

	private int currentWaveCount;
	public int CurrentWave => currentWaveCount;

	public Transform mapRoot;
	public int currentKillCount;
	public int bossKillCount;
	public StageInfo CurrentStage => currentStage;

	public StageRuleDictionary rules = new StageRuleDictionary();
	public StageRule currentRule;

	public PlayableDirector playableDirector;

	public bool continueBossChallenge = false;
	public MapObject map { get; private set; }

	private void Awake()
	{
		instance = this;

	}

	private void Start()
	{
		currentStage = GameManager.UserDB.stageContainer.LastPlayedStage();
		currentRule = rules[(StageType)currentStage.stageType];
		currentRule.Begin();
		//GameManager.UserDB.stageContainer.GetNextNormalStage(currentStage);
		bossSpawn = false;
	}

	public void ChangeMap(GameObject mapPrefab)
	{
		if (mapPrefab == null)
		{
			return;
		}
		if (map != null)
		{
			if (mapPrefab.name == map.name)
			{
				return;
			}
			Destroy(map.gameObject);
			map = null;
		}

		map = Instantiate(mapPrefab, mapRoot).GetComponent<MapObject>();
		map.name = map.name.Replace("(Clone)", "");
		map.transform.localPosition = Vector3.zero;
		map.transform.localScale = Vector3.one;
	}
	public void PlayStage(StageInfo _stageInfo)
	{
		bossSpawn = false;

		SetCurrentStage(_stageInfo);
		currentRule = rules[(StageType)currentStage.stageType];
		currentRule.Begin();
	}
	public void OnClickBoss(StageInfo _stageInfo)
	{
		SetCurrentStage(_stageInfo);
		currentRule.Begin(StageStateType.STAGECUTSCENE);
	}

	public void NextWave()
	{
		currentWaveCount++;
		//UIController.it.UiStageInfo.SetWaveGauge(Mathf.Clamp01((float)(currentWaveCount - 1) / currentStage.WaveCount));
	}

	public void RetryStage()
	{
		PlayStage(CurrentStage);
	}

	public void ReturnNormalStage()
	{
		continueBossChallenge = false;

		//var lastStageRecord = GameManager.UserDB.stageContainer.GetLastStage(StageType.Normal);
		//var stage = GameManager.UserDB.stageContainer.GetStage(StageType.Normal, lastStageRecord.stageNumber, lastStageRecord.stageDifficulty);
		//SetCurrentStage(stage);
		PlayStage(currentStage);
	}

	public void NextNormalStage()
	{
		//if (continueBossChallenge)
		//{
		//	var bossStageInfo = VGameManager.UserDB.stageContainer.GetStage(StageType.Normal, UserInfo.stage.PlayingStageLv(StageType.Normal) + 1, StageManager.it.CurrentStage.Difficulty);
		//	OnClickBoss(bossStageInfo);
		//	return;
		//}
		var lastStageRecord = GameManager.UserDB.stageContainer.GetLastStage(StageType.Normal);
		var stage = GameManager.UserDB.stageContainer.GetStage(StageType.Normal, lastStageRecord.stageNumber + 1, lastStageRecord.stageDifficulty);
		SetCurrentStage(stage);

		PlayStage(currentStage);
	}

	public void SetCurrentStage(StageInfo stage)
	{
		currentStage = stage;
		currentWaveCount = 0;
		currentStage.totalSpawnCount = 0;
		currentStage.totalBossSpawnCount = 0;
	}
	public void ResumeStage()
	{
		GameManager.GameStop = false;
	}

	public void PauseTimeline()
	{
		playableDirector.Pause();
	}


	/// <summary>
	/// 처치보상 처리
	/// </summary>
	public void CheckKillRewards(UnitType _unitType, Transform _transform = null)
	{
		//var rewards = currentStage.GenerateKillReward();

		//for (int i = 0; i < rewards.Count; i++)
		//{
		//	var info = rewards[i];

		//	if (info.Tid == Inventory.it.GoldTid && _transform != null)
		//	{
		//		UIController.it.ShowCoinEffect(_transform);
		//	}

		//	IdleNumber totalCount = (IdleNumber)info.RewardCount(_unitType);


		//	Inventory.it.AddItem(info.Tid, totalCount);
		//	UIController.it.ShowItemLog((int)info.Tid, totalCount);
		//}
	}

	public void Update()
	{
		currentRule?.OnUpdate(Time.deltaTime);
	}

	public bool bossSpawn = false;
	public void ShowBoss()
	{
		var bossData = CurrentStage.spawnBoss[UnityEngine.Random.Range(0, CurrentStage.spawnBoss.Count)];

		EnemyUnit boss = null;

		if (CurrentStage.stageType == StageType.Immortal)
		{
			boss = SpawnManager.it.MakeImmotal(bossData, 0);
		}
		else
		{
			boss = SpawnManager.it.MakeBoss(bossData, out VResult result);
		}


		TimelineAsset ta = playableDirector.playableAsset as TimelineAsset;

		var tracks = ta.GetOutputTracks();
		foreach (var track in tracks)
		{
			if (track is UnitDissolveTrack)
			{
				playableDirector.SetGenericBinding(track, boss.unitAnimation);
				break;
			}
		}

		UnitManager.it.Boss = boss;
		//bossSpawn = true;
		boss.unitAnimation.PlayDissolve(1.5f);
	}
}
