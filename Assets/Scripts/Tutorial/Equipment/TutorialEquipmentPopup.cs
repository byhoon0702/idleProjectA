using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tutorial/Equipment/Popup")]
public class TutorialEquipmentPopup : TutorialStep
{
	Transform rect = null;
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;

		var equipUi = GameObject.FindObjectOfType<UIManagementEquip>(true);
		if (equipUi.UiPopupEquipBreakthrough.gameObject.activeInHierarchy)
		{
			rect = equipUi.UiPopupEquipBreakthrough.EconomyButton.transform;
		}
		else if (equipUi.UiPopupEquipLevelup.gameObject.activeInHierarchy)
		{
			rect = equipUi.UiPopupEquipLevelup.ButtonUpgrade.transform;
		}
		else if (equipUi.UiPopupEquipUpgrade.gameObject.activeInHierarchy)
		{
			rect = equipUi.UiPopupEquipUpgrade.ButtonUpgrade.transform;
		}

		TutorialManager.instance.SetPosition(rect as RectTransform);

		return this;
	}

	public override ITutorial Back()
	{
		if (prev == null)
		{
			return this;
		}
		return prev.Enter(_quest);
	}

	public override ITutorial OnUpdate(float time)
	{
		if (rect != null && rect.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return this;
	}
}
