using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Wall Targeting Behavior", menuName = "ScriptableObject/Unit/Wall Targeting", order = 2)]
public class WallTargetingBehavior : UnitBehavior
{
	public GameObject OnTarget(UnitBase _unit)
	{
		GameObject go = GameObject.FindGameObjectWithTag("Wall");
		if (go == null)
		{
			return null;
		}
		Unit target = go.GetComponent<Unit>();

		if (target.IsAlive() == false)
		{
			return null;
		}

		_unit.SetTarget(target);

		return go;
	}
}
