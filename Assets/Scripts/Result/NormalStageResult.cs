using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStageResult : ResultCondition
{
	public override bool IsWin()
	{
		return SpawnManager.it.IsAllEnemyDead;
	}

	public override bool IsLose()
	{
		return SpawnManager.it.IsAllPlayerDead;
	}
}
