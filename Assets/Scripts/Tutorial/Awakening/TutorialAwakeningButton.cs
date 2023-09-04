using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Awakening/Button")]
public class TutorialAwakeningButton : TutorialStep
{
	UIManagementAwakening uiAwakening;
	Transform rect;

	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiAwakening = FindObject<UIManagementAwakening>();
		switch (_quest.GoalType)
		{
			case QuestGoalType.LEVELUP_AWAKENRUNE:
				rect = uiAwakening.Runes[0].transform;
				break;
			case QuestGoalType.LEVELUP_AWAKENING:
				rect = uiAwakening.ButtonUpgrade.transform;
				break;
		}


		if (rect != null)
		{
			TutorialManager.instance.SetPosition(rect as RectTransform);
		}
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (rect != null && rect.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		if (uiAwakening.PopupLevelUP.gameObject.activeInHierarchy)
		{

			return next == null ? this : next.Enter(_quest);
		}
		return this;
	}
}
