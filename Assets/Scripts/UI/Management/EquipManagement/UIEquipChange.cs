using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// <see cref="ChangeCurrentItem"/> 보여지는 장비 아이템 변경(UI만 갱신)
/// <see cref="OnEquipButtonClick"/> 장착 아이템변경(UI, 착용)
/// <see cref="OnUpgradeButtonClick"/> 아이템 강화 시도
/// </summary>
public class UIEquipChange : MonoBehaviour, IUIClosable
{
	[Header("아이템")]
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI gradeText;
	[SerializeField] private TextMeshProUGUI equipText;
	[SerializeField] private TextMeshProUGUI toOwnText;
	[SerializeField] private TextMeshProUGUI levelText;

	[SerializeField] private Slider expSlider;
	[SerializeField] private TextMeshProUGUI textExp;

	[Header("리스트")]
	[SerializeField] private UIItemEquipChange itemPrefab;
	[SerializeField] private RectTransform itemRoot;


	[Header("버튼")]
	[SerializeField] private Button closeButton;
	[SerializeField] private Button equipButton;
	[SerializeField] private Button upgradeButton;
	[SerializeField] private Button summonButton;
	[SerializeField] private Button upgradeAllButton;

	private long selectedItemTid;

	private ItemData itemData;
	private ItemType itemType;




	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);
		equipButton.onClick.RemoveAllListeners();
		equipButton.onClick.AddListener(OnEquipButtonClick);
		upgradeButton.onClick.RemoveAllListeners();
		upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
		summonButton.onClick.RemoveAllListeners();
		summonButton.onClick.AddListener(OnSummonButtonClick);
		upgradeAllButton.onClick.RemoveAllListeners();
		upgradeAllButton.onClick.AddListener(OnUpgradeAllButtonClick);
	}

	public void OnUpdate(ItemType _itemType, long _selectedItemTid, bool _refreshGrid)
	{
		selectedItemTid = _selectedItemTid;
		itemType = _itemType;

		if(selectedItemTid == 0)
		{
			// 선택된 아이템이 없는경우 첫번째 아이템을 강제선택 시킴
			selectedItemTid = DefaultSelectTid();
		}

		itemData = null;
		VResult vResult = InitData(selectedItemTid);
		if (vResult.Fail())
		{
			PopAlert.it.Create(vResult);
			gameObject.SetActive(false);
			return;
		}

		UpdateItemInfo();
		UpdateItemLevelupInfo();
		UpdateItems(_refreshGrid);
		UpdateButton();
	}

	private void OnDisable()
	{
		UserInfo.SaveUserData();
	}

	private VResult InitData(long _itemTid)
	{
		VResult result = new VResult();

		itemData = DataManager.Get<ItemDataSheet>().Get(_itemTid);
		if (itemData == null)
		{
			return result.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. itemTid: {_itemTid}");
		}

		return result.SetOk();
	}

	public void UpdateItemInfo()
	{
		if (itemData == null)
		{
			nameText.text = "";
			gradeText.text = "";

			icon.gameObject.SetActive(false);
		}
		else
		{
			nameText.text = itemData.name;
			gradeText.text = itemData.itemGrade.ToString();

			icon.gameObject.SetActive(true);
			icon.sprite = Resources.Load<Sprite>($"Icon/{itemData.Icon}");
		}
	}

	public void UpdateItemLevelupInfo()
	{
		equipText.text = $"장착효과: {itemData.EquipAbilityInfo.ToString()}";
		toOwnText.text = $"보유효과: {itemData.ToOwnAbilityInfo.ToString()}";


		ItemEquip item = Inventory.it.FindItemByTid(itemData.tid) as ItemEquip;

		if (item != null)
		{
			levelText.text = $"Lv. {item.Level}";
			expSlider.value = item.expRatio;
			if (item.IsMaxLv)
			{
				textExp.text = $"EXP Max ({(item.expRatio * 100).ToString("F2")}%)";
			}
			else
			{
				textExp.text = $"EXP {item.Exp.ToString("N0")} / {item.nextExp.ToString("N0")} ({(item.expRatio * 100).ToString("F2")}%)";
			}
		}
		else
		{
			levelText.text = $"";
			expSlider.value = 0;
			textExp.text = $"";
		}
	}

	private void UpdateItems(bool _refresh)
	{
		if (_refresh == false)
		{
			foreach (var v in itemRoot.GetComponentsInChildren<UIItemEquipChange>())
			{
				Destroy(v.gameObject);
			}

			foreach (var v in DataManager.Get<ItemDataSheet>().GetByItemType(itemType))
			{
				UIEquipData uiData = new UIEquipData();
				VResult result =  uiData.Setup(v);
				if(result.Fail())
				{
					VLog.LogError($"{result.ToString()}");
					continue;
				}

				var item = Instantiate(itemPrefab, itemRoot);
				item.OnUpdate(this, uiData);
			}
		}

		foreach (var v in itemRoot.GetComponentsInChildren<UIItemEquipChange>())
		{
			v.SetSelect(v.UIData.ItemTid == selectedItemTid);
			v.SetEquipped(v.UIData.ItemTid == UserInfo.GetUserEquipItem(itemType));
		}
	}

	private long DefaultSelectTid()
	{
		return DataManager.Get<ItemDataSheet>().GetByItemType(itemType)[0].tid;
	}


	public void ChangeCurrentItem(long _itemTid)
	{
		OnUpdate(itemType, _itemTid, true);
	}

	public void UpdateButton()
	{
		var item = Inventory.it.FindItemByTid(itemData.tid) as ItemEquip;

		if (item != null)
		{
			bool buttonActive = item.Count > 0;
			bool equipped = selectedItemTid != UserInfo.GetUserEquipItem(itemType);
			bool levelupable = ItemLevelupable();

			equipButton.interactable = buttonActive && equipped;
			upgradeButton.interactable = buttonActive && levelupable;
		}
		else
		{
			equipButton.interactable = false;
			upgradeButton.interactable = false;
		}

		summonButton.interactable = false;
		upgradeAllButton.interactable = false;
	}

	public bool ItemLevelupable()
	{
		var item = Inventory.it.FindItemByTid(itemData.tid) as ItemEquip;
		
		if(item == null && item.Count == 0)
		{
			return false;
		}
		if (item.Levelupable() == false)
		{
			return false;
		}
		if (Inventory.it.CheckMoney(item.Tid, new IdleNumber(item.nextExp)).Fail())
		{
			return false;
		}

		return true;
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);

		var uiMain = FindObjectOfType<UIManagementMain>();
		if(uiMain != null)
		{
			uiMain.UpdateEquipSlot();
		}
	}

	/// <summary>
	/// 실제 착용정보가 바뀜
	/// </summary>
	private void OnEquipButtonClick()
	{
		ItemEquip item = Inventory.it.FindItemByTid(itemData.tid) as ItemEquip;

		if (item.Equipable())
		{
			switch (itemType)
			{
				case ItemType.Weapon:
					UserInfo.EquipWeapon(item.Tid);
					break;
				case ItemType.Armor:
					UserInfo.EquipArmor(item.Tid);
					break;
				case ItemType.Accessory:
					UserInfo.EquipAccessory(item.Tid);
					break;
			}

			UpdateItems(true);
		}
	}

	private void OnUpgradeButtonClick()
	{
		ItemEquip item = Inventory.it.FindItemByTid(itemData.tid) as ItemEquip;
		if (ItemLevelupable())
		{
			if (Inventory.it.ConsumeItem(item.Tid, new IdleNumber(item.nextExp)).Ok())
			{
				item.AddLevel(1);
				UpdateItemLevelupInfo();
				UpdateButton();
			}
		}
	}

	private void OnSummonButtonClick()
	{

	}

	private void OnUpgradeAllButtonClick()
	{

	}
}
