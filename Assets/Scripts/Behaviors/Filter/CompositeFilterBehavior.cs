using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Composite Filter Behavior", menuName = "ScriptableObject/Filter/Composite")]
public class CompositeFilterBehavior : TargetPriorityBehaviorSO
{
	public TargetFilterBehaviorSO[] behaviors;
	public override List<T> FilterObject<T>(Vector3 pos, List<T> obj)
	{
		List<T> filter = new List<T>();

		for (int i = 0; i < behaviors.Length; i++)
		{
			//filter = behaviors[i].FilterObject(filter);
		}

		return filter;
	}
}
