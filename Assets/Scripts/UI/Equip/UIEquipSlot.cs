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
		itemUI.OnUpdate(equipInfo);

		lockedMark.SetActive(false);
		equippedMark.SetActive(false);
		//ShowSlider(false);
		if (equipInfo == null)
		{
			return;
		}

		lockedMark.SetActive(equipInfo.unlock == false);

		var data = GameManager.UserDB.equipContainer.GetSlot(equipInfo.type);
		isEquipped = false;
		if (data.itemTid == equipInfo.tid)
		{
			isEquipped = true;
			equippedMark.SetActive(true);

		}

	}

	public void OnSelect(long tid)
	{
		if (equipInfo == null)
		{
			return;
		}
		if (tid == equipInfo.tid)
		{

		}
	}

	public void OnRefresh()
	{

	}

	public void OnClickSelect()
	{
		parent?.SetSelectedTid(equipInfo.tid);
		action?.Invoke();
	}

}
