using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class UISkillSlot : MonoBehaviour
{
	[SerializeField] private ItemUISkill itemUI;
	[SerializeField] private Button slotButton;

	[SerializeField] protected GameObject expSliderObject;
	[SerializeField] protected Slider expSlider;
	[SerializeField] protected TextMeshProUGUI expText;

	[SerializeField] private GameObject equippedMark;
	[SerializeField] private GameObject lockedMark;
	private ISelectListener parent;
	private RuntimeData.SkillInfo skillInfo;

	private RequirementInfo skillRequirement;
	public bool isEquipped { get; private set; }
	Action onAction;
	private void OnDestroy()
	{
		if (parent != null)
		{
			parent.RemoveSelectListener(OnSelect);
		}
	}

	public bool IsAvailable(out string description)
	{
		description = "";
		if (skillRequirement == null)
		{

			return true;
		}


		switch (skillRequirement.type)
		{
			case RequirementType.STAGE:
				{
					long dungeonTid = skillRequirement.parameter1;
					int stageNumber = skillRequirement.parameter2;

					var stage = GameManager.UserDB.stageContainer.GetStage(dungeonTid, stageNumber);

					description = $"{dungeonTid}_{stageNumber} 클리어";
					if (stage == null)
					{
						return false;
					}

					return stage.isClear;
				}
			case RequirementType.USERLEVEL:
				{
					int requiredLevel = Mathf.FloorToInt(skillRequirement.parameter1);
					var userlevel = GameManager.UserDB.userInfoContainer.userInfo.UserLevel;
					description = $"{requiredLevel} 이상 해제";
					return userlevel >= requiredLevel;
				}

			case RequirementType.BASESKILL:
				{
					long tid = skillRequirement.parameter1;
					int baseSkillLevel = skillRequirement.parameter2;
					var baseSkillInfo = GameManager.UserDB.skillContainer.Get(tid);

					description = $"{baseSkillInfo.Name} {baseSkillLevel} 필요";

					if (baseSkillInfo == null)
					{
						return false;
					}

					return baseSkillInfo.level >= baseSkillLevel;
				}

			case RequirementType.NONE:
				return true;
		}
		return false;
	}

	public void OnUpdateLock()
	{
		bool isAvailable = IsAvailable(out string description);
		lockedMark.SetActive(isAvailable == false);
	}
	public void OnUpdate(ISelectListener _parent, RuntimeData.SkillInfo _skillInfo, Action _onAction)
	{
		parent = _parent;
		skillInfo = _skillInfo;
		onAction = _onAction;

		itemUI.OnUpdate(skillInfo);
		parent?.AddSelectListener(OnSelect);

		slotButton.enabled = onAction != null;
		lockedMark.SetActive(false);
		equippedMark.SetActive(false);
		ShowSlider(false);
		if (skillInfo == null)
		{
			return;
		}

		//skillRequirement = DataManager.Get<SkillTreeDataSheet>().GetSkillRequirement(skillInfo.rawData.detailData.rootSkillTid, skillInfo.Tid);

		for (int i = 0; i < GameManager.UserDB.skillContainer.skillSlot.Length; i++)
		{
			var data = GameManager.UserDB.skillContainer.skillSlot[i];
			isEquipped = false;
			if (data.itemTid == skillInfo.Tid)
			{
				isEquipped = true;
				equippedMark.SetActive(true);
				break;
			}
		}

		int nextCount = 10;// skillInfo.LevelUpNeedCount();
		expSlider.value = (float)skillInfo.count / nextCount;
		expText.text = $"{skillInfo.count}/{nextCount}";
	}

	public void OnRefresh()
	{

	}

	public void OnSelect(long tid)
	{
		if (skillInfo == null)
		{
			return;
		}
		if (tid == skillInfo.Tid)
		{

		}
	}
	public void OnClickSelect()
	{
		parent?.SetSelectedTid(skillInfo.Tid);
		onAction?.Invoke();
	}
	public void ShowSlider(bool show)
	{
		expSliderObject.SetActive(show);
	}
}
