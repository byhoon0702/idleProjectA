
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage HP Condition", menuName = "ScriptableObject/Stage/Condition/HP", order = 1)]
public class StageConditionHP : StageFailCondition
{
	public override bool CheckCondition()
	{
		return UnitManager.it.Player.IsAlive() == false;
	}
	public override void SetCondition()
	{

	}
}
