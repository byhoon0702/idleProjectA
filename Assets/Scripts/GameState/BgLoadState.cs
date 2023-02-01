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

		var stageInfo = StageManager.it.CurrentStageInfo;

		var bgData = DataManager.it.Get<BgDataSheet>().Get(stageInfo.bgTid);

		VGameManager.it.mapController.SetBG(bgData.bgCloseName, bgData.bgMiddleName, bgData.bgFarName);
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

