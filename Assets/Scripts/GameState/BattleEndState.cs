
using UnityEngine;
[CreateAssetMenu(fileName = "Battle End State", menuName = "ScriptableObject/Stage/State/Battle End", order = 1)]
public class BattleEndState : StageFSM
{

	public override FSM OnEnter()
	{
		SceneCamera.it.StopCameraMove();
		elapsedTime = 0f;

		UnitGlobal.it.WaveFinish();


		GameUIManager.it.ShowStageResult(stageRule);
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

		//	VGameManager.it.ChangeState(GameState.LOADING);
		//}
	}

	public override FSM RunNextState(float time)
	{
		//elapsedTime += time;
		//if (elapsedTime > 1)
		//{
		//	//stageRule.End();

		//	return null;

		//}
		return this;
	}
}
