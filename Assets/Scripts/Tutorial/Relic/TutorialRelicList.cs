using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Relic/List")]
public class TutorialRelicList : TutorialStep
{
	UIManagementRelic uiRelic;
	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiRelic = FindObject<UIManagementRelic>();
		uiRelic.Find(_quest.GoalTid);
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (uiRelic != null && uiRelic.gameObject.activeInHierarchy == false)
		{
			return Back();
		}

		return next == null ? this : next.Enter(_quest);
	}

}
