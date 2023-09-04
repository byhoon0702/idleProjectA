using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Tutorial/Pet/Button")]
public class TutorialPetButton : TutorialStep
{
	UIManagementPet uiPet;
	Transform rect = null;

	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		uiPet = FindObject<UIManagementPet>();

		switch (_quest.GoalType)
		{
			case QuestGoalType.EQUIP_PET:
				rect = uiPet.PetInfoUI.ButtonEquip.transform;
				break;
			case QuestGoalType.EVOLUTION_PET:
				rect = uiPet.PetInfoUI.ButtonEvolution.transform;
				break;
			case QuestGoalType.LEVELUP_PET:
				rect = uiPet.PetInfoUI.BtnLevelUp.transform;
				break;
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
		if (uiPet.EquipList.gameObject.activeInHierarchy || uiPet.UiPopupPetEvolution.gameObject.activeInHierarchy || uiPet.UiPopupPetLevelup.gameObject.activeInHierarchy)
		{
			return next == null ? this : next.Enter(_quest);
		}
		return this;
	}
}
