using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Closest Filter Behavior", menuName = "ScriptableObject/Filter/Closest")]
public class ClosestFilterBehavior : TargetPriorityBehaviorSO
{
	public override List<T> FilterObject<T>(Vector3 pos, List<T> targets)
	{

		targets.Sort((x, y) => { return (pos.x - y.transform.position.x).CompareTo((pos.x - x.transform.position.x)); });
		return targets;
	}

}
