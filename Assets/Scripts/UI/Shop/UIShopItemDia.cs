using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShopItemDia : UIShopItemBase
{
	[SerializeField] private TextMeshProUGUI textBuyInfo;
	[SerializeField] private Image item;
	[SerializeField] private TextMeshProUGUI textItemCount;

	private UIShopDia parent;

	public override void OnClickAds()
	{

		if (info.IsUnlock() == false)
		{
			return;
		}
		if (limitOver)
		{
			return;
		}
		MobileAdsManager.Instance.ShowAds(() =>
		{
			info.OnPurchaseSuccess();
			parent.Refresh();
		});
	}

	public override void OnClickFree()
	{
		if (info.IsUnlock() == false)
		{
			return;
		}
		if (limitOver)
		{
			return;
		}

		info.OnPurchaseSuccess();
		parent.Refresh();
	}


	public override void OnClickCash()
	{
		if (limitOver)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_buy_limit_over"]);
			return;
		}
		if (info.IsUnlock() == false)
		{
			return;
		}
#if UNITY_EDITOR
		OnPurchaseCompleted();
		return;
#endif
		var product = PurchaseManager.Instance.GetProductID(info.rawData.productIDs);

		PurchaseManager.Instance.BuyProduct(product.id);

		PurchaseManager.Instance.PurchaseCompleted = OnPurchaseCompleted;
	}
	protected override void OnPurchaseCompleted()
	{
		info.OnPurchaseSuccess();
		parent.Refresh();
	}

	public override void OnClickItem()
	{
		if (limitOver)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_buy_limit_over"]);
			return;
		}
		if (info.IsUnlock() == false)
		{
			return;
		}
		var cost = PlatformManager.UserDB.inventory.FindCurrency(info.CurrencyType);
		if (cost.Check(info.Price) == false)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_lack_of_currency"]);
			return;
		}
		info.OnPurchaseSuccess();
		parent.Refresh();
	}

	public override void OnUpdate(UIShopBase _parent, RuntimeData.ShopInfo _info)
	{
		base.OnUpdate(_parent, _info);
		parent = _parent as UIShopDia;
		item.sprite = info.Icon;
		textBuyInfo.text = "";
		if (info.rewardList.Count > 0)
		{
			textItemCount.text = info.rewardList[0].fixedCount.ToString();
		}


	}
}
