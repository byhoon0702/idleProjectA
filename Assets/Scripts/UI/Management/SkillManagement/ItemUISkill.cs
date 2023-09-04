using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using RuntimeData;

public class ItemUISkill : ItemUIBase
{
	[SerializeField] private GameObject objEvolutionLevel;
	[SerializeField] private TextMeshProUGUI textEvolutionLevel;
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
		bg.sprite = GameUIManager.it.spriteGradeList[(int)info.grade];
		imageFrame.sprite = GameUIManager.it.spriteGradeFrameList[(int)info.grade];

		objEvolutionLevel.SetActive(info.EvolutionLevel > 0);

		textEvolutionLevel.text = info.EvolutionLevel.ToString();


		textInfo.text = $"LV.{info.Level}";
	}

	public void OnSelect(long tid)
	{

	}

	public void OnClick()
	{
		onClick?.Invoke();
	}


}
