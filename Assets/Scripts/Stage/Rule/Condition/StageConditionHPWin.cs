
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage HP Condition Win", menuName = "ScriptableObject/Stage/Condition/HP Win", order = 1)]
public class StageConditionHPWin : StageClearCondition
{
	public override bool CheckCondition()
	{
		return UnitManager.it.Player.IsAlive() == false;
	}
	public override void SetCondition()
	{

	}
}
