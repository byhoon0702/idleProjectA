using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Dummy Filter Behavior", menuName = "ScriptableObject/Filter/Dummy")]
public class TargetDummyBehaviorSO : TargetFilterBehaviorSO
{
	public override UnitBase[] GetObject()
	{
		return FilterObject<DummyUnit>();
	}

}
