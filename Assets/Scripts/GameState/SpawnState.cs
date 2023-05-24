using System;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Spawn State", menuName = "ScriptableObject/Stage/State/Battle Spawn", order = 1)]
public class SpawnState : StageFSM
{

	bool isSpawnEnd = false;

	public override FSM OnEnter()
	{
		VSoundManager.it.PlayBgm("main_bgm");
		isSpawnEnd = false;
		UIController.it.UiStageInfo.OnUpdate(StageManager.it.CurrentStage);
		SpawnManager.it.SpawnCoroutine(() =>
		{
			isSpawnEnd = true;
		});

		return this;
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{

	}

	public override FSM RunNextState(float time)
	{
		if (isSpawnEnd)
		{
			return nextState != null ? nextState.OnEnter() : this;
		}
		return this;
	}
}
