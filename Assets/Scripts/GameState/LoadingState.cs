using UnityEngine;
[CreateAssetMenu(fileName = "Loading State", menuName = "ScriptableObject/Stage/State/Loading", order = 1)]
public class LoadingState : StageFSM
{
	public override FSM OnEnter()
	{
		GameUIManager.it.FadeCurtain(true, GameManager.it.firstEnter);

		GameManager.it.firstEnter = false;
		UIController.it.SkillGlobal.OnUpdate();
		UIController.it.Init();

		GameUIManager.it.mainUIObject.SetActive(true);
		GameUIManager.it.ReleaseAllPool();
		SpawnManager.it.ClearUnits();

		elapsedTime = 0;

		SceneCamera.it.ResetToStart();
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
		//	VGameManager.it.ChangeState(GameState.BGLOADING);

		//}
	}
}

