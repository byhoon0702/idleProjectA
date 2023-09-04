using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Pet/List")]
public class TutorialPetList : TutorialStep
{
	public int index;
	UIManagementPet uiPet;
	Transform rect;

	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiPet = FindObject<UIManagementPet>();
		rect = uiPet.UiPetGrid.GetChild(index);
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
