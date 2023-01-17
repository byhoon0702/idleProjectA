using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Wall Targeting Behavior", menuName = "Unit/Behavior/Wall Targeting", order = 2)]
public class WallTargetingBehavior : UnitBehavior
{
	public GameObject OnTarget(Character character)
	{
		GameObject go = GameObject.FindGameObjectWithTag("Wall");
		Character target = go.GetComponent<Character>();

		if (target.IsAlive() == false)
		{
			return null;
		}

		character.SetTarget(target);

		return go;
	}
}
