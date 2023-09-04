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
	_END,
}


public class UIManagementEquip : UIBase, ISelectListener
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
	public Toggle WeaponTab => weaponTab;
	[SerializeField] private Toggle armorTab;
	public Toggle ArmorTab => armorTab;
	[SerializeField] private Toggle ringTab;
	public Toggle RingTab => ringTab;
	[SerializeField] private Toggle necklaceTab;
	public Toggle NecklaceTab => necklaceTab;

	[Header("UI리스트")]
	[SerializeField] private UIManagementEquipInfo uiEquipInfo;

	[Header("팝업")]
	[SerializeField] private UIPopupLevelupEquipItem uiPopupEquipLevelup;
	public UIPopupLevelupEquipItem UiPopupEquipLevelup => uiPopupEquipLevelup;

	[SerializeField] private UIPopupEquipUpgrade uiPopupEquipUpgrade;
	public UIPopupEquipUpgrade UiPopupEquipUpgrade => uiPopupEquipUpgrade;


	[SerializeField] private UIPopupEquipBreakthrough uiPopupEquipBreakthrough;
	public UIPopupEquipBreakthrough UiPopupEquipBreakthrough => uiPopupEquipBreakthrough;


	[Header("Grid")]
	[SerializeField] private UIEquipGrid uiEquipGrid;
	public UIEquipGrid UiEquipGrid => uiEquipGrid;

	public EquipType equipType { get; private set; }
	public EquipTabType tabType { get; private set; }
	public long selectedItemTid { get; private set; }

	public event OnSelect onSelect;

	private RuntimeData.ItemInfo selectedInfo;


	public void UpdateTabSlot()
	{
		weaponTab.onValueChanged.AddListener((toggle) =>
		{
			if (tabType != EquipTabType.WEAPON)
			{
				OnUpdateEquip(EquipType.WEAPON, PlatformManager.UserDB.equipContainer.GetSlot(EquipType.WEAPON).itemTid);
			}
		});
		armorTab.onValueChanged.AddListener((toggle) =>
		{
			if (tabType != EquipTabType.ARMOR)
			{
				OnUpdateEquip(EquipType.ARMOR, PlatformManager.UserDB.equipContainer.GetSlot(EquipType.ARMOR).itemTid);
			}

		});
		ringTab.onValueChanged.AddListener((toggle) =>
		{
			if (tabType != EquipTabType.RING)
			{
				OnUpdateEquip(EquipType.RING, PlatformManager.UserDB.equipContainer.GetSlot(EquipType.RING).itemTid);
			}

		});
		necklaceTab.onValueChanged.AddListener((toggle) =>
		{
			if (tabType != EquipTabType.NECKLACE)
			{
				OnUpdateEquip(EquipType.NECKLACE, PlatformManager.UserDB.equipContainer.GetSlot(EquipType.NECKLACE).itemTid);
			}
		});

	}

	private void Awake()
	{
		UpdateTabSlot();
	}


	public void OnUpdateEquip(EquipType _itemType, long _selectedItemTid)
	{
		selectedItemTid = _selectedItemTid;
		equipType = _itemType;
		tabType = (EquipTabType)_itemType;

		if (selectedItemTid == 0)
		{
			selectedItemTid = PlatformManager.UserDB.equipContainer.GetSlot(_itemType).itemTid;
		}

		var list = PlatformManager.UserDB.equipContainer.GetList(equipType);
		selectedInfo = list.Find(x => x.Tid == selectedItemTid);
		if (selectedInfo == null)
		{
			selectedInfo = list[0];
			selectedItemTid = selectedInfo.Tid;
		}

		uiEquipGrid.gameObject.SetActive(true);
		uiEquipGrid.Init(this);
		uiEquipGrid.OnUpdate(list);


		uiEquipInfo.gameObject.SetActive(true);
		uiEquipInfo.OnUpdate(this, selectedInfo as RuntimeData.EquipItemInfo);
	}

	public void SelectEquipItem(long tid)
	{
		selectedItemTid = tid;
		selectedInfo = PlatformManager.UserDB.equipContainer.GetList(equipType).Find(x => x.Tid == selectedItemTid);
	}

	public void UpdateInfo()
	{
		if (selectedInfo is RuntimeData.EquipItemInfo)
		{
			uiEquipInfo.OnUpdate(this, selectedInfo as RuntimeData.EquipItemInfo);
		}

		onSelect?.Invoke(selectedItemTid);
	}

	private long DefaultSelectTid()
	{
		long tid = PlatformManager.UserDB.equipContainer.GetSlot(equipType).itemTid;
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


		}
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

	//protected override void OnClose()
	//{

	//	UIController.it.InactivateAllBottomToggle();
	//	base.OnClose();

	//}
}
