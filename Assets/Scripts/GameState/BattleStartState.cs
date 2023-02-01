public class BattleStartState : RootState
{
	public override void OnEnter()
	{
		elapsedTime = 0;
		VGameManager.it.battleRecord = new BattleRecord();
		UnitGlobal.it.WaveStart();
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			VGameManager.it.ChangeState(GameState.BATTLE);
		}
	}
}
