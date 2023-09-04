using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Skill/List")]
public class TutorialSkillList : TutorialStep
{
	public int index;

	UIManagementSkill uiSkill;
	UISkillSlot item;
	public override ITutorial Back()
	{
		return prev.Enter(_quest);
	}

	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiSkill = FindObject<UIManagementSkill>();
		var tr = uiSkill.Find(index);
		if (tr != null)
		{
			item = tr.GetComponent<UISkillSlot>();
		}
		if (item != null)
		{
			TutorialManager.instance.SetPosition(item.transform as RectTransform);
		}
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (uiSkill.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		if (item != null)
		{
			return next == null ? this : next.Enter(_quest);
		}
		return this;
	}

}
