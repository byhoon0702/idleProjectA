
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage HP Condition Fail", menuName = "ScriptableObject/Stage/Condition/HP Fail", order = 1)]
public class StageConditionHPFail : StageFailCondition
{
	public override bool CheckCondition()
	{
		return UnitManager.it.Player.IsAlive() == false;
	}
	public override void SetCondition()
	{

	}
}
