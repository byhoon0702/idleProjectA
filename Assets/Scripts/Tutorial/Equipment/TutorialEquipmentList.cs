using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Equipment/List")]
public class TutorialEquipmentList : TutorialStep
{
	public int index;
	private UIManagementEquip equipUi;
	Transform t;
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		equipUi = GameObject.FindObjectOfType<UIManagementEquip>(true);
		t = equipUi.UiEquipGrid.GetChild(index);
		if (t != null)
		{
			TutorialManager.instance.SetPosition(t as RectTransform);
		}
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
		if (t.gameObject.activeInHierarchy == false)
		{
			return this;
		}
		return next.Enter(_quest);
	}
}
