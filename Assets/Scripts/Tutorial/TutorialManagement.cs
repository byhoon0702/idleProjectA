using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Management")]
public class TutorialManagement : TutorialStep
{
	public UIManagement.ViewType type;
	UIManagement management;

	UIContentToggle toggle;
	public override ITutorial Back()
	{
		if (prev == null)
		{
			return this;
		}
		return prev.Enter(_quest);
	}

	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		management = GameObject.FindObjectOfType<UIManagement>(true);
		switch (type)
		{
			case UIManagement.ViewType.Training:
				toggle = management.TrainingButton;
				break;
			case UIManagement.ViewType.Veterancy:
				toggle = management.VeterancyButton;
				break;
			case UIManagement.ViewType.Skill:
				toggle = management.SkillButton;
				break;
			case UIManagement.ViewType.Awakening:
				toggle = management.AwakeningButton;
				break;
			case UIManagement.ViewType.Advancement:
				toggle = management.AdvanceButton;
				break;
			case UIManagement.ViewType.Costume:
				toggle = management.CostumeButton;
				break;
		}

		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (management.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		if (toggle.isOn)
		{
			return next == null ? this : next.Enter(_quest);
		}
		TutorialManager.instance.SetPosition(toggle.transform as RectTransform);
		return this;

	}
}
