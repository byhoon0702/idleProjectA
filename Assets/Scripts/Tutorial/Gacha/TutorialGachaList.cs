using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Gacha/List")]
public class TutorialGachaList : TutorialStep
{
	public GachaType type;
	UIManagementGacha ui;
	UIItemGacha item;
	public override ITutorial Back()
	{
		return prev.Enter(_quest);
	}

	public override ITutorial Enter(QuestInfo quest)
	{
		ui = GameObject.FindObjectOfType<UIManagementGacha>();
		item = ui.Find(type);
		if (item != null)
		{
			TutorialManager.instance.SetPosition(item.ButtonTenGacha.transform as RectTransform);

		}
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (item.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return this;
	}
}
