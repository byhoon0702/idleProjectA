using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;
[CreateAssetMenu(menuName = "Tutorial/Skill/Equip")]
public class TutorialSkillEquip : TutorialStep
{
	UIManagementSkill uiSkill;

	Transform button;

	public override ITutorial Back()
	{
		return prev.Enter(_quest);
	}

	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiSkill = FindObject<UIManagementSkill>();
		button = uiSkill.UiSkillSlots[0].transform;
		TutorialManager.instance.SetPosition(button as RectTransform);
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (button != null && button.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return next == null ? this : next.Enter(_quest);
	}
}
