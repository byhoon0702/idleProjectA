using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public enum StageType
{
	Normal = 0,

	Dungeon = 10,
	Tower = 20,
	Guardian = 30,


	/// <summary>
	/// 회춘 던전
	/// </summary>
	Youth = 40,
	Event_Dungeon = 50,
	Event_Tower = 60,
	Event_Guardian = 70,
}

[System.Serializable]
public class StageState
{
	public StageFSM current;
	public StageStateType nextType;

}

[System.Serializable]
public class StageStateDictionary : SerializableDictionary<StageStateType, StageState>
{ }


public class StageRule : ScriptableObject
{
	public StageClearCondition[] clearConditions;
	public StageFailCondition[] failConditions;

	[SerializeField] protected TimelineAsset timelineCutScene;
	public bool isCutsceneExist => timelineCutScene != null;
	protected float elapsedTime;

	[SerializeField] protected StageStateDictionary stateSerializableDictionary;
	[SerializeField] protected Dictionary<StageStateType, StageFSM> stateDictionary;
	[NonSerialized] public StageFSM currentFsm;

	public List<RuntimeData.RewardInfo> displayRewardList = new List<RuntimeData.RewardInfo>();
	public bool isWin { get; protected set; } = false;
	public bool isEnd { get; protected set; } = false;
	public virtual void Begin()
	{
		UIController.it.UiStageInfo.TurnOffUI();
		stateDictionary = new Dictionary<StageStateType, StageFSM>();
		elapsedTime = 0;
		isEnd = false;

		StageManager.it.currentKillCount = 0;
		StageManager.it.bossKillCount = 0;
		StageManager.it.cumulativeDamage = (IdleNumber)0;
		StageManager.it.usePhase = false;

		foreach (var state in stateSerializableDictionary)
		{
			if (state.Value.current == null)
			{
				continue;
			}

			StageFSM nextFsm = null;
			if (stateSerializableDictionary.ContainsKey(state.Value.nextType))
			{
				nextFsm = stateSerializableDictionary[state.Value.nextType].current;
			}
			state.Value.current.Init(this, nextFsm);
		}

		currentFsm = (StageFSM)stateSerializableDictionary[StageStateType.LOADING].current.OnEnter();
	}

	public StageFSM ChangeState(StageStateType type)
	{
		return (StageFSM)stateSerializableDictionary[type].current.OnEnter();
	}

	public void SetCondition()
	{
		for (int i = 0; i < clearConditions.Length; i++)
		{
			clearConditions[i].SetCondition();
		}
		for (int i = 0; i < failConditions.Length; i++)
		{
			failConditions[i].SetCondition();
		}
	}

	public virtual void Begin(StageStateType type)
	{
		currentFsm = (StageFSM)stateSerializableDictionary[type].current.OnEnter();
	}
	public virtual void OnUpdate(float deltaTime)
	{
		var next = currentFsm?.RunNextState(deltaTime);
		if (next != null)
		{
			currentFsm = StageFSM.Get(next);
		}
	}

	public virtual void OnLogicUpdate(float deltaTime)
	{
		for (int i = 0; i < clearConditions.Length; i++)
		{
			clearConditions[i].OnUpdate(deltaTime);
		}
		for (int i = 0; i < failConditions.Length; i++)
		{
			failConditions[i].OnUpdate(deltaTime);
		}
	}

	public virtual void End() { }
	public bool CheckWin()
	{
		if (clearConditions == null || clearConditions.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < clearConditions.Length; i++)
		{
			if (clearConditions[i].CheckCondition() == false)
			{
				return false;
			}
		}

		return true;
	}

	public bool CheckLose()
	{
		if (failConditions == null || failConditions.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < failConditions.Length; i++)
		{
			if (failConditions[i].CheckCondition())
			{
				return true;
			}
		}

		return false;
	}

	public virtual void AddReward()
	{
		displayRewardList = new List<RuntimeData.RewardInfo>();
		StageManager.it.CurrentStage.SetStageReward((IdleNumber)StageManager.it.CurrentStage.StageNumber - 1);
		List<RuntimeData.RewardInfo> rewardList = new List<RuntimeData.RewardInfo>();

		var list = StageManager.it.CurrentStage.GetStageRewardList();
		rewardList = RewardUtil.ReArrangReward(list);
		displayRewardList.AddRange(rewardList);

		PlatformManager.UserDB.AddRewards(rewardList, false);
	}

	public bool CheckEnd()
	{
		if (CheckLose())
		{
			isWin = false;
			isEnd = true;
			StageManager.it.OnStageEnd(false);
			return true;
		}
		if (CheckWin())
		{
			isWin = true;
			isEnd = true;

			var stage = StageManager.it.CurrentStage;
			if (stage.isClear == false && stage.StageType == StageType.Normal)
			{
				PlatformManager.Firebase.StageLog(stage.StageNumber);
			}

			StageManager.it.SaveStage();

			stage.isClear = true;
			AddReward();

			StageManager.it.OnStageEnd(true);
			PlatformManager.UserDB.Save();
			return true;
		}
		return false;
	}
}
