using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Wall Targeting Behavior", menuName = "Behaviors/Unit/Wall Targeting", order = 2)]
public class WallTargetingBehavior : UnitBehavior
{
	public GameObject OnTarget(UnitBase character)
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

		character.SetTarget(target);

		return go;
	}
}
