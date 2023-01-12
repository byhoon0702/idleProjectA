
public class BattleEndState : RootState
{
	public override void OnEnter()
	{
		elapsedTime = 0;
		SceneCamera.it.StopCameraMove();
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 3)
		{
			GameUIManager.it.FadeCurtain(true);
			VGameManager.it.ChangeState(GameState.LOADING);
		}
	}
}
