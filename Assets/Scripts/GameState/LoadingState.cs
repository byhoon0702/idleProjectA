using UnityEngine;

public class LoadingState : RootState
{
	public override void OnEnter()
	{
		VGameManager.it.mapController.Reset();
		GameUIManager.it.mainUIObject.SetActive(true);
		GameUIManager.it.ReleaseAllPool();
		SpawnManager.it.ClearCharacters();
		ProjectileManager.it.ClearProjectiles();
		elapsedTime = 0;

		SceneCamera.it.ResetToStart();
	}

	public override void OnExit()
	{
		VSoundManager.it.PlayBgm("main_bgm");
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

