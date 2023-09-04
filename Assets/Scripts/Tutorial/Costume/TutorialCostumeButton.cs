using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Costume/Button")]
public class TutorialCostumeButton : TutorialStep
{
	UICostumeManagement uiCostume;
	Transform tr = null;
	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiCostume = FindObject<UICostumeManagement>();
		tr = uiCostume.UiCostumeInfo.EquipButton.transform;
		if (tr != null)
		{
			TutorialManager.instance.SetPosition(tr);
		}
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if
			(tr != null && tr.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return next == null ? this : next.Enter(_quest);
	}
}
