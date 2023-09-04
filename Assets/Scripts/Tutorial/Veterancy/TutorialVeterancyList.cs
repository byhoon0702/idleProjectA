using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Veterancy/List")]
public class TutorialVeterancyList : TutorialStep
{
	public StatsType type;
	UIManagementVeterancy uiVeterancy;
	UIItemVeterancy item;
	public override ITutorial Back()
	{
		return prev.Enter(_quest);
	}

	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiVeterancy = GameObject.FindObjectOfType<UIManagementVeterancy>(true);
		item = uiVeterancy.Find(type);
		if (item != null)
		{
			TutorialManager.instance.SetPosition(item.UpgradeButton.transform as RectTransform);
		}
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (uiVeterancy.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return this;
	}
}
