using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public enum EquipTabType
{
	WEAPON = 0,
	ARMOR = 1,
	NECKLACE = 2,
	RING = 3,

	PET = 4,
	_END,

}


public class UIManagementEquip : MonoBehaviour, IUIClosable, ISelectListener
{

	public enum EquipPage
	{
		LEVELUP,
		OPTION,
		BREAKTHROUGH,
		TRANSCENDENCE,
		SET,
	}

	[Header("메인 탭")]
	[SerializeField] private Toggle weaponTab;
	[SerializeField] private Toggle armorTab;
	[SerializeField] private Toggle ringTab;
	[SerializeField] private Toggle necklaceTab;
	[SerializeField] private Toggle petTab;

	[Header("UI리스트")]
	[SerializeField] private UIManagementEquipInfo uiEquipInfo;
	[SerializeField] private UIManagementPetInfo uiPetInfo;

	[Header("팝업")]
	[SerializeField] private UIPopupLevelupEquipItem uiPopupEquipLevelup;
	public UIPopupLevelupEquipItem UiPopupEquipLevelup => uiPopupEquipLevelup;
	[SerializeField] private UIPopupLevelupPetItem uiPopupPetLevelup;
	public UIPopupLevelupPetItem UiPopupPetLevelup => uiPopupPetLevelup;

	[SerializeField] private UIPopupEquipUpgrade uiPopupEquipUpgrade;
	public UIPopupEquipUpgrade UiPopupEquipUpgrade => uiPopupEquipUpgrade;


	[SerializeField] private UIPopupPetEvolution uiPopupPetEvolution;
	public UIPopupPetEvolution UiPopupPetEvolution => uiPopupPetEvolution;



	[Header("Grid")]
	[SerializeField] private UIPetGrid uiPetGrid;
	[SerializeField] private UIEquipGrid uiEquipGrid;

	public EquipType equipType { get; private set; }
	public EquipTabType tabType { get; private set; }
	public long selectedItemTid { get; private set; }

	public event OnSelect onSelect;

	private RuntimeData.ItemInfo selectedInfo;

	void OnEnable()
	{
		AddCloseListener();
	}
	void OnDisable()
	{
		RemoveCloseListener();
	}
	public void AddCloseListener()
	{
		GameUIManager.it.onClose += Close;
	}

	public void RemoveCloseListener()
	{
		GameUIManager.it.onClose -= Close;
	}
	public void UpdateTabSlot()
	{
		weaponTab.onValueChanged.AddListener((toggle) =>
		{
			if (tabType != EquipTabType.WEAPON)
			{
				OnUpdateEquip(EquipType.WEAPON, GameManager.UserDB.equipContainer.GetSlot(EquipType.WEAPON).itemTid);
			}
		});
		armorTab.onValueChanged.AddListener((toggle) =>
		{
			if (tabType != EquipTabType.ARMOR)
			{
				OnUpdateEquip(EquipType.ARMOR, GameManager.UserDB.equipContainer.GetSlot(EquipType.ARMOR).itemTid);
			}

		});
		ringTab.onValueChanged.AddListener((toggle) =>
		{
			if (tabType != EquipTabType.RING)
			{
				OnUpdateEquip(EquipType.RING, GameManager.UserDB.equipContainer.GetSlot(EquipType.RING).itemTid);
			}

		});
		necklaceTab.onValueChanged.AddListener((toggle) =>
		{
			if (tabType != EquipTabType.NECKLACE)
			{
				OnUpdateEquip(EquipType.NECKLACE, GameManager.UserDB.equipContainer.GetSlot(EquipType.NECKLACE).itemTid);
			}
		});
		petTab.onValueChanged.AddListener((togle) =>
		{
			if (tabType != EquipTabType.PET)
			{
				OnUpdatePet(0);
			}

		});
	}

	private void Awake()
	{
		UpdateTabSlot();
	}

