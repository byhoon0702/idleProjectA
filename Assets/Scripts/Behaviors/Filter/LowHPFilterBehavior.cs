using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowHPFilterBehavior : TargetPriorityBehaviorSO
{
	public override List<T> FilterObject<T>(Vector3 pos, List<T> filter)
	{

		return filter;
	}
}
