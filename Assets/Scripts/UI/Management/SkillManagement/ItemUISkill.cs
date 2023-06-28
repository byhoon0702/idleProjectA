using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using RuntimeData;

public class ItemUISkill : ItemUIBase
{
	private RuntimeData.SkillInfo info;
	private ISelectListener selectListener;
	public override void OnUpdate(RuntimeData.IDataInfo _skillInfo, Action _onClick = null, ISelectListener _selectListener = null)
	{
		info = _skillInfo as RuntimeData.SkillInfo;
		onClick = _onClick;
		button.enabled = onClick != null;
		selectListener = _selectListener;
		gameObject.SetActive(info != null);


		if (info == null || info.Tid == 0)
		{
			return;
		}
		if (info.itemObject.Icon != null)
		{
			icon.sprite = info.itemObject.Icon;
		}


		levelText.text = $"{info.grade}";
		countText.text = $"LV.{info.level}";
	}

	public void OnSelect(long tid)
	{

	}

	public void OnClick()
	{
		onClick?.Invoke();
	}


}
