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
		SceneCamera.PlayableDirector.playableAsset = timelineCutScene;
		base.Begin();
		UIController.it.UiStageInfo.RefreshDPSCount();
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
		UIController.it.UiStageInfo.RefreshDPSCount();
		base.OnLogicUpdate(deltaTime);

		elapsedTime += deltaTime;

		if (count == 0)
		{
			SpawnBoss();
		}
	}
	public override void AddReward()
	{
		StageManager.it.CurrentStage.SetStageReward(StageManager.it.cumulativeDamage);
		List<RuntimeData.RewardInfo> rewardList = new List<RuntimeData.RewardInfo>();

		var list = StageManager.it.CurrentStage.StageClearReward;

		displayRewardList = new List<RuntimeData.RewardInfo>();
		rewardList = RewardUtil.ReArrangReward(list);
		displayRewardList.AddRange(rewardList);
		PlatformManager.UserDB.AddRewards(rewardList, false);
	}
	private void SpawnBoss()
	{
		count++;
		//	SpawnManager.it.SpawnImmotal(StageManager.it.CurrentStage.spawnLast, 1);
	}
}
