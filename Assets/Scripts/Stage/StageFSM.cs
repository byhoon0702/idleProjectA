
using UnityEngine;

public enum StageStateType
{
	NONE = 0,
	LOADING,
	MAPLOADING,
	SPAWN,
	STAGECUTSCENE,
	STAGESTART,
	STAGEUPDATE,
	STAGEEND,
	END,
}



public abstract class StageFSM : ScriptableObject, FSM
{
	protected float elapsedTime = 0;
	protected StageRule stageRule;
	protected StageFSM nextState;

	public virtual void Init(StageRule rule, StageFSM nextState)
	{
		stageRule = rule;
		this.nextState = nextState;
	}

	public abstract FSM OnEnter();
	public abstract void OnExit();

	public abstract void OnUpdate(float time);

	public abstract FSM RunNextState(float time);

	public static StageFSM Get(FSM fsm)
	{
		return fsm as StageFSM;
	}
}
