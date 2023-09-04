using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShopItemPackage : UIShopItemBase
{
	[SerializeField] private TextMeshProUGUI textBuyInfo;
	[SerializeField] private UITextMeshPro uiTextDescription;

	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;

	UIShopPackage parent;

	public override void OnUpdate(UIShopBase _parent, RuntimeData.ShopInfo _info)
	{
		base.OnUpdate(_parent, _info);
		parent = _parent as UIShopPackage;

		uiTextDescription.SetKey(info.rawData.description);

		int count = Mathf.Max(0, info.LimitCount - info.BuyCount);
		textBuyInfo.text = $"구입가능 횟수:{count}/{info.LimitCount}";
		if (count > 0)
		{
			textBuyInfo.color = Color.white;
		}
		else
		{
			textBuyInfo.color = Color.red;
		}

		SetGrid();
	}

	void SetGrid()
	{
		content.CreateListCell(info.rewardList.Count, itemPrefab, null);

		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < info.rewardList.Count)
			{
				UIItemReward item = child.GetComponent<UIItemReward>();
				item.Set(new AddItemInfo(info.rewardList[i]));

				child.gameObject.SetActive(true);
			}
		}
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


	public override void OnClickAds()
	{
		if (info.IsUnlock() == false)
		{
			return;
		}
		if (limitOver)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_buy_limit_over"]);
			return;
		}

		MobileAdsManager.Instance.ShowAds(() =>
		{
			info.OnPurchaseSuccess();
			parent.Refresh();
		});
	}

	public override void OnClickCash()
	{
		if (info.IsUnlock() == false)
		{
			return;
		}
		if (limitOver)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_buy_limit_over"]);
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
		if (info.IsUnlock() == false)
		{
			return;
		}
		if (limitOver)
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
}
