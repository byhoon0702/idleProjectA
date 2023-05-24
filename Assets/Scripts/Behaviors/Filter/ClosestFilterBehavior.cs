using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Closest Filter Behavior", menuName = "ScriptableObject/Filter/Closest")]
public class ClosestFilterBehavior : TargetPriorityBehaviorSO
{
	public override List<T> FilterObject<T>(Vector3 pos, List<T> targets)
	{

		targets.Sort((x, y) =>
		{
			float distanceA = (pos - x.transform.position).sqrMagnitude;
			float distanceB = (pos - y.transform.position).sqrMagnitude;

			return distanceA.CompareTo(distanceB);

		});
		return targets;
	}

}
