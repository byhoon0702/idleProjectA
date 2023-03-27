using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitFSM : ScriptableObject, FiniteStateMachine
{
	protected Unit owner;

	public abstract void OnEnter();
	public abstract void OnEnter<T>(T data);
	public abstract void OnExit();
	public abstract void OnUpdate(float time);

}
