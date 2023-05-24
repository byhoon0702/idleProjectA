using UnityEngine;

[CreateAssetMenu(fileName = "Cut Scene State", menuName = "ScriptableObject/Stage/State/CutScene", order = 1)]
public class CutSceneState : StageFSM
{
	public bool isCutsceneEnd = false;
	public override FSM OnEnter()
	{
		elapsedTime = 0;
		isCutsceneEnd = false;
		if (stageRule.isCutsceneExist == false)
		{
			return nextState.OnEnter();
		}

		GameManager.GameStop = true;
		UnitManager.it.Player.ChangeState(StateType.IDLE, true);
		DialogueManager.it.ChangeInkJson(Resources.Load("Story/StageCustscene") as TextAsset);
		DialogueManager.it.EnterDialogueMode(() => { StageManager.it.playableDirector.Play(); });
		StageManager.it.playableDirector.Play();

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
		isCutsceneEnd = GameManager.GameStop == false;
		if (isCutsceneEnd)
		{
			return nextState != null ? nextState.OnEnter() : this;
		}
		return this;
	}


}
