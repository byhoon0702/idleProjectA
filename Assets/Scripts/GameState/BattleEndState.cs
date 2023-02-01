
public class BattleEndState : RootState
{
	float elapsedTime;

	public override void OnEnter()
	{
		SceneCamera.it.StopCameraMove();
		elapsedTime = 0f;

		UnitGlobal.it.WaveFinish();
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			GameUIManager.it.FadeCurtain(true);
			VGameManager.it.ChangeState(GameState.LOADING);
		}
	}
}
