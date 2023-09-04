using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;
[CreateAssetMenu(menuName = "Tutorial/Skill/Popup")]
public class TutorialSkillPopup : TutorialStep
{
	Transform rect = null;

	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		var uiSkill = FindObject<UIManagementSkill>();
		if (uiSkill.UiPopupSkillLevelup.gameObject.activeInHierarchy)
		{
			rect = uiSkill.UiPopupSkillLevelup.ButtonUpgrade.transform;
		}
		else if (uiSkill.UiPopupSkillEvolution.gameObject.activeInHierarchy)
		{
			rect = uiSkill.UiPopupSkillEvolution.ButtonUpgrade.transform;
		}

		TutorialManager.instance.SetPosition(rect as RectTransform);
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (rect != null && rect.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return this;
	}
}
