using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class UIManagementEquipInfo : UIManagementBaseInfo<RuntimeData.EquipItemInfo>
{
	[SerializeField] private UIManagementEquip.EquipPage page;

	[SerializeField] private UIEquipSlot uiSlot;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI equipText;
	[SerializeField] private TextMeshProUGUI ownText;

	[SerializeField] private Button equipButton;
	[SerializeField] private Button levelupButton;
	[SerializeField] private Button upgradeButton;

	[SerializeField] private Button upgradeAllButton;

	[SerializeField] private UIItemOptionText[] ownedBuffs;
	[SerializeField] private UIItemOptionText[] optionBuffs;

	private UIManagementEquip parent;
	private RuntimeData.EquipItemInfo equipInfo;

	private void Awake()
	{
		equipButton.onClick.RemoveAllListeners();
		equipButton.onClick.AddListener(OnEquipButtonClick);
		levelupButton.onClick.RemoveAllListeners();
		levelupButton.onClick.AddListener(OnClickShowLevelUp);
		upgradeButton.onClick.RemoveAllListeners();
		upgradeButton.onClick.AddListener(OnClickShowUpgrade);

		upgradeAllButton.onClick.RemoveAllListeners();
		upgradeAllButton.onClick.AddListener(OnUpgradeAllButtonClick);
	}

	public override void OnUpdate(UIManagementEquip _parent, RuntimeData.EquipItemInfo _info)
	{
		parent = _parent;
		equipInfo = _info;
		UpdateItemInfo();
		UpdateItemLevelupInfo();
		UpdateButton();
	}

	public void UpdateItemInfo()
	{
		uiSlot.OnUpdate(parent, equipInfo, null);
		nameText.text = equipInfo.itemObject.ItemName;

	}

	public void UpdateItemLevelupInfo()
	{
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < equipInfo.equipAbilities.Count; i++)
		{
			string tail = equipInfo.equipAbilities[i].tailChar;
			sb.Append($"{equipInfo.equipAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{equipInfo.equipAbilities[i].Value.ToString()}{tail}</color>");
			sb.Append('\n');
		}
		equipText.text = $"{sb.ToString()}";

		sb.Clear();
		for (int i = 0; i < equipInfo.ownedAbilities.Count; i++)
		{
			string tail = equipInfo.ownedAbilities[i].tailChar;
			sb.Append($"{equipInfo.ownedAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{equipInfo.ownedAbilities[i].Value.ToString()}{tail}</color>");
			sb.Append('\n');
		}

		ownText.text = $"{sb.ToString()}";
	}
	public void UpdateButton()
	{
		if (equipInfo != null)
		{
			bool buttonActive = equipInfo.unlock;
			bool equipped = equipInfo.Tid != GameManager.UserDB.equipContainer.GetSlot(equipInfo.rawData.equipType).itemTid;
			bool levelupable = ItemLevelupable();

			equipButton.interactable = buttonActive && equipped;

		}
		else
		{
			equipButton.interactable = false;
		}

		//upgradeAllButton.interactable = false;
	}
	public bool ItemLevelupable()
	{


		if (equipInfo == null && equipInfo.count == 0)
		{
			return false;
		}
		if (equipInfo.CanLevelUp() == false)
		{
			return false;
		}
		//if (Inventory.it.CheckMoney(item.Tid, new IdleNumber(item.nextExp)).Fail())
		//{
		//	return false;
		//}

		return true;
	}


	/// <summary>
	/// 실제 착용정보가 바뀜
	/// </summary>
	private void OnEquipButtonClick()
	{
		GameManager.UserDB.equipContainer.GetSlot(parent.equipType).Equip(equipInfo);
		//parent.UpdateItems(true);

		UpdateItemInfo();
		UpdateItemLevelupInfo();
		UpdateButton();
		parent.OnUpdateEquip(parent.equipType, equipInfo.Tid);
	}

	public void OnClickShowLevelUp()
	{
		parent.UiPopupEquipLevelup.OnUpdate(parent, equipInfo);
	}

	public void OnClickShowUpgrade()
	{
		parent.UiPopupEquipUpgrade.OnUpdate(parent, equipInfo);
	}

	private void OnUpgradeAllButtonClick()
	{
		GameManager.UserDB.equipContainer.UpgradeAll(parent.equipType);
		parent.OnUpdateEquip(parent.equipType, equipInfo.Tid);

	}
}
