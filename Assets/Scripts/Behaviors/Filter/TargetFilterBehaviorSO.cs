using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetFilterBehaviorSO : ScriptableObject
{
	public abstract UnitBase[] GetObject();
	protected virtual T[] FilterObject<T>() where T : UnitBase
	{
		T[] chara = GameObject.FindObjectsOfType<T>();
		return chara;
	}

}
