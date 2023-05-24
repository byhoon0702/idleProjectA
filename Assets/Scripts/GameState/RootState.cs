using UnityEngine;

public abstract class RootState : FSM
{
	protected float elapsedTime = 0;

	public abstract void Init();

	public abstract FSM OnEnter();
	public abstract void OnExit();

	public abstract void OnUpdate(float time);

	public abstract FSM RunNextState(float time);
}




