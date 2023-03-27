using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkillSlot : MonoBehaviour
{
	[SerializeField] private ItemUISkill itemUI;

	private UIManagementSkill parent;
	private RuntimeData.SkillInfo skillInfo;

	private void Awake()
	{

	}

	public void OnUpdate(UIManagementSkill _parent, RuntimeData.SkillInfo _skillInfo)
	{
		parent = _parent;
		skillInfo = _skillInfo;

		itemUI.OnUpdate(skillInfo);

		OnRefresh();
	}

	public void OnRefresh()
	{

	}

	private void OnClickPlus()
	{
		parent.EquipSkill(skillInfo.tid);
	}

	private void OnClickMinus()
	{
		parent.UnEquipSkill(skillInfo.tid);
	}

	private void OnClickSkillInfo()
	{
		parent.ShowItemInfo(skillInfo);
	}
}
