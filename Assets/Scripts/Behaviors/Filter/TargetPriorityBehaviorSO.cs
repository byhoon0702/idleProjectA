using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetPriorityBehaviorSO : ScriptableObject
{
	public abstract List<T> FilterObject<T>(Vector3 pos, List<T> targets) where T : UnitBase;
}
