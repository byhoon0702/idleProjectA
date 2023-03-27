using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIPetSlot : MonoBehaviour
{
	[SerializeField] private ItemUIPet itemUI;
	[SerializeField] private Button slotButton;

	[SerializeField] protected GameObject expSliderObject;
	[SerializeField] protected Slider expSlider;
	[SerializeField] protected TextMeshProUGUI expText;


	[SerializeField] private GameObject equippedMark;
	[SerializeField] private GameObject lockedMark;

	private UIManagementPet parent;
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

	public void OnUpdate(UIManagementPet _parent, RuntimeData.PetInfo _petInfo, Action _action = null)
	{
		parent = _parent;
		petInfo = _petInfo;
		action = _action;

		parent.AddSelectListener(OnSelect);

		slotButton.interactable = action != null;
		itemUI.OnUpdate(petInfo);

		lockedMark.SetActive(false);
		equippedMark.SetActive(false);
		ShowSlider(false);
		if (petInfo == null)
		{
			return;
		}

		lockedMark.SetActive(petInfo.count == 0);

		var data = VGameManager.UserDB.petContainer.petSlot;
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
		//OnRefresh();
	}

	public void ShowSlider(bool show)
	{
		expSliderObject.SetActive(show);
	}

	public void OnSelect(long tid)
	{
		if (tid == petInfo.tid)
		{

		}
	}

	public void OnRefresh()
	{

	}

	public void OnClickSelect()
	{
		parent.selectedItemTid = petInfo.tid;
		action?.Invoke();
	}
}
