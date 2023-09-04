

using UnityEngine;

[CreateAssetMenu(fileName = "Stage Normal", menuName = "ScriptableObject/Stage/Type/Normal", order = 1)]
public class StageNormal : StageRule
{
	public float spawnInterval = 5f;
	private float spawnTime;

	public override void Begin()
	{
		SceneCamera.PlayableDirector.playableAsset = timelineCutScene;
		base.Begin();
		spawnTime = 0;
		GameManager.it.battleRecord.bossKillCount = 0;


		GameUIManager.it.uiController.AdRewardChest.Init();
	}

	public override void End()
	{
		if (isWin == false)
		{
			StageManager.it.ReturnNormalStage();
		}
		else
		{

			StageManager.it.NextNormalStage();
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		if (currentFsm is SpawnState)
		{
			if (StageManager.it.continueBossChallenge || StageManager.it.playBossStage)
			{
				currentFsm = ChangeState(StageStateType.STAGECUTSCENE);
				StageManager.it.playBossStage = false;
				return;
			}
		}

		var next = currentFsm?.RunNextState(deltaTime);
		if (next != null)
		{
			currentFsm = StageFSM.Get(next);
		}
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


		GameUIManager.it.uiController.AdRewardChest.OnUpdate(deltaTime);

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
		if (StageManager.it.bossSpawn)
		{
			return;
		}
		if (spawnTime == 0)
		{
			SpawnManager.it.SpawnEnemies(StageManager.it.CurrentStage.SpawnMinDistance, StageManager.it.CurrentStage.SpawnMaxDistance);
		}
		spawnTime += time;
		if (spawnTime > spawnInterval)
		{
			spawnTime = 0;
		}
	}
}
