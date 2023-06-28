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
	[SerializeField] private GameObject equipList;

	[Header("장착동료")]
	[SerializeField] private GameObject[] petSlotHighlights;
	[SerializeField] private UIPetSlot[] petSlots;

	[Header("아이템리스트")]
	[SerializeField] private UIPetGrid uiPetGrid;
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
		equipList.gameObject.SetActive(false);

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
		var petContainer = GameManager.UserDB.petContainer;
		for (int i = 0; i < petContainer.PetSlots.Length; i++)
		{

			var slot = petSlots[i];
			var slotData = petContainer.PetSlots[i];
			slot.gameObject.SetActive(true);
			slot.OnUpdate(this, slotData.item, () =>
			{
				if (exchangeSlot)
				{
					ExchangePet(slotData);
					return;
				}
				UpdateInfo();
			});
		}
	}


	public void UpdateItemList(bool _refresh)
	{
		uiPetGrid.Init(this);
		uiPetGrid.OnUpdate(GameManager.UserDB.petContainer.petList);


	}

	public void UpdateInfo()
	{
		exchangeSlot = false;
		for (int i = 0; i < petSlotHighlights.Length; i++)
		{
			petSlotHighlights[i].SetActive(false);
		}
		RuntimeData.PetInfo info = null;
		info = GameManager.UserDB.petContainer.petList.Find(x => x.Tid == selectedItemTid);
		if (info == null)
		{
			info = GameManager.UserDB.petContainer.petList[0];
			selectedItemTid = info.Tid;
		}

		petInfoUI.OnUpdate(this, info);

		onSelect?.Invoke(selectedItemTid);
	}



	public void ExchangePet(PetSlot slot)
	{
		equipList.gameObject.SetActive(true);
		GameManager.UserDB.petContainer.Unequip(slot.itemTid);
		GameManager.UserDB.petContainer.Equip(selectedItemTid);

		for (int i = 0; i < petSlotHighlights.Length; i++)
		{
			petSlotHighlights[i].SetActive(false);
		}
		exchangeSlot = false;

		UpdateEquipItem();
		UpdateItemList(false);
		UpdateInfo();

		SpawnManager.it.ChangePet(GameManager.UserDB.petContainer.GetIndex(selectedItemTid));
	}

	public void EquipPet()
	{
		bool equipped = GameManager.UserDB.petContainer.Equip(selectedItemTid);

		if (equipped == false)
		{
			exchangeSlot = true;
			for (int i = 0; i < petSlotHighlights.Length; i++)
			{
				petSlotHighlights[i].SetActive(true);
			}
			return;
		}

		SpawnManager.it.AddPet(GameManager.UserDB.petContainer.GetIndex(selectedItemTid));
		UpdateEquipItem();
		UpdateItemList(false);
		UpdateInfo();


	}

	public void UnEquipPet()
	{
		SpawnManager.it.RemovePet(GameManager.UserDB.petContainer.GetIndex(selectedItemTid));
		GameManager.UserDB.petContainer.Unequip(selectedItemTid);

		UpdateEquipItem();
		UpdateItemList(false);
		UpdateInfo();

	}



	public override void Close()
	{
		UIController.it.InactivateAllBottomToggle();
		gameObject.SetActive(false);
		equipList.gameObject.SetActive(false);
	}
}
