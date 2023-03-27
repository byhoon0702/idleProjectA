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

	RuntimeData.PetInfo petInfo;
	ISelectListener selectListener;
	private void OnDestroy()
	{
		selectListener?.RemoveSelectListener(OnSelect);
	}

	public override void OnUpdate(RuntimeData.IItemInfo _petInfo, Action _onClick = null, ISelectListener _selectListener = null)
	{
		petInfo = _petInfo as RuntimeData.PetInfo;
		onClick = _onClick;
		selectListener = _selectListener;
		button.interactable = onClick != null;

		gameObject.SetActive(petInfo != null);

		if (petInfo == null)
		{
			return;
		}

		selectListener?.AddSelectListener(OnSelect);

		levelText.text = $"LV {petInfo.level}";
		if (petInfo.count == 0)
		{

		}
		else
		{

		}
	}

	public void OnSelect(long tid)
	{
		if (tid == petInfo.tid)
		{

		}
	}


	public void OnClick()
	{
		onClick?.Invoke();
	}
}
