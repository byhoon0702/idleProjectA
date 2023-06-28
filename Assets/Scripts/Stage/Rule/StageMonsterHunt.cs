using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage Monster Hunt", menuName = "ScriptableObject/Stage/Type/Monster Hunt", order = 1)]
public class StageMonsterHunt : StageRule
{
	public bool withBoss;

	public float spawnInterval = 5f;
	private float spawnTime;
	private StageInfo currentInfo;
	private bool noMoreSpawn;

	public override void Begin()
	{
		base.Begin();
		currentInfo = StageManager.it.CurrentStage;
		spawnTime = 0;
		noMoreSpawn = false;
	}

	public override void End()
	{
		StageManager.it.ReturnNormalStage();
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
	}


	public override void OnLogicUpdate(float deltaTime)
	{
		if (GameManager.GameStop)
		{
			return;
		}
		if (isEnd)
		{
			return;
		}

		base.OnLogicUpdate(deltaTime);
		// 플레이어 죽음
		if (CheckEnd())
		{
			return;
		}

		SpawnUpdate(deltaTime);
		elapsedTime += deltaTime;
	}

	private void SpawnUpdate(float time)
	{
		if (noMoreSpawn)
		{
			return;
		}
		if (spawnTime == 0)
		{
			if (GameManager.it.battleRecord.killCount >= currentInfo.CountLimit && currentInfo.totalBossSpawnCount < currentInfo.BossCountLimit)
			{
				SpawnManager.it.SpawnLast(currentInfo.spawnBoss[0], 0);
				noMoreSpawn = true;
			}
			else
			{
				SpawnManager.it.SpawnEnemies(currentInfo.SpawnMinDistance, currentInfo.SpawnMaxDistance);
			}

		}
		spawnTime += time;
		if (spawnTime > spawnInterval)
		{
			spawnTime = 0;
		}
	}



}
