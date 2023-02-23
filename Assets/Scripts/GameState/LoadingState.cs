using UnityEngine;

public class LoadingState : RootState
{
	public override void OnEnter()
	{
		UnitGlobal.it.skillModule.InitSkill(UserInfo.skills);
		UIController.it.SkillGlobal.OnUpdate();

		Inventory.it.Initialize(UserInfo.InstantItems);
		UIController.it.Init();

		VGameManager.it.mapController.Reset();
		GameUIManager.it.mainUIObject.SetActive(true);
		GameUIManager.it.ReleaseAllPool();
		if (SpawnManagerV2.it != null)
		{
			SpawnManagerV2.it.ClearUnits();
		}
		else
		{
			SpawnManager.it.ClearUnits();
		}
		ProjectileManager.it.ClearPool();
		elapsedTime = 0;


		if (SceneCameraV2.it != null)
		{
			SceneCameraV2.it.ResetToStart();
		}
		else
		{
			SceneCamera.it.ResetToStart();
		}
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

