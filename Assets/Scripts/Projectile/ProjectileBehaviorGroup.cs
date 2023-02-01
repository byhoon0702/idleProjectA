using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Behavior Group", menuName = "Behaviors/Projectile/Behavior Group", order = 1)]

public class ProjectileBehaviorGroup : ScriptableObject
{
	public ParabolaBehavior parabola;
	public StraightBehavior straight;
	public BezierBehavior bezier;

	public ProjectileBehavior Call(ProjectileType type)
	{
		switch (type)
		{
			case ProjectileType.PARABOLA:
				return parabola;

			case ProjectileType.BEZIER:
				return bezier;
			case ProjectileType.GUIDED:
			default:
				return straight;

		}
	}
}
