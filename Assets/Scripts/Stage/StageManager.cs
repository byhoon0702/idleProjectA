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


public class PrevStageInfo
{
	public StageType StageType { get; private set; }
	public long Tid { get; private set; }
	public int StageNumber { get; private set; }

	public PrevStageInfo(StageType stageType, long tid, int stageNumber)
	{
		StageType = stageType;
		Tid = tid;
		StageNumber = stageNumber;

	}
}

public class StageManager : MonoBehaviour
{
	private static StageManager instance;
	public static StageManager it => instance;

	public static event Action<RuntimeData.StageInfo> StageClearEvent;
	public event Action<double> OnStageTimeSpan;
	public event Action OnStageRewardAcquired;

	private RuntimeData.StageInfo currentStage = null;

	private int currentWaveCount;
	public int CurrentWave => currentWaveCount;

	public Transform mapRoot;
	public int currentKillCount { get; set; }
	public int bossKillCount { get; set; }
	public IdleNumber cumulativeDamage { get; set; }
	public RuntimeData.StageInfo CurrentStage => currentStage;
	public PrevStageInfo PrevStage { get; set; }

	public StageRule currentRule { get; set; }

	public bool continueBossChallenge { get; set; }
	public bool playBossStage { get; set; }
	public MapObject map { get; private set; }

	public double stagePlayTick { get; set; }


	//Offline
	public double stagePlayTickForOffline { get; set; }
	public int killCountForOffline { get; set; }
	//
	public bool usePhase { get; set; }

	public string SavedPlayStage
	{
		get
		{
			return $"{PlatformManager.UserDB.userInfoContainer.userInfo.UUID}_SavedPlayStage";
		}
	}
	public Dictionary<long, AddItemInfo> stageAcquiredReward { get; private set; } = new Dictionary<long, AddItemInfo>();


	[System.Serializable]
	public struct LocalSaveStage
	{
		public long tid;
		public int stageNumber;
		public LocalSaveStage(long tid, int stageNumber)
		{
			this.tid = tid;
			this.stageNumber = stageNumber;
		}
	}
	public bool isStartStage;
	private void Awake()
	{
		instance = this;
		isStartStage = false;

	}
	public void GetLocalSavedStage()
	{
		RuntimeData.StageInfo savedStage = null;
		if (PlayerPrefs.HasKey(SavedPlayStage))
		{
			string json = PlayerPrefs.GetString(SavedPlayStage);
			try
			{
				LocalSaveStage temp = JsonUtility.FromJson<LocalSaveStage>(json);
				savedStage = new RuntimeData.StageInfo(temp.tid, temp.stageNumber);
			}
			catch (Exception e)
			{
				savedStage = PlatformManager.UserDB.stageContainer.LastPlayedNormalStage();
			}
		}
		else
		{
			savedStage = PlatformManager.UserDB.stageContainer.LastPlayedNormalStage();
		}
		SetCurrentStage(savedStage);
	}

	public void SetLocalSaveStage(RuntimeData.StageInfo info)
	{
		LocalSaveStage stage = new LocalSaveStage(info.stageData.tid, info.StageNumber);
		string json = JsonUtility.ToJson(stage);
		PlayerPrefs.SetString(SavedPlayStage, json);
		PlayerPrefs.Save();

	}
	public void Init()
	{
		GetLocalSavedStage();

		currentStage.SetReward((IdleNumber)1);
		currentRule = currentStage.itemObject.rule;
		currentRule.Begin();
		bossSpawn = false;

	}

