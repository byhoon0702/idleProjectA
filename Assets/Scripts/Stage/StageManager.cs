using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	private static StageManager instance;
	public static StageManager it => instance;

	private StageInfo currentStageInfo = null;

	public bool isCurrentStageLimited = false;

	public StageInfo CurrentStageInfo
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
		return DataManager.it.Get<StageInfoDataSheet>().GetNormalStage(_act, _stage);
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

	public void PlayNormalStage()
	{
		var stageInfo = GetNormalStageInfo(CurrentStageInfo.act, CurrentStageInfo.stage);
		currentStageInfo = stageInfo;
		VGameManager.it.ChangeState(GameState.BATTLEEND);
	}

	public void ClearBossStage()
	{
		var nextStageInfo = GetNextNormalStageInfo(CurrentStageInfo);
		currentStageInfo = nextStageInfo;
		VGameManager.it.ChangeState(GameState.BATTLEEND);
	}

	public void ClearNormalStage()
	{
		var nextStageInfo = GetNextNormalStageInfo(CurrentStageInfo);
		currentStageInfo = nextStageInfo;
		isCurrentStageLimited = true;
		VGameManager.it.ChangeState(GameState.BATTLEEND);
	}

	public void FailNormalStage()
	{
		isCurrentStageLimited = false;
		VGameManager.it.ChangeState(GameState.BATTLEEND);
	}
}
