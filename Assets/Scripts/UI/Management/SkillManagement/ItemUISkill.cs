using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemUISkill : ItemUIBase
{
	private RuntimeData.SkillInfo info;
	private ISelectListener selectListener;
	public override void OnUpdate(RuntimeData.IItemInfo _skillInfo, Action _onClick = null, ISelectListener _selectListener = null)
	{
		info = _skillInfo as RuntimeData.SkillInfo;
		onClick = _onClick;
		button.interactable = onClick != null;
		selectListener = _selectListener;
		gameObject.SetActive(info != null);
	}

	public void OnSelect(long tid)
	{

	}

	public void OnClick()
	{
		onClick?.Invoke();
	}


}
