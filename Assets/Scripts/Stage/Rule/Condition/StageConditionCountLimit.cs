using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Stage Count Condition", menuName = "ScriptableObject/Stage/Condition/CountLimit", order = 1)]
public class StageConditionCountLimit : StageClearCondition
{
	public override bool CheckCondition()
	{
		//둘다 0 이거나 둘중 하나라도 -1(무한) 이면 카운트는 승리 조건이 아니다
		return StageManager.it.currentKillCount >= StageManager.it.CurrentStage.CountLimit;
	}

	public override void SetCondition()
	{
		//UIController.it.UiStageInfo.SetKillCount(true);
	}
	public override void OnUpdate(float time)
	{
		UIController.it.UiStageInfo.RefreshKillCount();
	}
}
