public class BGLoadState : RootState
{
	public override void OnEnter()
	{
		//Loading BG
		//WhenLoading Finish 
		//FadeOut and Change State To Spawn
		GameUIManager.it.FadeCurtain(false);
		UIController.it.RefreshUI();
		elapsedTime = 0;
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			VGameManager.it.ChangeState(GameState.PLAYERSPAWN);

		}
	}
}

