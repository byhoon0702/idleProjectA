using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Tutorial/Battle/Guardian")]
public class TutorialBattleGuardian : TutorialStep
{
	UIManagementBattle uiBattle;
	// Start is called before the first frame update
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		uiBattle = FindObject<UIManagementBattle>();
		TutorialManager.instance.SetPosition(uiBattle.UiPageBattleGuardian.ButtonPlay.transform as RectTransform);
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
