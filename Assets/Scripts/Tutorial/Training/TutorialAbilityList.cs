using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Ability/List")]
public class TutorialAbilityList : TutorialStep
{
	UITraining ui;
	UIItemTraining training;

	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		ui = GameObject.FindObjectOfType<UITraining>(true);

		training = ui.Find(_quest.GoalTid);
		if (training != null)
		{
			TutorialManager.instance.SetPosition(training.UpgradeButton.transform as RectTransform);
		}
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (ui.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return this;
	}
}
