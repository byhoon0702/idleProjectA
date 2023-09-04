using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Equipment/Buttons")]
public class TutorialEquipmentButtons : TutorialStep
{
	private UIManagementEquip equipUI;
	private UIManagementEquipInfo equipItemInfo;
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;

		var uiBase = GameObject.FindObjectOfType<UIManagementEquipInfo>(true);
		equipItemInfo = uiBase;
		equipUI = uiBase.Parent;

		if (uiBase == null)
		{
			return this;
		}

		Transform rect = null;
		switch (_quest.GoalType)
		{
			case QuestGoalType.EQUIP_ITEM:
				rect = uiBase.EquipButton.transform;
				break;
			case QuestGoalType.BREAKTHROUGH_WEAPON:
				rect = uiBase.BreakthroughButton.transform;

				break;
			case QuestGoalType.LEVELUP_WEAPON:
				rect = uiBase.LevelupButton.transform;

				break;
		}
		TutorialManager.instance.SetPosition(rect as RectTransform);
		return this;
	}
	public override ITutorial Back()
	{
		if (prev == null)
		{
			return this;
		}
		return prev;
	}
	public override ITutorial OnUpdate(float time)
	{
		if (equipItemInfo.SelectedInfo.unlock == false)
		{
			return Back();
		}
		if (equipUI.UiPopupEquipBreakthrough.isActiveAndEnabled || equipUI.UiPopupEquipLevelup.isActiveAndEnabled || equipUI.UiPopupEquipUpgrade.isActiveAndEnabled)
		{
			return next == null ? this : next.Enter(_quest);
		}
		return this;
	}
}
