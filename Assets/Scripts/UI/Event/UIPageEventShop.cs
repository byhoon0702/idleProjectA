using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPageEventShop : UIShopBase<RuntimeData.EventShopInfo>
{
	[SerializeField] private Image imageCurrency;
	[SerializeField] private TextMeshProUGUI textCurrency;
	[SerializeField] private UIPopupEventBuyItem popupBuyNormalItem;
	public override void ShowBuyPopup(RuntimeData.EventShopInfo info)
	{
		popupBuyNormalItem.Activate();
		popupBuyNormalItem.OnUpdate(this, info);
	}
	public override void OnUpdate(ShopType type)
	{

		currentType = type;
		var eventInfo = PlatformManager.UserDB.eventContainer.GetCurrentEvent();
		if (eventInfo != null)
		{
			infoList = eventInfo.Get(currentType);
		}
		else
		{
			infoList = new List<RuntimeData.EventShopInfo>();
		}

		var currency = PlatformManager.UserDB.inventory.FindCurrency(eventInfo.rawData.currencyTid);
		imageCurrency.sprite = currency.IconImage;
		textCurrency.text = currency.Value.ToString();
		SetGrid();
	}
	public override void Refresh()
	{
		var eventInfo = PlatformManager.UserDB.eventContainer.GetCurrentEvent();
		if (eventInfo != null)
		{
			infoList = eventInfo.Get(currentType);
		}
		else
		{
			infoList = new List<RuntimeData.EventShopInfo>();
		}
		var currency = PlatformManager.UserDB.inventory.FindCurrency(eventInfo.rawData.currencyTid);
		if (currency != null)
		{
			imageCurrency.sprite = currency.IconImage;
			textCurrency.text = currency.Value.ToString();
		}
		SetGrid();
	}
	protected override void SetGrid()
	{
		content.CreateListCell(infoList.Count, itemPrefab, null);
		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < infoList.Count)
			{
				child.gameObject.SetActive(true);
				UIListCellEventShop normalItem = child.GetComponent<UIListCellEventShop>();
				normalItem.OnUpdate(this, infoList[i]);
			}
		}

	}

	private void OnDisable()
	{
		popupBuyNormalItem.Close();
	}
}
