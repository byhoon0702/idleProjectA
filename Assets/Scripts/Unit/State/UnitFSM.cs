using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitFSM : ScriptableObject, FSM
{
	[SerializeField] protected StateType state;
	public StateType State => state;
	protected Unit owner;
	public virtual void Init(Unit _owner)
	{
		owner = _owner;
	}
	public abstract FSM OnEnter();
	public abstract void OnExit();
	public abstract void OnUpdate(float time);
	public abstract void OnFixedUpdate(float fixedTime);

	public virtual FSM RunNextState(float time)
	{
		return this;
	}


}
