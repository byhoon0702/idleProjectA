using UnityEngine;

[CreateAssetMenu(fileName = "Battle Progress State", menuName = "ScriptableObject/Stage/State/Battle Progress", order = 1)]
public class BattleState : StageFSM
{
	public override FSM OnEnter()
	{
		elapsedTime = 0;
		stageRule.SetCondition();
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

		UIController.it.UiStageInfo.SwitchContentExitButton(StageManager.it.CurrentStage.stageType != StageType.Normal || StageManager.it.bossSpawn);
		UIController.it.UiStageInfo.ShowNormalButtonGroup(StageManager.it.CurrentStage.stageType == StageType.Normal);
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
		//stageRule.OnUpdate(time);
		stageRule.OnLogicUpdate(time);
		if (stageRule.isEnd)
		{
			return nextState != null ? nextState.OnEnter() : this;
		}
		return this;
	}
}
