public class BattleState : RootState
{
	private int waveCount;

	public override void OnEnter()
	{
		waveCount = 1;
		elapsedTime = 0;
		if (SceneCameraV2.it != null)
		{
			SceneCameraV2.it.ActivateCameraMove();
		}
		else
		{
			SceneCamera.it.ActivateCameraMove();
		}
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		switch (StageManager.it.CurrentStageType)
		{
			case StageType.NORMAL:
				UpdateNormalStage(time);
				break;
			case StageType.CHASING:
				UpdateChasingDungeon(time);
				break;
		}
	}

	private void SpawnEnemies()
	{
		if (SpawnManagerV2.it != null)
		{
			SpawnManagerV2.it.SpawnEnemies();
		}
		else
		{
			SpawnManager.it.SpawnEnemies();
		}
		waveCount++;
	}

	private void SpawnBoss()
	{
		if (SpawnManagerV2.it != null)
		{
			SpawnManagerV2.it.SpawnBoss();
		}
		else
		{
			SpawnManager.it.SpawnBoss();
		}
		waveCount++;
	}

	private void UpdateTimeAttackStage(float _time)
	{

	}

	private void UpdateBossRelayStage(float _time)
	{

	}

	private void UpdateNormalStage(float _time)
	{
		if (SpawnManagerV2.it != null)
		{
			if (SpawnManagerV2.it.IsAllPlayerDead == true)
			{
				UIController.it.ShowDefeatNavigator();
				StageManager.it.FailNormalStage();
				return;
			}

			if (SpawnManagerV2.it.IsBossClear == true)
			{
				StageManager.it.ClearNormalStage();
				return;
			}
			else if (SpawnManagerV2.it.IsAllEnemyDead == true)
			{
				elapsedTime += _time;

				bool isInfiniteSpawn = StageManager.it.isCurrentStageLimited == false;
				bool isBossWave = waveCount >= StageManager.it.CurrentNormalStageInfo.waveCount;
				bool isSpawnTime = elapsedTime >= ConfigMeta.it.NORMAL_DUNGEON_SPAWN_CYCLE / SpawnManagerV2.it.playerUnit.info.MoveSpeed();

				if (isSpawnTime == false)
				{
					return;
				}

				if (isInfiniteSpawn == true)
				{
					SpawnManagerV2.it.ClearDeadEnemy();
					SpawnEnemies();
					elapsedTime = 0;
					return;
				}
				else
				{
					if (isBossWave == true)
					{
						SpawnBoss();
						elapsedTime = 0;
						return;
					}
					else
					{
						SpawnEnemies();
						elapsedTime = 0;
						return;
					}
				}
			}
		}
		else
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
				elapsedTime += _time;

				bool isInfiniteSpawn = StageManager.it.isCurrentStageLimited == false;
				bool isBossWave = waveCount >= StageManager.it.CurrentNormalStageInfo.waveCount;
				bool isSpawnTime = elapsedTime >= ConfigMeta.it.NORMAL_DUNGEON_SPAWN_CYCLE / SpawnManager.it.playerUnit.info.MoveSpeed();

				if (isSpawnTime == false)
				{
					return;
				}

				if (isInfiniteSpawn == true)
				{
					SpawnManager.it.ClearDeadEnemy();
					SpawnEnemies();
					elapsedTime = 0;
					return;
				}
				else
				{
					if (isBossWave == true)
					{
						SpawnBoss();
						elapsedTime = 0;
						return;
					}
					else
					{
						SpawnEnemies();
						elapsedTime = 0;
						return;
					}
				}
			}
		}
	}

	private void UpdateChasingDungeon(float _time)
	{
		if (SpawnManagerV2.it != null)
		{
			if (SpawnManagerV2.it.IsBossClear == true)
			{
				StageManager.it.ClearChasingStage();
				return;
			}
			else if (SpawnManagerV2.it.IsBossDead == true)
			{
				SpawnManagerV2.it.SpawnBoss();
			}
		}
		else
		{
			if (SpawnManager.it.IsBossClear == true)
			{
				StageManager.it.ClearChasingStage();
				return;
			}
			else if (SpawnManager.it.IsBossDead == true)
			{
				SpawnManager.it.SpawnBoss();
			}
		}
	}
}
