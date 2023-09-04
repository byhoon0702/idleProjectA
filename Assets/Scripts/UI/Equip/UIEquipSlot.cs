using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;




public class UIEquipSlot : MonoBehaviour
{
	[SerializeField] private ItemUIEquip itemUI;
	[SerializeField] private Button slotButton;

	[SerializeField] private GameObject equippedMark;
	[SerializeField] private GameObject lockedMark;
	[SerializeField] private Slider slider;
	[SerializeField] private TextMeshProUGUI textSliderCount;

	private ISelectListener parent;
	private RuntimeData.EquipItemInfo equipInfo;
	public bool isEquipped { get; private set; }
	Action action;
	private void Awake()
	{

	}

	private void OnDestroy()
	{
		if (parent != null)
		{
			parent.RemoveSelectListener(OnSelect);
		}
	}

	public void OnUpdate(ISelectListener _parent, RuntimeData.EquipItemInfo _equipInfo, Action _action = null)
	{
		parent = _parent;
		equipInfo = _equipInfo;
		action = _action;

		parent?.AddSelectListener(OnSelect);

		slotButton.enabled = action != null;

		lockedMark.SetActive(false);
		equippedMark.SetActive(false);
		ShowSlider(true);
		itemUI.OnUpdate(equipInfo);
		if (equipInfo == null)
		{
			return;
		}


		itemUI.ShowLevel(equipInfo.unlock);
		itemUI.ShowStars(equipInfo.unlock);
		lockedMark.SetActive(equipInfo.unlock == false);

		var data = PlatformManager.UserDB.equipContainer.GetSlot(equipInfo.type);
		isEquipped = false;
		if (data.itemTid == equipInfo.Tid)
		{
			isEquipped = true;
			equippedMark.SetActive(true);
		}

		slider.value = (float)equipInfo.Count / (float)EquipContainer.needCount;

		textSliderCount.text = $"{equipInfo.Count}/{EquipContainer.needCount}";

	}
	public void ShowEquipMark(bool isTrue)
	{
		equippedMark.SetActive(isTrue);
	}

	public void ShowSlider(bool isTrue)
	{
		slider.gameObject.SetActive(isTrue);
	}

	public void OnSelect(long tid)
	{
		if (equipInfo == null)
		{
			return;
		}
		if (tid == equipInfo.Tid)
		{

		}
	}

	public void OnRefresh()
	{

	}

	public void OnClickSelect()
	{
		parent?.SetSelectedTid(equipInfo.Tid);
		action?.Invoke();
	}

}
