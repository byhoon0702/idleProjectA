using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Tutorial/Battle/Page")]
public class TutorialBattlePage : TutorialStep
{
	UIManagementBattle uiBattle;
	Transform rect = null;
	Toggle toggle;
	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiBattle = FindObject<UIManagementBattle>();
		switch (quest.GoalType)
		{
			case QuestGoalType.DUNGEON_CLEAR:
				toggle = uiBattle.DungeonButton;

				break;
			case QuestGoalType.TOWER_CLEAR:
				toggle = uiBattle.TowerButton;
				break;
			case QuestGoalType.GUARDIAN_CLEAR:
				toggle = uiBattle.GuardianButton;

				break;
		}
		rect = toggle.transform;
		TutorialManager.instance.SetPosition(rect as RectTransform);

		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (rect != null && rect.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		if (toggle.isOn)
		{
			return next == null ? this : next.Enter(_quest);
		}
		return this;

	}
}
