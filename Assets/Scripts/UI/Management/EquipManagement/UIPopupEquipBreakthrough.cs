using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupEquipBreakthrough : UIBase
{

	[SerializeField] protected TextMeshProUGUI textMeshProName;
	[SerializeField] protected TextMeshProUGUI textLevelChange;

	[SerializeField] private UIEconomyButton economyButton;
	public UIEconomyButton EconomyButton => economyButton;

	[SerializeField] private UIEquipSlot uiEquipSlot;

	protected UIManagementEquip parent;
	protected RuntimeData.EquipItemInfo itemInfo;
	protected virtual void Awake()
	{
		economyButton.onlyOnce = true;
		economyButton.SetButtonEvent(OnClickLevelUp);
	}

	public void OnUpdate(UIManagementEquip _parent, RuntimeData.EquipItemInfo info)
	{
		Activate();
		parent = _parent;
		itemInfo = info;

		textMeshProName.text = PlatformManager.Language[itemInfo.rawData.name];
		OnUpdateInfo();
	}

	public void OnUpdateInfo()
	{
		uiEquipSlot.OnUpdate(null, itemInfo, null);

		CurrencyType type = CurrencyType.BREAKTHROUGHT_ITEM_D + itemInfo.BreakthroughLevel;

		var currencyitem = PlatformManager.UserDB.inventory.FindCurrency(type);

		IdleNumber value = (IdleNumber)100;
		economyButton.SetButton(type, currencyitem.IconImage, $"{currencyitem.Value.ToString()}/{value.ToString()}", currencyitem.Value >= value);
		economyButton.SetLabel(PlatformManager.Language["str_ui_breakthrough"]);

		bool isMax = itemInfo.IsMaxBreakThrough();
		if (isMax)
		{
			textLevelChange.text = $"최대 돌파 레벨";
		}
		else
		{
			var data = PlatformManager.CommonData.EquipBreakThroughList[itemInfo.BreakthroughLevel + 1];
			textLevelChange.text = $"<color=red>LV {itemInfo.MaxLevel}</color> -> <color=green>LV {data.maxLevel}</color>";
		}

		if (value > currencyitem.Value)
		{
			economyButton.SetInteractable(false);

		}
		else
		{
			economyButton.SetInteractable(true);
		}
	}

	public bool OnClickLevelUp()
	{
		if (ItemLevelupable() == false)
		{
			return false;
		}
		itemInfo.BreakThrough();
		parent.OnUpdateEquip(itemInfo.type, itemInfo.Tid);
		Close();
		return true;
	}

	public bool ItemLevelupable()
	{
		if (itemInfo.unlock == false)
		{
			return false;
		}
		if (itemInfo.IsMaxBreakThrough())
		{
			ToastUI.Instance.Enqueue("최대 돌파 레벨입니다.");
			return false;
		}
		CurrencyType type = CurrencyType.BREAKTHROUGHT_ITEM_D + itemInfo.BreakthroughLevel;
		var currencyitem = PlatformManager.UserDB.inventory.FindCurrency(type);

		if (currencyitem == null)
		{
			return false;
		}

		if (currencyitem.Check((IdleNumber)100) == false)
		{
			ToastUI.Instance.Enqueue($"{PlatformManager.Language[currencyitem.rawData.name]} 아이템이 부족합니다.");
			return false;
		}

		return true;
	}
}
