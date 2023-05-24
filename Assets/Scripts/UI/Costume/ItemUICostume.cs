using TMPro;
using UnityEngine;
using System;

public class ItemUICostume : ItemUIBase
{
	[SerializeField] private GameObject evoltionLevelObject;
	[SerializeField] private TextMeshProUGUI evoltionLevelText;

	RuntimeData.CostumeInfo info;
	ISelectListener selectListener;
	private void OnDestroy()
	{
		selectListener?.RemoveSelectListener(OnSelect);
	}

	public override void OnUpdate(RuntimeData.IDataInfo _equipInfo, Action _onClick = null, ISelectListener _selectListener = null)
	{
		info = _equipInfo as RuntimeData.CostumeInfo;
		onClick = _onClick;
		selectListener = _selectListener;
		button.enabled = onClick != null;

		gameObject.SetActive(info != null);

		if (info == null)
		{
			return;
		}

		selectListener?.AddSelectListener(OnSelect);

		levelText.text = $"LV {info.level}";
		if (info.count == 0)
		{

		}
		else
		{

		}
		if (info.itemObject.Icon != null)
		{
			icon.sprite = info.itemObject.Icon;
		}
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
