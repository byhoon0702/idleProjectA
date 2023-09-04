using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Tutorial/Battle/Tower")]

public class TutorialBattleTower : TutorialStep
{
	UIManagementBattle uiBattle;
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		uiBattle = FindObject<UIManagementBattle>();
		TutorialManager.instance.SetPosition(uiBattle.UiPageBattleTower.ButtonPlay.transform as RectTransform);

		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (uiBattle.UiPageBattleTower.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return next == null ? this : next.Enter(_quest);
	}
}
