using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
public class UIPopupLevelupEquipItem : UIBase
{

	[SerializeField] protected UIEconomyButton buttonUpgrade;
	public UIEconomyButton ButtonUpgrade => buttonUpgrade;
	[SerializeField] protected GameObject buttonMax;
	[SerializeField] protected GameObject buttonNeedBreak;

	[SerializeField] protected TextMeshProUGUI textMeshProName;
	[SerializeField] protected TextMeshProUGUI textEquipBuff;
	[SerializeField] protected TextMeshProUGUI[] textOwnedBuff;


	[SerializeField] private Image imageCost;
	[SerializeField] private TextMeshProUGUI textMeshCost;


	[SerializeField] private UIEquipSlot uiEquipSlot;

	protected UIManagementEquip parent;
	protected RuntimeData.EquipItemInfo itemInfo;
	protected virtual void Awake()
	{
		buttonUpgrade.SetButtonEvent(OnClickLevelUp);
	}
	public void OnUpdate(UIManagementEquip _parent, RuntimeData.EquipItemInfo info)
	{
		gameObject.SetActive(true);
		parent = _parent;
		itemInfo = info;

		textMeshProName.text = PlatformManager.Language[itemInfo.rawData.name];
		OnUpdateInfo();


	}
	public void OnUpdateInfo()
	{
		uiEquipSlot.OnUpdate(null, itemInfo, null);
		UpdateItemLevelupInfo();
		var currencyitem = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.UPGRADE_ITEM);
		IdleNumber value = itemInfo.LevelUpNeedCount();



		bool isMax = itemInfo.IsMaxLevel();
		if (isMax)
		{
			bool isBreakMax = itemInfo.IsMaxBreakThrough();
			buttonMax.SetActive(isBreakMax);
			buttonUpgrade.gameObject.SetActive(false);

			buttonNeedBreak.SetActive(isBreakMax == false);
		}
		else
		{
			buttonUpgrade.SetButton(currencyitem.type, currencyitem.IconImage, $"{currencyitem.Value.ToString()}/{value.ToString()}", value <= currencyitem.Value);
			buttonUpgrade.gameObject.SetActive(true);
			buttonMax.SetActive(false);
			buttonNeedBreak.SetActive(false);
		}
	}

	public void UpdateItemLevelupInfo()
	{
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < itemInfo.equipAbilities.Count; i++)
		{
			string tail = itemInfo.equipAbilities[i].tailChar;
			IdleNumber nextValue = itemInfo.equipAbilities[i].GetNextValue(itemInfo.Level + 1);
			sb.Append($"{itemInfo.equipAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{itemInfo.equipAbilities[i].GetValue(itemInfo.Level).ToString()}{tail}</color><color=green> > {nextValue.ToString()}{tail}</color>");
			sb.Append('\n');


		}
		textEquipBuff.text = sb.ToString();


		for (int i = 0; i < textOwnedBuff.Length; i++)
		{
			sb.Clear();
			textOwnedBuff[i].text = "";
			textOwnedBuff[i].gameObject.SetActive(false);
			if (i < itemInfo.ownedAbilities.Count)
			{
				string tail = itemInfo.ownedAbilities[i].tailChar;
				IdleNumber nextValue = itemInfo.ownedAbilities[i].GetNextValue(itemInfo.Level + 1);
				sb.Append($"{itemInfo.ownedAbilities[i].type.ToUIString()}");
				sb.Append($" <color=yellow>{itemInfo.ownedAbilities[i].GetValue(itemInfo.Level).ToString()}{tail}</color><color=green> > {nextValue.ToString()}{tail}</color>");
				sb.Append('\n');
				textOwnedBuff[i].gameObject.SetActive(true);
				textOwnedBuff[i].text = sb.ToString();
			}
		}


	}
	public bool OnClickLevelUp()
	{
		if (ItemLevelupable() == false)
		{
			return false;
		}

		var currencyitem = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.UPGRADE_ITEM);

		if (currencyitem.Pay((IdleNumber)itemInfo.LevelUpNeedCount()) == false)
		{
			ToastUI.Instance.Enqueue("강화석이 부족합니다");
			return false;
		}

		PlatformManager.UserDB.equipContainer.LevelUpEquipItem(ref itemInfo);

		parent.OnUpdateEquip(itemInfo.type, itemInfo.Tid);
		OnUpdateInfo();
		return true;
	}

	public bool ItemLevelupable()
	{
		if (itemInfo.unlock == false)
		{
			return false;
		}
		if (itemInfo.IsMaxLevel())
		{
			ToastUI.Instance.Enqueue("최대 레벨입니다.");
			return false;
		}
		var currencyitem = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.UPGRADE_ITEM);

		if (currencyitem == null)
		{
			return false;
		}

		if (currencyitem.Check(itemInfo.LevelUpNeedCount()) == false)
		{
			ToastUI.Instance.Enqueue("강화석이 부족합니다");
			return false;
		}

		return true;
	}

}
