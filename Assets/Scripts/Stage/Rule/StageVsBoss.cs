using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stage Vs Boss", menuName = "ScriptableObject/Stage/Type/Vs Boss", order = 1)]
public class StageVsBoss : StageRule
{
	public float interval = 0f;
	public bool usePhase;
	private int bossIndex = 0;
	public override void Begin()
	{
		bossIndex = 0;
		base.Begin();
		StageManager.it.usePhase = usePhase;
	}

	public override void End()
	{
		StageManager.it.ReturnNormalStage();
	}

	public override void OnLogicUpdate(float deltaTime)
	{
		if (isEnd)
		{
			return;
		}

		if (CheckEnd())
		{
			return;
		}

		base.OnLogicUpdate(deltaTime);

		elapsedTime += deltaTime;

		if ((interval > 0 && elapsedTime > interval) || UnitManager.it.GetBosses().Count == 0)
		{
			SpawnBoss();
			elapsedTime = 0;
		}


	}

	private void SpawnBoss()
	{

		int displayCount = StageManager.it.CurrentStage.DisplayUnitCount;
		int bossSpawnCount = StageManager.it.CurrentStage.totalBossSpawnCount;

		int countLimit = StageManager.it.CurrentStage.BossCountLimit;
		int perWaveCount = Mathf.Min(StageManager.it.CurrentStage.SpawnPerWave, Mathf.Min(displayCount - UnitManager.it.GetBosses().Count));

		if (countLimit > 0)
		{
			int leftCount = countLimit - bossSpawnCount;
			perWaveCount = Mathf.Min(leftCount, perWaveCount);
		}

		if (perWaveCount == 0)
		{
			return;
		}

		int i = 0;
		for (; i < perWaveCount; i++)
		{
			int index = bossIndex + i;
			index = StageManager.it.CurrentStage.spawnBoss.Count > index ? index : 0;
			Vector3 pos = StageManager.it.map.bossSpawnPos != null ? StageManager.it.map.bossSpawnPos.position : new Vector3(2, 0, 0);
			SpawnManager.it.SpawnLast(StageManager.it.CurrentStage.spawnBoss[index], pos, 1);
			bossIndex++;

		}
	}
}
