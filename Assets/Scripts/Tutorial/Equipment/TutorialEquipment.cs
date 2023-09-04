using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEquipment : TutorialStep
{
	public EquipTabType tabType;
	private UIManagementEquip equipUi;
	private Toggle _toggle;
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		equipUi = GameObject.FindObjectOfType<UIManagementEquip>(true);

		switch (tabType)
		{
			case EquipTabType.WEAPON:
				_toggle = equipUi.WeaponTab;
				break;
			case EquipTabType.ARMOR:
				_toggle = equipUi.ArmorTab;
				break;
			case EquipTabType.RING:
				_toggle = equipUi.RingTab;
				break;
			case EquipTabType.NECKLACE:
				_toggle = equipUi.NecklaceTab;
				break;
		}

		TutorialManager.instance.SetPosition(_toggle.transform as RectTransform);
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
		if (_toggle.isOn)
		{
			return next != null ? next : this;
		}
		return this;
	}
}
