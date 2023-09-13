using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Infinity", menuName = "ScriptableObject/Stage/Type/Infinity", order = 1)]
public class StageInfinity : StageRule
{
	public float spawnInterval = 5f;
	private float spawnTime;

	public override void Begin()
	{
		base.Begin();
	}

	public override void End()
	{
		StageManager.it.ReturnNormalStage();
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

		if (CheckEnd())
		{
			return;
		}

		base.OnLogicUpdate(deltaTime);
		SpawnUpdate(deltaTime);
		elapsedTime += deltaTime;
	}

	public override void AddReward()
	{
		var currentStage = StageManager.it.CurrentStage;

		currentStage.SetReward((IdleNumber)StageManager.it.currentKillCount);
		List<RuntimeData.RewardInfo> rewardList = new List<RuntimeData.RewardInfo>();
		List<RuntimeData.RewardInfo> totalrewardList = new List<RuntimeData.RewardInfo>();

		//totalrewardList.AddRange(StageManager.it.CurrentStage.StageClearReward);
		//totalrewardList.Add(currentStage.MonsterExp);
		totalrewardList.Add(currentStage.MonsterGold);
		totalrewardList.AddRange(currentStage.GetMonsterRewardList());
		displayRewardList = new List<RuntimeData.RewardInfo>();
		rewardList = RewardUtil.ReArrangReward(totalrewardList);
		displayRewardList.AddRange(rewardList);
		PlatformManager.UserDB.AddRewards(rewardList, false);
	}

	private void SpawnUpdate(float time)
	{
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
