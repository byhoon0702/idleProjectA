﻿public class LoadingState : RootState
{
	public override void OnEnter()
	{
		VGameManager.it.mapController.Reset();

		GameUIManager.it.ReleaseAllPool();
		SpawnManager.it.ClearCharacters();
		elapsedTime = 0;
		//FadeIn
		//Clear Object 

		SceneCamera.it.ResetToStart();
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			VGameManager.it.ChangeState(GameState.BGLOADING);

		}
	}
}

