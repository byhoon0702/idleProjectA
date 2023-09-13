using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Tutorial/Battle/Guardian")]
public class TutorialBattleGuardian : TutorialStep
{
	UIManagementBattle uiBattle;
	RectTransform rect;
	// Start is called before the first frame update
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		uiBattle = FindObject<UIManagementBattle>();
		rect = uiBattle.UiPageBattleGuardian.ButtonPlay.transform as RectTransform;
		TutorialManager.instance.SetPosition(rect);
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (rect != null && rect.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return next == null ? this : next.Enter(_quest);
	}
}
