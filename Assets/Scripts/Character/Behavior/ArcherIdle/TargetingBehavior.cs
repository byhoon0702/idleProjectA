﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Targeting Behavior", menuName = "Unit/Behavior/Targeting", order = 2)]
public class TargetingBehavior : UnitBehavior
{
	public WallTargetingBehavior wallTargetingBehavior;
	public OpponentTargetBehavior opponentTargetBehavior;

	public GameObject OnTarget(Character character, Targeting targeting)
	{
		if (targeting == Targeting.WALL)
		{
			return wallTargetingBehavior.OnTarget(character);
		}

		return opponentTargetBehavior.OnTarget(character);
	}
}
