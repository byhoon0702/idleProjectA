using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Costume/List")]
public class TutorialCostumeList : TutorialStep
{
	public int index;
	UICostumeManagement uiCostume;
	Transform tr;
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		uiCostume = FindObject<UICostumeManagement>();
		tr = uiCostume.Find(index);
		if (tr != null)
		{
			TutorialManager.instance.SetPosition(tr as RectTransform);
		}
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (tr != null && tr.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return next == null ? this : next.Enter(_quest);
	}
}
