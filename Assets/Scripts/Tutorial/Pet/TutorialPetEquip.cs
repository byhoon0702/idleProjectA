using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tutorial/Pet/Equip")]
public class TutorialPetEquip : TutorialStep
{
	UIManagementPet uiPet;
	Transform rect;
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		uiPet = FindObject<UIManagementPet>();

		rect = uiPet.PetSlots[0].transform;

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
