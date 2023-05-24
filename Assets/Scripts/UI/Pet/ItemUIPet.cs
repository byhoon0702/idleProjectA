using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using RuntimeData;

public class ItemUIPet : ItemUIBase
{
	[SerializeField] private GameObject evoltionLevelObject;
	[SerializeField] private TextMeshProUGUI evoltionLevelText;

	RuntimeData.PetInfo info;
	ISelectListener selectListener;
	private void OnDestroy()
	{
		selectListener?.RemoveSelectListener(OnSelect);
	}

	public override void OnUpdate(RuntimeData.IDataInfo _petInfo, Action _onClick = null, ISelectListener _selectListener = null)
	{
		info = _petInfo as RuntimeData.PetInfo;
		onClick = _onClick;
		selectListener = _selectListener;
		button.enabled = onClick != null;

		gameObject.SetActive(info != null);

		if (info == null || info.itemObject == null)
		{
			return;
		}

		bg.sprite = GameUIManager.it.spriteGradeList[(int)info.grade];
		selectListener?.AddSelectListener(OnSelect);

		levelText.text = $"LV {info.level}";

		if (info.itemObject.Icon != null)
		{
			icon.sprite = info.itemObject.Icon;
		}
		countText.text = $"{info.count}";
		evoltionLevelText.text = $"{info.evolutionLevel}";
	}

	public void OnSelect(long tid)
	{
		if (tid == info.tid)
		{

		}
	}


	public void OnClick()
	{
		onClick?.Invoke();
	}
}
