using UnityEngine;

[CreateAssetMenu(fileName = "Bg Load State", menuName = "ScriptableObject/Stage/State/Bg Load", order = 1)]
public class BgLoadState : StageFSM
{

	public override FSM OnEnter()
	{
		GameUIManager.it.FadeCurtain(false);
		elapsedTime = 0;
		long tid = StageManager.it.CurrentStage.dungeonData.tid;
		StageType type = StageManager.it.CurrentStage.dungeonData.stageType;
		var data = GameManager.UserDB.stageContainer.GetStageMap(type, tid);
		StageManager.it.ChangeMap(data.mapPrefab);


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

