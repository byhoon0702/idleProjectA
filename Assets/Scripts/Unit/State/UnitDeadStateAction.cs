
using System;
using UnityEngine;


public abstract class UnitDeadStateAction : ScriptableObject
{
	public abstract void OnEnter(UnitBase unitAnimation);
	public abstract void OnUpdate(UnitBase unitAnimation, float delta, float elapsedTime);

}
