using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;



public class UIEquip_Levelup : MonoBehaviour
{
	[SerializeField] private UIItemBase uiItem;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI equipText;
	[SerializeField] private TextMeshProUGUI toOwnText;

	[SerializeField] private Button equipButton;
	[SerializeField] private Button upgradeButton;
	[SerializeField] private Button optionButton;
	[SerializeField] private Button upgradeAllButton;


	private UIEquip owner;



	private void Awake()
	{
		equipButton.onClick.RemoveAllListeners();
		equipButton.onClick.AddListener(OnEquipButtonClick);
		upgradeButton.onClick.RemoveAllListeners();
		upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
		optionButton.onClick.RemoveAllListeners();
		optionButton.onClick.AddListener(OnOptionButtonClick);
		upgradeAllButton.onClick.RemoveAllListeners();
		upgradeAllButton.onClick.AddListener(OnUpgradeAllButtonClick);
	}
	RuntimeData.EquipItemInfo equipInfo;


	public void OnUpdate(UIEquip _owner)
	{
		owner = _owner;
		equipInfo = VGameManager.it.userDB.inventory.FindEquipItem(owner.itemData.tid, owner.equipType);
		UpdateItemInfo();
		UpdateItemLevelupInfo();
		UpdateButton();
	}

	public void UpdateItemInfo()
	{
		if (owner.itemData == null)
		{
			nameText.text = "";
			uiItem.gameObject.SetActive(false);
		}
		else
		{
			uiItem.gameObject.SetActive(true);
			nameText.text = owner.itemData.name;

			//var item = VGameManager.it.userDB.inventory.FindEquipItem(owner.itemData.tid, owner.itemData.equipType);
			if (equipInfo != null)
			{
				//uiItem.OnUpdate(item);
			}
			else
			{
				uiItem.OnUpdate(owner.itemData);
			}
		}
	}

	public void UpdateItemLevelupInfo()
	{
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < owner.itemData.equipValues.Count; i++)
		{
			sb.Append($"{owner.itemData.equipValues[i].type.ToUIString()}");
			sb.Append($" {owner.itemData.equipValues[i].value}");
			sb.Append('\n');
		}
		equipText.text = $"{sb.ToString()}";

		sb.Clear();
		for (int i = 0; i < owner.itemData.ownValues.Count; i++)
		{
			sb.Append($"{owner.itemData.ownValues[i].type.ToUIString()}");

			sb.Append($" {owner.itemData.ownValues[i].value}");
			sb.Append('\n');
		}

		toOwnText.text = $"{sb.ToString()}";
	}
	public void UpdateButton()
	{



		if (equipInfo != null)
		{
			bool buttonActive = equipInfo.count > 0;
			bool equipped = equipInfo.tid != VGameManager.it.userDB.equipContainer[owner.itemData.equipType].itemTid;
			bool levelupable = ItemLevelupable();

			equipButton.interactable = buttonActive && equipped;
			upgradeButton.interactable = buttonActive && levelupable;
		}
		else
		{
			equipButton.interactable = false;
			upgradeButton.interactable = false;
		}

		optionButton.interactable = false;
		upgradeAllButton.interactable = false;
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
		//var info = VGameManager.it.userDB.inventory.FindEquipItem(owner.itemData.tid, owner.equipType);
		VGameManager.it.userDB.equipContainer[owner.equipType].Equip(equipInfo);

		//UserInfo.SaveUserData();
		owner.UpdateItems(true);

	}

	private void OnUpgradeButtonClick()
	{
		//equipInfo
		//ItemEquip item = Inventory.it.FindItemByTid(owner.itemData.tid) as ItemEquip;
		//if (ItemLevelupable())
		//{
		//	if (Inventory.it.ConsumeItem(item.Tid, new IdleNumber(item.nextExp)).Ok())
		//	{
		//		item.AddLevel(1);
		//		UpdateItemLevelupInfo();
		//		UpdateButton();
		//	}
		//}
	}

	private void OnOptionButtonClick()
	{

	}

	private void OnUpgradeAllButtonClick()
	{

	}
}
