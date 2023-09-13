using UnityEngine;

[CreateAssetMenu(fileName = "Battle Progress State", menuName = "ScriptableObject/Stage/State/Battle Progress", order = 1)]
public class BattleState : StageFSM
{
	public override FSM OnEnter()
	{
		elapsedTime = 0;
		stageRule.SetCondition();
		UnitManager.it.Player.ChangeState(StateType.IDLE, true);
		if (StageManager.it.bossSpawn)
		{
			UIController.it.UiStageInfo.ShowBossName();
			UIController.it.UiStageInfo.SwitchBossMode(true);
		}
		else
		{
			UIController.it.UiStageInfo.ShowStageName();
			UIController.it.UiStageInfo.SwitchBossMode(false);
		}

		bool isNormal = StageManager.it.CheckNormalStage();
		UIController.it.UiStageInfo.SwitchContentExitButton(!isNormal || StageManager.it.bossSpawn);
		UIController.it.UiStageInfo.ShowNormalButtonGroup(isNormal);

		StageManager.it.stagePlayTick = Time.realtimeSinceStartupAsDouble;

		return this;
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{


	}

	public override FSM RunNextState(float time)
	{
		StageManager.it.StagePlayTime();

		stageRule.OnLogicUpdate(time);
		if (stageRule.isEnd)
		{
			return nextState != null ? nextState.OnEnter() : this;
		}
		return this;
	}
}
