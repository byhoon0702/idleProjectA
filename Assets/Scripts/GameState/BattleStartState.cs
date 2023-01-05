public class BattleStartState : RootState
{
	public override void OnEnter()
	{
		elapsedTime = 0;
		GameManager.it.battleRecord = new BattleRecord();
		GameManager.it.battleRecord.InitCharacter(CharacterManager.it.GetCharacters(true));
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			GameManager.it.ChangeState(GameState.BATTLE);
		}
	}
}
