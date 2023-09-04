using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Tutorial/Costume/Page")]
public class TutorialCostumePage : TutorialStep
{
	public CostumeType type;
	UICostumeManagement uiCostume;
	Transform tr;
	Toggle toggle;
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		uiCostume = FindObject<UICostumeManagement>();

		switch (type)
		{
			case CostumeType.CHARACTER:
				toggle = uiCostume.ClothTab;

				break;
			case CostumeType.HYPER:
				toggle = uiCostume.AwakeningTab;
				break;
		}

		tr = toggle.transform;

		TutorialManager.instance.SetPosition(tr as RectTransform);
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (uiCostume != null && uiCostume.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		if (toggle != null && toggle.isOn)
		{
			return next == null ? this : next.Enter(_quest);
		}
		return this;
	}
}
