using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UnitBehavior : ScriptableObject
{
	public bool useDash;
	public float dashDistance;
	public float dashDeadzone;
	public abstract void OnPreUpdate(Unit unit, float deltatime);
	public abstract void OnUpdate(Unit unit, float deltatime);
	public abstract void OnPostUpdate(Unit unit, float deltatime);
}
