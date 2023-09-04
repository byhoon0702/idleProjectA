using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stage Time Condition Win", menuName = "ScriptableObject/Stage/Condition/TimeLimit Win", order = 1)]
public class StageConditionTimeLimitWin : StageClearCondition
{
	public float leftTime;
	public float maxTime;
	public override bool CheckCondition()
	{
		return leftTime <= 0;
	}

	public override void OnUpdate(float time)
	{
		leftTime -= time;
		UIController.it.UiStageInfo.UpdateTimer(leftTime, maxTime);
	}

	public override void SetCondition()
	{
		leftTime = StageManager.it.CurrentStage.TimeLimit;
		maxTime = StageManager.it.CurrentStage.TimeLimit;
		UIController.it.UiStageInfo.SetTimer(leftTime);
	}
}
