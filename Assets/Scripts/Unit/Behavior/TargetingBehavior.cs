using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Targeting Behavior", menuName = "Unit/Behavior/Targeting", order = 2)]
public class TargetingBehavior : UnitBehavior
{
	public WallTargetingBehavior wallTargetingBehavior;
	public OpponentTargetBehavior opponentTargetBehavior;

	public GameObject OnTarget(UnitBase _unit, Targeting targeting, bool ignoreSearchDelay = false)
	{
		if (targeting == Targeting.WALL)
		{
			return wallTargetingBehavior.OnTarget(_unit);
		}

		return opponentTargetBehavior.OnTarget(_unit, ignoreSearchDelay);
	}
}
