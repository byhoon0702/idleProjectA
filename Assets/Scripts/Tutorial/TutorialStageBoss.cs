using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Stage/Boss")]
public class TutorialStageBoss : TutorialStep
{
	UIStageInfo uiStageInfo;
	Transform rect = null;
	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiStageInfo = FindObject<UIStageInfo>();
		rect = uiStageInfo.BtnPlayBoss.transform;

		TutorialManager.instance.SetPosition(rect);
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (StageManager.it.playBoss)
		{
			return next == null ? this : next.Enter(_quest);
		}

		return this;
	}
}
