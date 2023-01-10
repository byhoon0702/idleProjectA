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

		switch (StageManager.it.CurrentStageType)
		{
			case StageManager.StageType.BOSS:
			case StageManager.StageType.NORMAL1:
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
				break;
			case StageManager.StageType.NORMAL2:
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
				break;
			case StageManager.StageType.INFINITE:
				{
					if (elapsedTime >= 5f)
					{
						elapsedTime = 0;
						SpawnManager.it.ClearDeadEnemy();
						SpawnManager.it.SpawnEnemies(waveCount++);
					}
				}
				break;
		}
	}
}
