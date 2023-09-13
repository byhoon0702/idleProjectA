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

	[SerializeField] private GameObject equippedMark;
	[SerializeField] private GameObject lockedMark;
	[SerializeField] private Slider slider;
	[SerializeField] private TextMeshProUGUI textSlider;

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

	public void ShowEquipMark(bool isTrue)
	{
		equippedMark.SetActive(isTrue);
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

		var data = PlatformManager.UserDB.petContainer.PetSlots;
		isEquipped = false;
		for (int i = 0; i < data.Length; i++)
		{
			if (data[i].itemTid == petInfo.Tid)
			{
				isEquipped = true;
				equippedMark.SetActive(true);
				break;
			}
		}

		bool isMax = petInfo.IsMaxEvolution();


		if (isMax)
		{
			slider.value = 1f;

			textSlider.text = $"{petInfo.Count}/Max";
		}
		else
		{
			var evol_data = PlatformManager.CommonData.PetEvolutionLevelDataList[petInfo.evolutionLevel];

			slider.value = petInfo.Count / (float)evol_data.consumeCount;

			textSlider.text = $"{petInfo.Count}/{evol_data.consumeCount}";
		}
	}

	public void ShowSlider(bool isTrue)
	{
		slider.gameObject.SetActive(isTrue);
	}

	public void OnSelect(long tid)
	{
		if (petInfo == null)
		{
			return;
		}
		if (tid == petInfo.Tid)
		{

		}
	}

	public void OnRefresh()
	{

	}

	public void OnClickSelect()
	{
		if (parent != null && petInfo != null)
		{
			parent.SetSelectedTid(petInfo.Tid);
		}
		action?.Invoke();
	}
}
