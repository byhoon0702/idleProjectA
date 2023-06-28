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
		UnitManager.it.Player.position = StageManager.it.map.playerSpawnPos != null ? StageManager.it.map.playerSpawnPos.position : new Vector3(-2, 0, 0);
		UnitManager.it.Player.ChangeDirection(Vector3.right);

		DialogueManager.it.ChangeInkJson(Resources.Load("Story/StageCustscene") as TextAsset);
		DialogueManager.it.EnterDialogueMode(() => { SceneCamera.PlayableDirector.Play(); });

		//UnitManager.it.Player
		SceneCamera.it.PlayBossEnter(null);

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
