using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Tutorial/Skill/Button")]
public class TutorialSkillButton : TutorialStep
{
	UIManagementSkill uiSkill;
	UIManagementSkillInfo uiSkillInfo;

	Transform button;

	public override ITutorial Back()
	{
		return prev.Enter(_quest);
	}

	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		uiSkill = FindObject<UIManagementSkill>();
		uiSkillInfo = uiSkill.UiManagementSkillInfo;

		switch (_quest.GoalType)
		{
			case QuestGoalType.EQUIP_SKILL:
				button = uiSkillInfo.ButtonEquip.transform;
				break;
			case QuestGoalType.LEVELUP_SKILL:
				button = uiSkillInfo.ButtonLevelup.transform;
				break;
			case QuestGoalType.EVOLUTION_SKILL:
				button = uiSkillInfo.ButtonEvolution.transform;
				break;
		}
		TutorialManager.instance.SetPosition(button);
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (button != null && button.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		if (uiSkill.ObjSkillSlotRoot.activeInHierarchy || uiSkill.UiPopupSkillEvolution.gameObject.activeInHierarchy || uiSkill.UiPopupSkillLevelup.gameObject.activeInHierarchy)
		{
			return next == null ? this : next.Enter(_quest);
		}
		return this;
	}
}
