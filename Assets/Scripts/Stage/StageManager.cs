using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	private static StageManager instance;
	public static StageManager it => instance;

	private StageInfo currentStageInfo = null;
	private StageInfo currentNormalStageInfo = null;

	public bool isCurrentStageLimited = false;

	public StageInfo CurrentStageInfo => currentStageInfo;

	public StageInfo CurrentNormalStageInfo
	{
		get
		{
			if (currentStageInfo == null)
			{
				currentStageInfo = GetNormalStageInfo(1, 1);
			}
			return currentStageInfo;
		}
	}

	public StageType CurrentStageType
	{
		get
		{
			if (currentStageInfo == null)
			{
				return StageType.NONE;
			}
			return currentStageInfo.stageType;
		}
	}

	private void Start()
	{
	}

	private void Awake()
	{
		instance = this;
	}

	public StageInfo GetNormalStageInfo(int _act, int _stage)
	{
		return DataManager.Get<StageInfoDataSheet>().GetNormalStage(_act, _stage);
	}

	public StageInfo GetNextNormalStageInfo(StageInfo _currentStageInfo)
	{
		var currentAct = _currentStageInfo.act;
		var currentStage = _currentStageInfo.stage;

		StageInfo nextStageInfo = null;

		nextStageInfo = GetNormalStageInfo(currentAct, currentStage + 1);

		if (nextStageInfo == null)
		{
			nextStageInfo = GetNormalStageInfo(currentAct + 1, currentStage);
		}
		if (nextStageInfo == null)
		{
			nextStageInfo = GetNormalStageInfo(1, 1);
		}
		return nextStageInfo;
	}

	public void PlayStage(StageInfo _stageInfo)
	{
		currentStageInfo = _stageInfo;
		if (_stageInfo.stageType == StageType.NORMAL)
		{
			currentNormalStageInfo = _stageInfo;
		}
		VGameManager.it.ChangeState(GameState.BATTLEEND);
	}

	public void ClearChasingStage()
	{
		PlayStage(currentNormalStageInfo);
	}

	public void ClearNormalStage()
	{
		var nextStageInfo = GetNextNormalStageInfo(CurrentNormalStageInfo);
		isCurrentStageLimited = true;
		PlayStage(nextStageInfo);
	}

	public void FailNormalStage()
	{
		isCurrentStageLimited = false;
		PlayStage(currentStageInfo);
	}

	public void ResetStage()
	{
		VGameManager.it.ChangeState(GameState.BATTLEEND);
	}

	public void GetReward(Transform _transform = null)
	{
		for (int i = 0; i < currentStageInfo.stageRewardInfoList.Count; i++)
		{
			var info = currentStageInfo.stageRewardInfoList[i];
			if (GetResult(info.dropRate) == true)
			{
				if (info.tid == Inventory.it.GoldTid && _transform != null)
				{
					UIController.it.ShowCoinEffect(_transform);
				}
				Inventory.it.AddItem(info.tid, new IdleNumber(info.count));
				UIController.it.ShowItemLog((int)info.tid, new IdleNumber(info.count));
			}
		}
	}

	public void GetBossKillReward(Transform _transform = null)
	{
		for (int i = 0; i < currentStageInfo.bossRewardInfoList.Count; i++)
		{
			var info = currentStageInfo.bossRewardInfoList[i];
			{
				if (info.tid == Inventory.it.GoldTid && _transform != null)
				{
					UIController.it.ShowCoinEffect(_transform);
				}
				Inventory.it.AddItem(info.tid, new IdleNumber(info.count));
			}
		}
	}

	private bool GetResult(float _percentage)
	{
		float randomValue = UnityEngine.Random.Range(0, 100);
		if (randomValue < _percentage)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
