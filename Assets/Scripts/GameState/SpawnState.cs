public class SpawnState : RootState
{
	public override void OnEnter()
	{
		SpawnManager.it.SpawnCoroutine(() =>
		{
			GameManager.it.ChangeState(GameState.BATTLESTART);
		});
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{

	}
}
