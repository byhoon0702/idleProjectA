public class SpawnState : RootState
{
	public override void OnEnter()
	{
		if (SpawnManagerV2.it != null)
		{
			SpawnManagerV2.it.SpawnCoroutine(() =>
			{
				VGameManager.it.ChangeState(GameState.BATTLESTART);
			});
		}
		else
		{
			SpawnManager.it.SpawnCoroutine(() =>
			{
				VGameManager.it.ChangeState(GameState.BATTLESTART);
			});
		}
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{

	}
}
