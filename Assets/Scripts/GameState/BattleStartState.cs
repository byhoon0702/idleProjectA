using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Battle Start State", menuName = "ScriptableObject/Stage/State/Battle Start", order = 1)]
public class BattleStartState : StageFSM
{

	public override FSM OnEnter()
	{
		elapsedTime = 0;
		GameManager.it.battleRecord = new BattleRecord();

		//UnitGlobal.it.WaveStart();
		return this;
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		//elapsedTime += time;
		//if (elapsedTime > 0.5f)
		//{
		//	VGameManager.it.ChangeState(GameState.BATTLE);
		//}
	}
	public override FSM RunNextState(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 0.5f)
		{
			return nextState != null ? nextState.OnEnter() : this;
		}

		return this;
	}
}
