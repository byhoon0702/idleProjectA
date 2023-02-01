public class BattleState : RootState
{
	private int waveCount;

	public override void OnEnter()
	{
		waveCount = 1;
		elapsedTime = 0;
		SceneCamera.it.ActivateCameraMove();
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		switch (StageManager.it.CurrentStageType)
		{
			case StageType.NORMAL:
				{
					if (SpawnManager.it.IsAllPlayerDead == true)
					{
						UIController.it.ShowDefeatNavigator();
						StageManager.it.FailNormalStage();
						return;
					}

					if (SpawnManager.it.IsBossClear == true)
					{
						StageManager.it.ClearNormalStage();
						return;
					}
					else if (SpawnManager.it.IsAllEnemyDead == true)
					{
						elapsedTime += time;

						bool isInfiniteSpawn = StageManager.it.isCurrentStageLimited == false;
						bool isWaveFinish = waveCount > StageManager.it.CurrentStageInfo.waveCount;
						bool isBossWave = waveCount == StageManager.it.CurrentStageInfo.waveCount;
						bool isSpawnTime = elapsedTime >= ConfigMeta.it.NORMAL_DUNGEON_SPAWN_CYCLE / SpawnManager.it.playerCharacter.info.MoveSpeed();

						if (isSpawnTime == false)
						{
							return;
						}

						if (isInfiniteSpawn == true)
						{
							SpawnManager.it.ClearDeadEnemy();
							SpawnEnemies();
							return;
						}
						else
						{
							if (isBossWave == true)
							{
								SpawnBoss();
								return;
							}
							else
							{
								SpawnEnemies();
								return;
							}
						}
					}
				}
				break;
		}
	}

	private void SpawnEnemies()
	{
		SpawnManager.it.SpawnEnemies();
		waveCount++;
		elapsedTime = 0;
	}

	private void SpawnBoss()
	{
		SpawnManager.it.SpawnBoss();
		waveCount++;
		elapsedTime = 0;
	}
}