	public void OnUpdatePet(long _selectedItemTid)
	{
		selectedItemTid = _selectedItemTid;
		tabType = EquipTabType.PET;

		if (selectedItemTid == 0)
		{
			selectedItemTid = DefaultSelectTid();
		}
		var list = GameManager.UserDB.petContainer.petList;
		selectedInfo = list.Find(x => x.tid == selectedItemTid);
		if (selectedInfo == null)
		{
			selectedInfo = list[0];
			selectedItemTid = selectedInfo.tid;
		}

		uiEquipGrid.gameObject.SetActive(false);
		uiPetGrid.gameObject.SetActive(true);
		uiPetGrid.Init(this);
		uiPetGrid.OnUpdate(list);

		uiEquipInfo.gameObject.SetActive(false);
		uiPetInfo.gameObject.SetActive(true);
		uiPetInfo.OnUpdate(this, selectedInfo as RuntimeData.PetInfo);
	}
	public void OnUpdateEquip(EquipType _itemType, long _selectedItemTid)
	{
		selectedItemTid = _selectedItemTid;
		equipType = _itemType;
		tabType = (EquipTabType)_itemType;

		if (selectedItemTid == 0)
		{
			selectedItemTid = DefaultSelectTid();
		}

		var list = GameManager.UserDB.equipContainer.GetList(equipType);
		selectedInfo = list.Find(x => x.tid == selectedItemTid);
		if (selectedInfo == null)
		{
			selectedInfo = list[0];
			selectedItemTid = selectedInfo.tid;
		}

		uiEquipGrid.gameObject.SetActive(true);
		uiEquipGrid.Init(this);
		uiEquipGrid.OnUpdate(list);

		uiPetGrid.gameObject.SetActive(false);

		uiPetInfo.gameObject.SetActive(false);
		uiEquipInfo.gameObject.SetActive(true);
		uiEquipInfo.OnUpdate(this, selectedInfo as RuntimeData.EquipItemInfo);
	}

	public void SelectEquipItem(long tid)
	{
		selectedItemTid = tid;
		selectedInfo = GameManager.UserDB.equipContainer.GetList(equipType).Find(x => x.tid == selectedItemTid);
	}

	public void SelectPetItem(long tid)
	{
		selectedItemTid = tid;
		selectedInfo = GameManager.UserDB.petContainer.petList.Find(x => x.tid == selectedItemTid);
	}


	public void UpdateInfo()
	{
		if (selectedInfo is RuntimeData.EquipItemInfo)
		{
			uiEquipInfo.OnUpdate(this, selectedInfo as RuntimeData.EquipItemInfo);
		}
		else
		{
			uiPetInfo.OnUpdate(this, selectedInfo as RuntimeData.PetInfo);
		}

		onSelect?.Invoke(selectedItemTid);
	}

	private long DefaultSelectTid()
	{
		long tid = GameManager.UserDB.equipContainer.GetList(equipType)[0].tid;
		if (tid == 0)
		{
			tid = DataManager.Get<EquipItemDataSheet>().GetByItemType(equipType)[0].tid;
		}
		return tid;
	}

	public void OnUpdate(EquipTabType _tabType, long _tid)
	{
		selectedItemTid = _tid;
		tabType = _tabType;

		switch (tabType)
		{
			case EquipTabType.WEAPON:
				weaponTab.isOn = true;
				OnUpdateEquip((EquipType)tabType, _tid);
				break;
			case EquipTabType.ARMOR:
				armorTab.isOn = true;
				OnUpdateEquip((EquipType)tabType, _tid);
				break;
			case EquipTabType.RING:
				ringTab.isOn = true;
				OnUpdateEquip((EquipType)tabType, _tid);
				break;
			case EquipTabType.NECKLACE:
				necklaceTab.isOn = true;
				OnUpdateEquip((EquipType)tabType, _tid);
				break;

			case EquipTabType.PET:
				petTab.isOn = true;
				OnUpdatePet(_tid);
				break;

		}
	}


	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void SetSelectedTid(long tid)
	{
		selectedItemTid = tid;
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

	public void EquipPet(RuntimeData.PetInfo pet)
	{
		selectedItemTid = pet.tid;
		bool equipped = GameManager.UserDB.petContainer.Equip(selectedItemTid);

		if (equipped == false)
		{
			//exchangeSlot = true;
			ExchangePet(GameManager.UserDB.petContainer.PetSlots[0]);
			return;
		}

		SpawnManager.it.AddPet(GameManager.UserDB.petContainer.GetIndex(selectedItemTid));
		UpdateInfo();
		OnUpdatePet(selectedItemTid);
	}

	public void UnEquipPet(RuntimeData.PetInfo pet)
	{
		selectedItemTid = pet.tid;
		SpawnManager.it.RemovePet(GameManager.UserDB.petContainer.GetIndex(selectedItemTid));
		GameManager.UserDB.petContainer.Unequip(selectedItemTid);
		UpdateInfo();
		OnUpdatePet(selectedItemTid);
	}
	public void ExchangePet(PetSlot slot)
	{
		GameManager.UserDB.petContainer.Unequip(slot.itemTid);
		GameManager.UserDB.petContainer.Equip(selectedItemTid);


		UpdateInfo();
		SpawnManager.it.ChangePet(GameManager.UserDB.petContainer.GetIndex(selectedItemTid));
		OnUpdatePet(selectedItemTid);

	}

}
