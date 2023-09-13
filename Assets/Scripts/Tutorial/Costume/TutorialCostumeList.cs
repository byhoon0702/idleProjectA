using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Costume/List")]
public class TutorialCostumeList : TutorialStep
{
	public int index;
	UICostumeManagement uiCostume;
	UICostumeSlot tr;
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		uiCostume = FindObject<UICostumeManagement>();
		tr = uiCostume.Find(index);
		if (tr != null)
		{
			TutorialManager.instance.SetPosition(tr.transform as RectTransform);
		}
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if ((_quest.GoalType == QuestGoalType.EQUIP_COSTUME && uiCostume.costumeType != CostumeType.CHARACTER) || _quest.GoalType == QuestGoalType.EQUIP_COSTUME_HYPER && uiCostume.costumeType != CostumeType.HYPER)
		{
			return Back();
		}

		if (tr != null && tr.CostumeInfo.Tid == uiCostume.selectedItemTid)
		{
			return next == null ? this : next.Enter(_quest);
		}
		return this;
	}
}
