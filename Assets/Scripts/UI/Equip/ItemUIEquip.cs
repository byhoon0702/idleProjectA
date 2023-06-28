using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ItemUIEquip : ItemUIBase
{
	[SerializeField] private GameObject evoltionLevelObject;
	[SerializeField] private TextMeshProUGUI evoltionLevelText;
	[SerializeField] private Toggle[] toggleStarLevel;

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



		if (info == null)
		{
			return;
		}
		bg.sprite = GameUIManager.it.spriteGradeList[(int)info.grade];
		selectListener?.AddSelectListener(OnSelect);

		levelText.text = $"{info.grade.ToString()}";

		if (info.itemObject.Icon != null)
		{
			icon.sprite = info.itemObject.Icon;
		}
		else
		{
			icon.sprite = null;
		}

		countText.text = "";
	}

	public void ShowCount(bool isShow)
	{
		if (isShow == false)
		{
			countText.text = "";
			return;
		}
		countText.text = info.count.ToString();
	}
	public void ShowLevel(bool isShow)
	{
		if (isShow == false)
		{
			countText.text = "";
			return;
		}
		countText.text = $"LV.{info.level}";
	}

	public void ShowStars(bool istrue)
	{
		for (int i = 0; i < toggleStarLevel.Length; i++)
		{
			toggleStarLevel[i].gameObject.SetActive(istrue);
			toggleStarLevel[i].isOn = i < info.star;
		}
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
