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

		return skillRequirement.IsRequirementFulfill(out description);
	}

	public void OnUpdateLock()
	{
		bool isAvailable = IsAvailable(out string description);
		if (skillInfo.unlock == false)
		{
			isAvailable = false;
		}
		if (skillInfo.Level == 0)
		{
			isAvailable = false;
		}
		lockedMark.SetActive(isAvailable == false);
	}
	public void OnUpdate(ISelectListener _parent, RuntimeData.SkillInfo _skillInfo, Action _onAction = null)
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
		isEquipped = skillInfo.isEquipped;
		equippedMark.SetActive(isEquipped);
		OnUpdateLock();

		bool isMax = skillInfo.IsMax();
		if (isMax)
		{
			expSlider.value = 1f;
			expText.text = $"{skillInfo.Count}/MAX";
		}
		else
		{
			int nextCount = skillInfo.LevelUpNeedCount();
			expSlider.value = (float)skillInfo.Count / nextCount;
			expText.text = $"{skillInfo.Count}/{nextCount}";
		}


	}

	public void Refresh()
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


		if (parent != null && skillInfo != null)
		{
			parent.SetSelectedTid(skillInfo.Tid);
		}
		onAction?.Invoke();
	}
	public void ShowSlider(bool show)
	{
		expSliderObject.SetActive(show);
	}
}
