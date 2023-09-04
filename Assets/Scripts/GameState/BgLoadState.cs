using UnityEngine;

[CreateAssetMenu(fileName = "Bg Load State", menuName = "ScriptableObject/Stage/State/Bg Load", order = 1)]
public class BgLoadState : StageFSM
{
	public override FSM OnEnter()
	{
		GameUIManager.it.FadeCurtain(false);
		elapsedTime = 0;
		long tid = StageManager.it.CurrentStage.stageData.tid;
		StageType type = StageManager.it.CurrentStage.stageData.stageType;
		var data = PlatformManager.UserDB.stageContainer.GetStageMap(type, tid);
		StageManager.it.ChangeMap(data.mapPrefab);

		SoundManager.Instance.PlayBgm(data.bgmClip);


		if (StageManager.it.IsSameStage(StageManager.it.CurrentStage) == false)
		{
			PlatformManager.UserDB.userInfoContainer.userInfo.KillPerMinutes = 30;
			StageManager.it.killCountForOffline = 0;
			StageManager.it.stagePlayTickForOffline = Time.realtimeSinceStartupAsDouble;
		}

		var curStage = StageManager.it.CurrentStage;
		StageManager.it.PrevStage = new PrevStageInfo(curStage.StageType, curStage.stageData.tid, curStage.StageNumber);


		GameUIManager.it.ShowStageStart();

		return this;
	}
	public override FSM RunNextState(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			return nextState != null ? nextState.OnEnter() : this;

		}
		return this;
	}


	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		//elapsedTime += time;
		//if (elapsedTime > 1)
		//{
		//	VGameManager.it.ChangeState(GameState.PLAYERSPAWN);

		//}
	}
}

