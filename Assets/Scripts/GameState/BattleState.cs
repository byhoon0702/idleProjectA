public class BattleState : RootState
{
	int waveCount = 0;
	bool isWaveEnd = false;

	public override void OnEnter()
	{
		waveCount = 0;
		elapsedTime = 0;
		isWaveEnd = false;
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
			if (isWaveEnd == true)
			{
				if (SpawnManager.it.IsAllEnemyDead == true)
				{
					StageManager.it.ClearStage();
					StageManager.it.PlayNormalStage();
				}
			}
			if (elapsedTime >= 5f)
			{
				elapsedTime = 0;
				if (SpawnManager.it.SpawnEnemies(waveCount++) == false)
				{
					isWaveEnd = true;
				}
			}
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
