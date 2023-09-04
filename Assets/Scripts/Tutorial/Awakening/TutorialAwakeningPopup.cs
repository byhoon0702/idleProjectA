using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Awakening/Popup")]
public class TutorialAwakeningPopup : TutorialStep
{
	UIManagementAwakening uiAwakening;
	Transform rect;

	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiAwakening = FindObject<UIManagementAwakening>();
		rect = uiAwakening.PopupLevelUP.ButtonLevelUp.transform;
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
		return next == null ? this : next.Enter(_quest);
	}
}
