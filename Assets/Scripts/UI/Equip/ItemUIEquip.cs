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
		imageFrame.sprite = GameUIManager.it.spriteGradeFrameList[(int)info.grade];

		selectListener?.AddSelectListener(OnSelect);

		if (info.itemObject.ItemIcon != null)
		{
			icon.sprite = info.itemObject.ItemIcon;
		}
		else
		{
			icon.sprite = null;
		}

		textInfo.text = "";
		evoltionLevelObject.SetActive(info.BreakthroughLevel > 0);
		evoltionLevelText.text = info.BreakthroughLevel.ToString();
	}

	public void ShowCount(bool isShow)
	{
		if (isShow == false)
		{
			textInfo.text = "";
			return;
		}
		textInfo.text = info.Count.ToString();
	}
	public void ShowLevel(bool isShow)
	{
		if (isShow == false)
		{
			textInfo.text = "";
			return;
		}
		textInfo.text = $"LV.{info.Level}";
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
