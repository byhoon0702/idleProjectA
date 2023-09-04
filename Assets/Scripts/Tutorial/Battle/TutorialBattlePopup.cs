using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;
[CreateAssetMenu(menuName = "Tutorial/Battle/Popup")]
public class TutorialBattlePopup : TutorialStep
{
	UIManagementBattle uiBattle;
	Transform enter;
	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiBattle = FindObject<UIManagementBattle>();
		if (uiBattle.UiDungeonPopup.gameObject.activeInHierarchy)
		{
			enter = uiBattle.UiDungeonPopup.ButtonEnter.transform;
		}
		else if (uiBattle.UiDungeonStagePopup.gameObject.activeInHierarchy)
		{
			enter = uiBattle.UiDungeonStagePopup.ButtonEnter.transform;
		}

		TutorialManager.instance.SetPosition(enter);
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (enter != null && enter.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return next == null ? this : next.Enter(_quest);
	}
}
