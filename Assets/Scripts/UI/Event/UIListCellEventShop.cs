
using UnityEngine;
using TMPro;

public class UIListCellEventShop : UIShopItemBase<RuntimeData.EventShopInfo>
{

	[SerializeField] private TextMeshProUGUI textInfo;
	[SerializeField] private UIItemReward itemImage;
	private UIPageEventShop parent;
	public override void OnUpdate(UIShopBase<RuntimeData.EventShopInfo> _parent, RuntimeData.EventShopInfo _info)
	{
		base.OnUpdate(_parent, _info);
		parent = _parent as UIPageEventShop;
		int count = Mathf.Max(0, info.LimitCount - info.BuyCount);

		if (_info.LimitCount == 0)
		{
			textInfo.text = $"";
		}
		else
		{
			textInfo.text = $"{count}/{_info.LimitCount}";
		}

		if (limitOver)
		{
			textInfo.color = Color.red;
		}
		else
		{
			textInfo.color = Color.white;
		}

		if (info.rewardList.Count > 0)
		{
			var item = info.rewardList[0];

			itemImage.Set(new AddItemInfo(item));
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


		var item = PlatformManager.UserDB.inventory.GetPersistent(InventoryContainer.AdFreeTid);
		bool free = item.unlock;
		if (free)
		{
			info.OnPurchaseSuccess();
			parent.Refresh();
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
		if (limitOver)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_buy_limit_over"]);
			return;
		}
		if (info.IsUnlock() == false)
		{
			return;
		}
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
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_buy_limit_over"]);
			return;
		}
		var cost = PlatformManager.UserDB.inventory.FindCurrency(info.CurrencyType);
		if (cost.Value < info.Price)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_lack_of_currency"]);
			return;
		}


		parent.ShowBuyPopup(info);

	}
}
