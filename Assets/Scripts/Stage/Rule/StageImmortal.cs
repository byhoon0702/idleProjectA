using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Immortal", menuName = "ScriptableObject/Stage/Type/Immortal", order = 1)]
public class StageImmortal : StageRule
{
	public bool infinite;

	private int count = 0;
	public override void Begin()
	{
		count = 0;
		StageManager.it.playableDirector.playableAsset = timelineCutScene;
		base.Begin();
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

		if (count == 0)
		{
			SpawnBoss();
		}
	}

	internal void SpawnBoss()
	{
		count++;
		//	SpawnManager.it.SpawnImmotal(StageManager.it.CurrentStage.spawnLast, 1);
	}
}
