﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIPetSlot : MonoBehaviour
{
	[SerializeField] private ItemUIPet itemUI;
	[SerializeField] private Button slotButton;

	[SerializeField] private GameObject equippedMark;
	[SerializeField] private GameObject lockedMark;

	private ISelectListener parent;
	private RuntimeData.PetInfo petInfo;
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

	public void OnUpdate(ISelectListener _parent, RuntimeData.PetInfo _petInfo, Action _action = null)
	{
		parent = _parent;
		petInfo = _petInfo;
		action = _action;

		parent?.AddSelectListener(OnSelect);

		slotButton.enabled = action != null;
		itemUI.OnUpdate(petInfo);

		lockedMark.SetActive(false);
		equippedMark.SetActive(false);

		if (petInfo == null)
		{
			return;
		}

		lockedMark.SetActive(petInfo.unlock == false);

		var data = GameManager.UserDB.petContainer.PetSlots;
		isEquipped = false;
		for (int i = 0; i < data.Length; i++)
		{
			if (data[i].itemTid == petInfo.tid)
			{
				isEquipped = true;
				equippedMark.SetActive(true);
				break;
			}
		}
	}

	public void OnSelect(long tid)
	{
		if (petInfo == null)
		{
			return;
		}
		if (tid == petInfo.tid)
		{

		}
	}

	public void OnRefresh()
	{

	}

	public void OnClickSelect()
	{
		parent.SetSelectedTid(petInfo.tid);
		action?.Invoke();
	}
}
