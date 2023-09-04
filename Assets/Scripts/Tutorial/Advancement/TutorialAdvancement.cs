using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Advancement/List")]
public class TutorialAdvancement : TutorialStep
{
	UIManagementAdvancement uiAdvancement;
	UIItemAdvancement item;

	public override ITutorial Enter(QuestInfo quest)
	{
		uiAdvancement = FindObject<UIManagementAdvancement>();
		var tr = uiAdvancement.Find(0);
		item = tr.GetComponent<UIItemAdvancement>();

		TutorialManager.instance.SetPosition(item.ButtonActivate.transform as RectTransform);
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (item != null && item.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return next == null ? this : next.Enter(_quest);
	}
}
