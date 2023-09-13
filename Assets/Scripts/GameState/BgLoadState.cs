using UnityEngine;

[CreateAssetMenu(fileName = "Bg Load State", menuName = "ScriptableObject/Stage/State/Bg Load", order = 1)]
public class BgLoadState : StageFSM
{
	public override FSM OnEnter()
	{
		GameUIManager.it.FadeCurtain(false);
		elapsedTime = 0;

		var stage = StageManager.it.CurrentStage;


		StageManager.it.ChangeMap(stage.itemObject.mapPrefab);

		SoundManager.Instance.PlayBgm(stage.itemObject.bgmClip);

		if (StageManager.it.IsSameStage(stage) == false)
		{
			PlatformManager.UserDB.userInfoContainer.userInfo.KillPerMinutes = 30;
			StageManager.it.killCountForOffline = 0;
			StageManager.it.stagePlayTickForOffline = Time.realtimeSinceStartupAsDouble;
		}


		StageManager.it.PrevStage = new PrevStageInfo(stage.StageType, stage.stageData.tid, stage.StageNumber);


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

