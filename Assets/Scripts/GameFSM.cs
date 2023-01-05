using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootState : FiniteStateMachine
{
	protected float elapsedTime = 0;

	public virtual void Init()
	{

	}
	public virtual void OnEnter()
	{
		elapsedTime = 0;
		//FadeIn
		//Clear Object 
	}

	public virtual void OnExit()
	{

	}

	public virtual void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			GameManager.it.ChangeState(GameState.BGLOADING);

		}
	}
}


public class LoadingState : RootState
{
	public override void OnEnter()
	{

		GameUIManager.it.ReleaseAllPool();
		SpawnManager.it.ClearCharacters();
		elapsedTime = 0;
		//FadeIn
		//Clear Object 

		SceneCamera.it.ResetToStart();
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			GameManager.it.ChangeState(GameState.BGLOADING);

		}
	}
}

public class BGLoadState : RootState
{
	public override void OnEnter()
	{
		//Loading BG
		//WhenLoading Finish 
		//FadeOut and Change State To Spawn
		GameUIManager.it.FadeCurtain(false);
		UIController.it.RefreshUI();
		elapsedTime = 0;
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			GameManager.it.ChangeState(GameState.PLAYERSPAWN);

		}
	}
}

public class SpawnState : RootState
{
	public override void OnEnter()
	{
		SpawnManager.it.SpawnCoroutine(() =>
		{
			GameManager.it.ChangeState(GameState.BATTLESTART);
		});
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{

	}
}

public class BattleStartState : RootState
{
	public override void OnEnter()
	{
		elapsedTime = 0;
		GameManager.it.battleRecord = new BattleRecord();
		GameManager.it.battleRecord.InitCharacter(CharacterManager.it.GetCharacters(true));
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			GameManager.it.ChangeState(GameState.BATTLE);
		}
	}
}

public class BattleState : RootState
{
	int waveCount = 0;

	public override void OnEnter()
	{
		waveCount = 0;
		elapsedTime = 0;
		SceneCamera.it.ActivateCameraMove();
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		// 패배
		if (SpawnManager.it.IsAllPlayerDead == true)
		{
			UIController.it.ShowDefeatNavigator();
			StageManager.it.PlayNormalStage();
			return;
		}

		// 몹을 전부 무찔렀을 때 리스폰하는 방식
		if (StageManager.it.CurrentStageType == StageManager.StageType.NORMAL1)
		{
			if (SpawnManager.it.IsAllEnemyDead == true)
			{
				// 웨이브가 끝남
				if (SpawnManager.it.SpawnEnemies(waveCount++) == false)
				{
					StageManager.it.ClearStage();
					StageManager.it.PlayNormalStage();
				}
			}
		}
		// 일정 시간마다 적이 리스폰되는 방식
		else if (StageManager.it.CurrentStageType == StageManager.StageType.NORMAL2)
		{
			//if (elapsedTime >= 5f)
			//{
			//	elapsedTime = 0;
			//	if (SpawnManager.it.SpawnEnemies(waveCount++) == false)
			//	{

			//	}
			//}
		}
		// 보스전
		else if (StageManager.it.CurrentStageType == StageManager.StageType.BOSS)
		{
			if (SpawnManager.it.IsAllEnemyDead == true)
			{
				// 웨이브가 끝남
				if (SpawnManager.it.SpawnEnemies(waveCount++) == false)
				{
					StageManager.it.ClearStage();
					StageManager.it.PlayNormalStage();
				}
			}
		}
	}
}

public class BattleEndState : RootState
{
	public override void OnEnter()
	{
		elapsedTime = 0;
		SceneCamera.it.StopCameraMove();
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 3)
		{
			GameUIManager.it.FadeCurtain(true);
			GameManager.it.ChangeState(GameState.LOADING);
		}
	}
}
