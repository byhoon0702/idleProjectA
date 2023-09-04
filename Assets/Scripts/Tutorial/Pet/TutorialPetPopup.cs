using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Pet/Popup")]
public class TutorialPetPopup : TutorialStep
{
	UIManagementPet uiPet;
	Transform rect;
	public override ITutorial Back()
	{
		return prev == null ? this : prev.Enter(_quest);
	}

	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		uiPet = FindObject<UIManagementPet>();
		if (uiPet.UiPopupPetLevelup.gameObject.activeInHierarchy)
		{
			rect = uiPet.UiPopupPetLevelup.ButtonUpgrade.transform;
		}
		else if (uiPet.UiPopupPetEvolution.gameObject.activeInHierarchy)
		{
			rect = uiPet.UiPopupPetEvolution.ButtonUpgrade.transform;
		}

		TutorialManager.instance.SetPosition(rect as RectTransform);
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
