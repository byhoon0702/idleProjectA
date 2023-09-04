using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;




public class UICostumeSlot : MonoBehaviour
{
	[SerializeField] private ItemUICostume itemUI;
	[SerializeField] private Button slotButton;

	//[SerializeField] protected GameObject expSliderObject;
	//[SerializeField] protected Slider expSlider;
	//[SerializeField] protected TextMeshProUGUI expText;


	[SerializeField] private GameObject equippedMark;
	[SerializeField] private GameObject lockedMark;

	private ISelectListener parent;
	private RuntimeData.CostumeInfo costumeInfo;
	public bool isEquipped { get; private set; }
	Action action;


	private void OnDestroy()
	{
		if (parent != null)
		{
			parent.RemoveSelectListener(OnSelect);
		}
	}

	public void OnUpdate(ISelectListener _parent, RuntimeData.CostumeInfo _costumeInfo, Action _action = null)
	{
		parent = _parent;
		costumeInfo = _costumeInfo;
		action = _action;

		parent?.AddSelectListener(OnSelect);

		slotButton.enabled = action != null;
		itemUI.OnUpdate(costumeInfo);

		lockedMark.SetActive(false);
		equippedMark.SetActive(false);
		ShowSlider(false);
		if (costumeInfo == null)
		{
			return;
		}

		lockedMark.SetActive(costumeInfo.unlock == false);

		var data = PlatformManager.UserDB.costumeContainer[costumeInfo.Type];
		isEquipped = false;
		if (data.itemTid == costumeInfo.Tid)
		{
			isEquipped = true;
			equippedMark.SetActive(true);

		}

		//int nextCount = costumeInfo.LevelUpNeedCount();
		//expSlider.value = (float)costumeInfo.count / nextCount;
		//expText.text = $"{costumeInfo.count}/{nextCount}";
	}

	public void ShowSlider(bool show)
	{
		//expSliderObject.SetActive(show);
	}

	public void OnSelect(long tid)
	{
		if (costumeInfo == null)
		{
			return;
		}
		if (tid == costumeInfo.Tid)
		{

		}
	}

	public void OnRefresh()
	{

	}

	public void OnClickSelect()
	{
		parent?.SetSelectedTid(costumeInfo.Tid);
		action?.Invoke();
	}

}
