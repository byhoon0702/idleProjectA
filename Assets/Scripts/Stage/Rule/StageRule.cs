using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public enum StageType
{

	Normal = 0,

	/// <summary>
	/// 참깨 동굴
	/// </summary>
	Sesame = 3,
	/// <summary>
	/// 용사의 무덤
	/// </summary>
	Tomb,
	/// <summary>
	/// 오염된 부화장
	/// </summary>
	Hatchery,
	/// <summary>
	/// 불로초 평원
	/// </summary>
	Immortal,
	/// <summary>
	/// 회춘 던전
	/// </summary>
	Youth,

	/// <summary>
	/// 악몽의 탑
	/// </summary>
	NightmareTower = 101,
	/// <summary>
	/// 활력
	/// </summary>
	Vitality,
	/// <summary>
	/// 냉기의 수호자
	/// </summary>
	Guardian_Frost,
	/// <summary>
	/// 화염의 수호자
	/// </summary>
	Guardian_Flame,
	/// <summary>
	/// 바위의 수호자
	/// </summary>
	Guardian_Stone,
	/// <summary>
	/// 어둠의 수호자
	/// </summary>
	Guardian_Dark,
	/// <summary>
	/// 부서진 배의 수호자
	/// </summary>
	Guardian_Shipwreck,
	/// <summary>
	/// 화산의 수호자
	/// </summary>
	Guardian_Volcano,

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


public abstract class StageRule : ScriptableObject
{
	public StageClearCondition[] clearConditions;
	public StageFailCondition[] failConditions;

	[SerializeField] protected TimelineAsset timelineCutScene;

	public bool isCutsceneExist => timelineCutScene != null;
	protected float elapsedTime;

	[SerializeField] protected StageStateDictionary stateSerializableDictionary;
	[SerializeField] protected Dictionary<StageStateType, StageFSM> stateDictionary;
	[NonSerialized] public StageFSM currentFsm;

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

	public abstract void End();
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

	public bool CheckEnd()
	{
		if (CheckLose())
		{
			isWin = false;
			isEnd = true;
			return true;
		}
		if (CheckWin())
		{
			StageManager.it.CurrentStage.isClear = true;

			GameManager.UserDB.stageContainer.SavePlayStage(StageManager.it.CurrentStage);
			isWin = true;
			isEnd = true;
			return true;
		}
		return false;
	}
}
