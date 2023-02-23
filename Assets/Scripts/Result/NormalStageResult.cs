using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStageResult : ResultCondition
{
	public override bool IsWin()
	{
		if (SpawnManagerV2.it != null)
		{
			return SpawnManagerV2.it.IsAllEnemyDead;
		}
		else
		{
			return SpawnManager.it.IsAllEnemyDead;
		}
	}

	public override bool IsLose()
	{
		if (SpawnManagerV2.it != null)
		{
			return SpawnManagerV2.it.IsAllPlayerDead;
		}
		else
		{
			return SpawnManager.it.IsAllPlayerDead;
		}
	}
}
