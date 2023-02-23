using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Filter Behavior", menuName = "ScriptableObject/Filter/Player")]
public class TargetPlayerBehaviorSO : TargetFilterBehaviorSO
{

	public override UnitBase[] GetObject()
	{
		return FilterObject<PlayerUnit>();
	}

}
