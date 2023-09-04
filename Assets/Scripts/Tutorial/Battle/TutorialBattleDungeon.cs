using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Battle/Dungeon")]
public class TutorialBattleDungeon : TutorialStep
{
	UIManagementBattle uiBattle;
	Transform rect;
	// Start is called before the first frame update
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		uiBattle = FindObject<UIManagementBattle>();
		rect = uiBattle.UiPageBattleDungeon.transform;
		var item = uiBattle.UiPageBattleDungeon.Find(_quest.GoalTid);
		if (item != null)
		{
			TutorialManager.instance.SetPosition(item.transform);
		}
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (rect != null && rect.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		if (uiBattle.UiDungeonPopup.gameObject.activeInHierarchy || uiBattle.UiDungeonStagePopup.gameObject.activeInHierarchy)
		{
			return next == null ? this : next.Enter(_quest);
		}
		return this;

	}
}
