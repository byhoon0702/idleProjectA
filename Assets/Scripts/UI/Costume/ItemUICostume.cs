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

		textInfo.text = $"LV {info.Level}";
		if (info.Count == 0)
		{

		}
		else
		{

		}

		if (info.itemObject != null && info.itemObject.ItemIcon != null)
		{
			icon.sprite = info.itemObject.ItemIcon;
		}
		bg.sprite = GameUIManager.it.spriteGradeList[(int)info.grade];
		imageFrame.sprite = GameUIManager.it.spriteGradeFrameList[(int)info.grade];
	}

	public void OnSelect(long tid)
	{
		if (tid == info.Tid)
		{

		}
	}


	public void OnClick()
	{
		onClick?.Invoke();
	}
}
