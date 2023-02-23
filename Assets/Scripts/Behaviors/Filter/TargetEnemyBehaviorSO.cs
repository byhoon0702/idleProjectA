using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Enemy Filter Behavior", menuName = "ScriptableObject/Filter/Enemy")]
public class TargetEnemyBehaviorSO : TargetFilterBehaviorSO
{
	public override UnitBase[] GetObject()
	{
		return FilterObject<EnemyUnit>();
	}

}
