using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛에서 처리 하도록 할 것
/// </summary>
public abstract class TargetPriorityBehaviorSO : ScriptableObject
{
	public abstract List<T> FilterObject<T>(Vector3 pos, List<T> targets) where T : UnitBase;
}
