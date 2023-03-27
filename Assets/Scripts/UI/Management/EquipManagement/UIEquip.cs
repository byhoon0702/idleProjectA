using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;


/// <summary>
/// <see cref="ChangeCurrentItem"/> 보여지는 장비 아이템 변경(UI만 갱신)
/// <see cref="OnEquipButtonClick"/> 장착 아이템변경(UI, 착용)
/// <see cref="OnUpgradeButtonClick"/> 아이템 강화 시도
/// </summary>
public class UIEquip : MonoBehaviour, IUIClosable
{
	[Header("메인 탭")]
	[SerializeField] private Button weaponTab;
	[SerializeField] private Button armorTab;
	[SerializeField] private Button ringTab;
	[SerializeField] private Button necklaceTab;

	[Header("서브 탭")]
	[SerializeField] private Button levelupTab;
	[SerializeField] private Button upgradeTab;
	[SerializeField] private Button limitTab;
	[SerializeField] private Button setTab;

	[Header("UI리스트")]
	[SerializeField] private UIEquip_Levelup levelupUI;

	[Header("리스트")]
	[SerializeField] private UIItemEquipChange itemPrefab;
	[SerializeField] private RectTransform itemRoot;

	[Header("버튼")]
	[SerializeField] private Button closeButton;


	public long selectedItemTid { get; private set; }

	public EquipItemData itemData { get; private set; }

	public EquipType equipType = EquipType.WEAPON;
	private void Awake()
	{
		weaponTab.onClick.AddListener(() =>
		{
			if (equipType == EquipType.WEAPON)
			{
				return;
			}
			OnUpdate(EquipType.WEAPON, UserInfo.equip.EquipWeaponTid, false);
		});
		armorTab.onClick.AddListener(() =>
		{
			if (equipType == EquipType.ARMOR)
			{
				return;
			}
			OnUpdate(EquipType.ARMOR, UserInfo.equip.EquipArmorTid, false);
		});
		ringTab.onClick.AddListener(() =>
		{
			if (equipType == EquipType.RING)
			{
				return;
			}
			OnUpdate(EquipType.RING, UserInfo.equip.EquipAccessoryTid, false);
		});
		necklaceTab.onClick.AddListener(() =>
		{
			if (equipType == EquipType.NECKLACE)
			{
				return;
			}
			OnUpdate(EquipType.NECKLACE, UserInfo.equip.EquipNecklaceTid, false);
		});
	}

	public void OnUpdate(EquipType _itemType, long _selectedItemTid, bool _refreshGrid)
	{
		selectedItemTid = _selectedItemTid;
		equipType = _itemType;

		if (selectedItemTid == 0)
		{
			// 선택된 아이템이 없는경우 첫번째 아이템을 강제선택 시킴
			selectedItemTid = DefaultSelectTid();
		}

		itemData = null;
		VResult vResult = InitData(selectedItemTid);
		if (vResult.Fail())
		{
			PopAlert.Create(vResult);
			gameObject.SetActive(false);
			return;
		}

		UpdateItems(_refreshGrid);
		levelupUI.OnUpdate(this);
	}

	private VResult InitData(long _itemTid)
	{
		VResult result = new VResult();

		itemData = DataManager.Get<EquipItemDataSheet>().Get(_itemTid);
		if (itemData == null)
		{
			return result.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. itemTid: {_itemTid}");
		}

		return result.SetOk();
	}


	public void UpdateItems(bool _refresh)
	{

		if (_refresh == false)
		{
			foreach (var v in itemRoot.GetComponentsInChildren<UIItemEquipChange>())
			{
				Destroy(v.gameObject);
			}

			var list = DataManager.Get<EquipItemDataSheet>().GetByItemType(equipType);
			for (int i = 0; i < list.Count; i++)
			{
				//var item = Instantiate(itemPrefab, itemRoot);
				//item.OnUpdate(this, list[i]);
			}
			//case ItemType.Weapon:
			//	equipType = EquipType.WEAPON;
			//	break;
			//case ItemType.Armor:
			//	equipType = EquipType.ARMOR;
			//	break;
			//case ItemType.Ring:
			//	equipType = EquipType.RING;
			//	break;
			//case ItemType.Necklace:
			//	equipType = EquipType.NECKLACE;
			//	break;

			foreach (var v in VGameManager.it.userDB.inventory[equipType])
			{
				var item = Instantiate(itemPrefab, itemRoot);
				item.OnUpdate(this, v);
			}
		}

		foreach (var v in itemRoot.GetComponentsInChildren<UIItemEquipChange>())
		{
			v.SetSelect(v.UIData.tid == selectedItemTid);
			v.SetEquipped(v.UIData.tid == VGameManager.it.userDB.equipContainer[equipType].itemTid);
		}
	}

	private long DefaultSelectTid()
	{
		return DataManager.Get<EquipItemDataSheet>().GetByItemType(equipType)[0].tid;
	}

	public void ChangeCurrentItem(long _itemTid)
	{
		OnUpdate(equipType, _itemTid, true);
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);

		var uiMain = FindObjectOfType<UIManagementMain>();
		if (uiMain != null)
		{
			uiMain.UpdateEquipSlot();
		}
	}
}
