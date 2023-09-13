using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RuntimeData;
using System.Net.Http.Headers;

public class UICostumeInfo : MonoBehaviour
{
	[SerializeField] private UICostumeManagement parentUI;

	[SerializeField] private TextMeshProUGUI itemName;
	[SerializeField] private TextMeshProUGUI textPoint;
	[SerializeField] private TextMeshProUGUI textGetPlace;

	[SerializeField] private TextMeshProUGUI textTotalPoints;

	[SerializeField] private UIEconomyButton upgradeButton;

	[SerializeField] private Button equipButton;
	[SerializeField] private Button equippedButton;
	public Button EquipButton => equipButton;
	[SerializeField] private TextMeshProUGUI equipButtonLabel;

	private RuntimeData.CostumeInfo costumeInfo;

	public void OnEnable()
	{
		equipButton.SetButtonEvent(OnClickEquip);
		upgradeButton.onlyOnce = true;
		upgradeButton.SetButtonEvent(OnClickBuy);
	}

	private void OnClickEquip()
	{
		if (costumeInfo.unlock == false)
		{
			return;
		}

		var data = PlatformManager.UserDB.costumeContainer[costumeInfo.Type];

		if (data.itemTid == costumeInfo.Tid)
		{
			ToastUI.Instance.Enqueue("장착 중입니다");
			return;
		}

		PlatformManager.UserDB.costumeContainer.Equip(parentUI.selectedItemTid, parentUI.costumeType);
		if (UnitManager.it.Player != null)
		{
			UnitManager.it.Player.ChangeCostume();
		}
		parentUI.OnUpdate(false);
		if (costumeInfo.Type == CostumeType.CHARACTER)
		{
			PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.EQUIP_COSTUME, costumeInfo.Tid, (IdleNumber)1);
		}
		else if (costumeInfo.Type == CostumeType.HYPER)
		{
			PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.EQUIP_COSTUME_HYPER, costumeInfo.Tid, (IdleNumber)1);
		}
	}

	private bool OnClickBuy()
	{
		if (costumeInfo.Cost.currency == CurrencyType.NONE)
		{
			return false;
		}
		if (costumeInfo.Cost.currency == CurrencyType.FREE)
		{
			PlatformManager.UserDB.costumeContainer.Buy(costumeInfo.Tid);
			parentUI.OnUpdate(false);
			return false;
		}

		if (costumeInfo.Cost.currency == CurrencyType.CASH)
		{
			PlatformManager.UserDB.costumeContainer.Buy(costumeInfo.Tid);
			parentUI.OnUpdate(false);
			return false;

		}


		var currencyItem = PlatformManager.UserDB.inventory.FindCurrency(costumeInfo.Cost.currency);
		if (currencyItem == null)
		{
			return false;


		}

		if (currencyItem.Pay(costumeInfo.Price) == false)
		{
			return false;

		}

		PlatformManager.UserDB.costumeContainer.Buy(costumeInfo.Tid);
		parentUI.OnUpdate(false);
		return true;

	}

	public void OnUpdate(RuntimeData.CostumeInfo info)
	{
		costumeInfo = info;
		itemName.text = costumeInfo.ItemName;
		textPoint.text = costumeInfo.rawData.point.ToString();
		textTotalPoints.text = PlatformManager.UserDB.costumeContainer.TotalCostumePoints.ToString();
		textGetPlace.text = PlatformManager.Language[costumeInfo.rawData.acquiredMessage];

		Sprite icon = info.Currency != null ? info.Currency.ItemIcon : null;
		var currencyItem = PlatformManager.UserDB.inventory.FindCurrency(info.Cost.currency);
		if (currencyItem != null)
		{
			upgradeButton.SetButton(info.Cost.currency, icon, $"{currencyItem.Value.ToString()}/{info.Price.ToString()}", currencyItem.Value >= info.Price);
		}
		else
		{
			upgradeButton.SetButton(info.Cost.currency, icon, info.Price.ToString());
		}

		equipButton.interactable = costumeInfo.unlock;

		upgradeButton.gameObject.SetActive(costumeInfo.unlock == false);

		var data = PlatformManager.UserDB.costumeContainer[costumeInfo.Type];

		if (data.itemTid == costumeInfo.Tid)
		{
			equippedButton.gameObject.SetActive(costumeInfo.unlock);
			equipButton.gameObject.SetActive(false);
		}
		else
		{
			equippedButton.gameObject.SetActive(false);
			equipButton.gameObject.SetActive(costumeInfo.unlock);
		}

	}
}
