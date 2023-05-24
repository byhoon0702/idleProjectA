using TMPro;
using UnityEngine;
using System;

public class ItemUIEquip : ItemUIBase
{
	[SerializeField] private GameObject evoltionLevelObject;
	[SerializeField] private TextMeshProUGUI evoltionLevelText;

	RuntimeData.EquipItemInfo info;
	ISelectListener selectListener;
	private void OnDestroy()
	{
		selectListener?.RemoveSelectListener(OnSelect);
	}

	public override void OnUpdate(RuntimeData.IDataInfo _equipInfo, Action _onClick = null, ISelectListener _selectListener = null)
	{
		info = _equipInfo as RuntimeData.EquipItemInfo;
		onClick = _onClick;
		selectListener = _selectListener;
		button.enabled = onClick != null;

		gameObject.SetActive(info != null);


		bg.sprite = GameUIManager.it.spriteGradeList[(int)info.grade];

		if (info == null)
		{
			return;
		}

		selectListener?.AddSelectListener(OnSelect);

		levelText.text = $"LV {info.level}";

		if (info.itemObject.Icon != null)
		{
			icon.sprite = info.itemObject.Icon;
		}

		countText.text = info.count.ToString();
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
