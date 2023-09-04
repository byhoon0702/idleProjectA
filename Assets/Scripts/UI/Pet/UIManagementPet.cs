using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public delegate void OnSelect(long tid);

public interface ISelectListener
{
	void SetSelectedTid(long tid);
	void AddSelectListener(OnSelect callback);
	void RemoveSelectListener(OnSelect callback);

}


public class UIManagementPet : UIBase, ISelectListener
{
	[SerializeField] private UIManagementPetInfo petInfoUI;
	public UIManagementPetInfo PetInfoUI => petInfoUI;
	[SerializeField] private GameObject equipList;
	public GameObject EquipList => equipList;

	[Header("장착동료")]
	[SerializeField] private GameObject[] petSlotHighlights;
	[SerializeField] private UIPetSlot[] petSlots;
	public UIPetSlot[] PetSlots => petSlots;

	[Header("아이템리스트")]
	[SerializeField] private UIPetGrid uiPetGrid;
	public UIPetGrid UiPetGrid => uiPetGrid;
	[SerializeField] private UIPopupLevelupPetItem uIPopupLevelupPetItem;
	public UIPopupLevelupPetItem UiPopupPetLevelup => uIPopupLevelupPetItem;
	[SerializeField] private UIPopupPetEvolution uIPopupPetEvolution;
	public UIPopupPetEvolution UiPopupPetEvolution => uIPopupPetEvolution;

	[Header("Button")]
	[SerializeField] private Button closeButton;
	[SerializeField] private Button upgradeAllButton;

	public long selectedItemTid { get; private set; }
	public event OnSelect onSelect;

	private bool exchangeSlot;

	protected override void OnEnable()
	{
		base.OnEnable();
		equipList.SetActive(false);

	}

	protected override void OnDisable()
	{
		base.OnDisable();
	}

	private void Awake()
	{
	}


	public void OnUpdate(bool _refreshGrid)
	{
		UpdateEquipItem();
		UpdateItemList(_refreshGrid);
		UpdateInfo();
	}
	public void AddSelectListener(OnSelect listener)
	{
		if (onSelect.IsRegistered(listener) == false)
		{
			onSelect += listener;
		}
	}
	public void RemoveSelectListener(OnSelect listener)
	{
		if (onSelect.IsRegistered(listener))
		{
			onSelect -= listener;
		}
	}

	public void SetSelectedTid(long tid)
	{
		if (exchangeSlot)
		{
			return;
		}

		selectedItemTid = tid;
	}

	public void UpdateEquipItem()
	{
		var petContainer = PlatformManager.UserDB.petContainer;
		for (int i = 0; i < petContainer.PetSlots.Length; i++)
		{

			var slot = petSlots[i];
			var slotData = petContainer.PetSlots[i];
			slot.gameObject.SetActive(true);
			slot.OnUpdate(this, slotData.item, () =>
			{
				ExchangePet(slotData);
				UpdateInfo();
				equipList.SetActive(false);
			});
		}
	}


	public void UpdateItemList(bool _refresh)
	{
		uiPetGrid.Init(this);
		uiPetGrid.OnUpdate(PlatformManager.UserDB.petContainer.petList);
	}

	public void UpdateInfo()
	{
		exchangeSlot = false;
		for (int i = 0; i < petSlotHighlights.Length; i++)
		{
			petSlotHighlights[i].SetActive(false);
		}
		RuntimeData.PetInfo info = null;
		info = PlatformManager.UserDB.petContainer.petList.Find(x => x.Tid == selectedItemTid);
		if (info == null)
		{
			info = PlatformManager.UserDB.petContainer.petList[0];
			selectedItemTid = info.Tid;
		}

		petInfoUI.OnUpdate(this, info);

		onSelect?.Invoke(selectedItemTid);
	}


	public void ExchangePet(PetSlot slot)
	{
		slot.UnEquip();
		slot.Equip(selectedItemTid);

		for (int i = 0; i < petSlotHighlights.Length; i++)
		{
			petSlotHighlights[i].SetActive(false);
		}

		UpdateInfo();
		UpdateEquipItem();
		UpdateItemList(false);

		SpawnManager.it.ChangePet(PlatformManager.UserDB.petContainer.GetIndex(selectedItemTid));
	}

	public void EquipPet()
	{
		equipList.SetActive(true);
		exchangeSlot = true;
		//bool equipped = PlatformManager.UserDB.petContainer.Equip(selectedItemTid);

		//if (equipped == false)
		//{
		//	exchangeSlot = true;
		//	for (int i = 0; i < petSlotHighlights.Length; i++)
		//	{
		//		petSlotHighlights[i].SetActive(true);
		//	}
		//	return;
		//}

		//SpawnManager.it.AddPet(PlatformManager.UserDB.petContainer.GetIndex(selectedItemTid));
		//UpdateEquipItem();
		//UpdateItemList(false);
		//UpdateInfo();
	}

	public void UnEquipPet()
	{
		SpawnManager.it.RemovePet(PlatformManager.UserDB.petContainer.GetIndex(selectedItemTid));
		PlatformManager.UserDB.petContainer.Unequip(selectedItemTid);

		UpdateEquipItem();
		UpdateItemList(false);
		UpdateInfo();

	}



	protected override void OnClose()
	{
		if (equipList.activeInHierarchy)
		{
			equipList.SetActive(false);
			return;
		}
		UIController.it.InactivateAllBottomToggle();
		base.OnClose();

	}
}