	public bool IsSameStage(RuntimeData.StageInfo info)
	{
		if (PrevStage == null)
		{
			return false;
		}

		if (PrevStage.StageType != info.StageType)
		{
			return false;
		}

		if (PrevStage.StageType != StageType.Normal)
		{
			if (PrevStage.Tid != info.stageData.tid)
			{
				return false;
			}
		}

		if (PrevStage.StageNumber != info.StageNumber)
		{
			return false;
		}

		return true;
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

	public void PlayStage(RuntimeData.StageInfo stageInfo)
	{
		playBoss = false;
		if (stageInfo.StageType == StageType.Normal)
		{
		}
		switch (stageInfo.StageType)
		{
			case StageType.Normal:
				SetLocalSaveStage(stageInfo);
				break;
			case StageType.Dungeon:
				PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.DUNGEON_CLEAR, stageInfo.stageData.dungeonTid, (IdleNumber)1);
				break;
			case StageType.Tower:
				PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.TOWER_CLEAR, stageInfo.stageData.dungeonTid, (IdleNumber)1);
				break;
			case StageType.Guardian:
				PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.GUARDIAN_CLEAR, stageInfo.stageData.dungeonTid, (IdleNumber)1);
				break;
			case StageType.Youth:
				PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.ADVANCEMENT, stageInfo.stageData.dungeonTid, (IdleNumber)1);
				break;
		}
		bossSpawn = false;

		GameManager.GameStop = false;
		stageAcquiredReward = new Dictionary<long, AddItemInfo>();
		SetCurrentStage(stageInfo);
		currentStage.SetReward((IdleNumber)1);
		currentRule = currentStage.itemObject.rule;
		currentRule.Begin();
	}

	public bool playBoss { get; private set; }

	public void OnClickBoss(RuntimeData.StageInfo stageInfo)
	{
		playBoss = true;
		SetCurrentStage(stageInfo);
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

		GetLocalSavedStage();
		PlayStage(currentStage);
	}


	public void NextNormalStage()
	{
		//if (continueBossChallenge)
		//{
		//	var bossStageInfo = VPlatformManager.UserDB.stageContainer.GetStage(StageType.Normal, UserInfo.stage.PlayingStageLv(StageType.Normal) + 1, StageManager.it.CurrentStage.Difficulty);
		//	OnClickBoss(bossStageInfo);
		//	return;
		//}

		var stage = PlatformManager.UserDB.stageContainer.GetNextStage(currentStage);
		SetCurrentStage(stage);

		PlayStage(currentStage);
	}

	public void SetCurrentStage(RuntimeData.StageInfo stage)
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
		SceneCamera.PlayableDirector.Pause();
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
		Vector3 pos = map.bossSpawnPos != null ? map.bossSpawnPos.position : new Vector3(2, 0, 0);
		if (CurrentStage.Rule is StageImmortal)
		{
			boss = SpawnManager.it.MakeImmotal(bossData, 0);
		}
		else
		{
			boss = SpawnManager.it.MakeBoss(bossData, pos, out VResult result);
		}


		TimelineAsset ta = SceneCamera.PlayableDirector.playableAsset as TimelineAsset;

		var tracks = ta.GetOutputTracks();
		foreach (var track in tracks)
		{
			if (track is UnitDissolveTrack)
			{
				SceneCamera.PlayableDirector.SetGenericBinding(track, boss.unitAnimation);
				break;
			}
		}

		UnitManager.it.Boss = boss;
		//bossSpawn = true;
		boss.unitAnimation.PlayDissolve(1.5f);
		//boss.position =

	}

	public void OnStageEnd(bool isWin)
	{
		isStartStage = false;
		stageAcquiredReward.Clear();
		if (isWin)
		{
			StageClearEvent?.Invoke(CurrentStage);
			PlatformManager.Instance.LeaderBoard.AddScore();

			if (CurrentStage.StageType == StageType.Dungeon || CurrentStage.StageType == StageType.Guardian)
			{
				var data = DataManager.Get<BattleDataSheet>().Get(CurrentStage.stageData.dungeonTid);

				var currency = PlatformManager.UserDB.inventory.FindCurrency(data.dungeonItemTid);

				currency.Pay((IdleNumber)1);
			}

			PlatformManager.UserDB.questContainer.ProgressAdd(QuestGoalType.STAGE_CLEAR_COUNT, CurrentStage.stageData.dungeonTid, (IdleNumber)1);
		}
	}

	public void AddAcquiredItem(List<RuntimeData.RewardInfo> rewardList)
	{
		if (rewardList == null)
		{
			return;
		}
		for (int i = 0; i < rewardList.Count; i++)
		{
			long tid = rewardList[i].Tid;
			if (stageAcquiredReward.ContainsKey(tid))
			{
				var info = stageAcquiredReward[tid];

				info.value += rewardList[i].fixedCount;
				stageAcquiredReward[tid] = info;
			}
			else
			{
				stageAcquiredReward.Add(tid, new AddItemInfo(rewardList[i]));
			}
		}
		OnStageRewardAcquired?.Invoke();
	}

	public void AddAcquiredItem(long tid, AddItemInfo itemInfo)
	{
		if (stageAcquiredReward.ContainsKey(tid))
		{
			var info = stageAcquiredReward[tid];

			info.value += itemInfo.value;
			stageAcquiredReward[tid] = info;
		}
		else
		{
			stageAcquiredReward.Add(tid, itemInfo);
		}

		OnStageRewardAcquired?.Invoke();
	}

	public void StagePlayTime()
	{
		OnStageTimeSpan?.Invoke(Time.realtimeSinceStartup - stagePlayTick);
	}

	public void SetOfflineKill()
	{
		if (PlatformManager.UserDB.userInfoContainer.userInfo.KillPerMinutes > 30)
		{
			return;
		}
		if (CurrentStage.StageType != StageType.Normal)
		{
			return;
		}

		if (killCountForOffline >= 1000)
		{
			TimeSpan ts = TimeSpan.FromSeconds(Time.realtimeSinceStartupAsDouble - stagePlayTickForOffline);
			PlatformManager.UserDB.userInfoContainer.userInfo.KillPerMinutes = (int)(1000 / ts.TotalMinutes);
		}

		if (killCountForOffline < 1000)
		{
			killCountForOffline++;
		}
	}

	public List<RuntimeData.RewardInfo> RewardPerKill(RuntimeData.StageInfo stageInfo, int kill)
	{

		List<RuntimeData.RewardInfo> rewardList = new List<RuntimeData.RewardInfo>();

		int totalCount = kill;
		var totalExp = stageInfo.GetMonsterExp() * totalCount;

		rewardList.Add(new RuntimeData.RewardInfo(0, RewardCategory.EXP, Grade.D, totalExp));

		var totalGold = stageInfo.GetMonsterGold() * totalCount;
		var goldItem = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.GOLD);
		rewardList.Add(new RuntimeData.RewardInfo(goldItem.Tid, RewardCategory.Currency, Grade.D, totalGold));

		Dictionary<long, RuntimeData.RewardInfo> infos = new Dictionary<long, RuntimeData.RewardInfo>();


		void AddItemToList(RuntimeData.RewardInfo reward)
		{
			if (infos.ContainsKey(reward.Tid) == false)
			{
				infos.Add(reward.Tid, reward.Clone());
			}
			else
			{
				infos[reward.Tid].AddCount(reward.fixedCount);
			}
		}
		for (int i = 0; i < totalCount; i++)
		{
			var totalReward = stageInfo.GetMonsterRewardList();
			for (int ii = 0; ii < totalReward.Count; ii++)
			{
				var reward = totalReward[ii];
				if (reward.Category == RewardCategory.RewardBox)
				{
					var list = PlatformManager.UserDB.OpenRewardBox(reward);
					for (int iii = 0; iii < list.Count; iii++)
					{
						AddItemToList(list[iii]);
					}
				}
				else
				{
					AddItemToList(reward);
				}
			}
		}

		rewardList.AddRange(infos.Values);
		return rewardList;
	}


	public void OfflineRewards(int totalMinutes)
	{
		if (PlatformManager.UserDB.contentsContainer.IsOpen(ContentType.OFFLINE_REWARD) == false)
		{
			return;
		}

		List<AddItemInfo> rewardList = new List<AddItemInfo>();
		int killcount = PlatformManager.UserDB.userInfoContainer.userInfo.KillPerMinutes;
		if (killcount < 30)
		{
			killcount = 30;
		}
		totalMinutes = Mathf.Min(totalMinutes, 12 * 60);
		int totalCount = killcount * totalMinutes;
		var totalExp = CurrentStage.GetMonsterExp() * totalCount;

		rewardList.Add(new AddItemInfo(0, totalExp, RewardCategory.EXP));

		var totalGold = CurrentStage.GetMonsterGold() * totalCount;
		var goldItem = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.GOLD);
		rewardList.Add(new AddItemInfo(goldItem.Tid, totalGold, RewardCategory.Currency));

		Dictionary<long, RuntimeData.RewardInfo> infos = new Dictionary<long, RuntimeData.RewardInfo>();


		void AddItemToList(RuntimeData.RewardInfo reward)
		{
			if (infos.ContainsKey(reward.Tid) == false)
			{
				infos.Add(reward.Tid, reward.Clone());
			}
			else
			{
				infos[reward.Tid].AddCount(reward.fixedCount);
			}
		}
		for (int i = 0; i < totalCount; i++)
		{
			var totalReward = CurrentStage.GetMonsterRewardList();
			for (int ii = 0; ii < totalReward.Count; ii++)
			{
				var reward = totalReward[ii];
				if (reward.Category == RewardCategory.RewardBox)
				{
					var list = PlatformManager.UserDB.OpenRewardBox(reward);
					for (int iii = 0; iii < list.Count; iii++)
					{
						AddItemToList(list[iii]);
					}
				}
				else
				{
					AddItemToList(reward);
				}
			}
		}

		foreach (var info in infos)
		{
			rewardList.Add(new AddItemInfo(info.Value));
		}


		GameUIManager.it.uiController.ShowOfflineRewardPopup(rewardList, totalMinutes, totalCount);
		PlatformManager.UserDB.AddRewards(rewardList, false);
	}

	private void OnDestroy()
	{
		StageClearEvent = null;
		OnStageTimeSpan = null;
		OnStageRewardAcquired = null;
	}
}
