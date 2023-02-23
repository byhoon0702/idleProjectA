using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Opponent Targeting Behavior", menuName = "Unit/Behavior/Opponent Targeting", order = 2)]
public class OpponentTargetBehavior : UnitBehavior
{
	public GameObject OnTarget(UnitBase _unit, bool ignoreSearchDelay)
	{
		//	if (_unit is PlayerUnit || _unit is Pet)
		//	{
		//		_unit.FindTarget(Time.deltaTime, UnitManager.it.GetEnemyUnit(), ignoreSearchDelay);
		//	}
		//	else
		//	{
		//		_unit.FindTarget(Time.deltaTime, UnitManager.it.PlayerToList, ignoreSearchDelay);
		//	}

		return null;
	}
}
