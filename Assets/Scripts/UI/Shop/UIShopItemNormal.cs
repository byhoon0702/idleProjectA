using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class UIShopItemBase<T> : MonoBehaviour where T : RuntimeData.ShopInfo
{
	[SerializeField] protected Button buttonLock;
	[SerializeField] protected UITextMeshPro uiTextTitle;
	[SerializeField] protected Button buttonCash;
	[SerializeField] protected TextMeshProUGUI textPriceCash;

	[SerializeField] protected Button buttonItem;
	[SerializeField] protected Image imagePriceItem;
	[SerializeField] protected TextMeshProUGUI textPriceItem;

	[SerializeField] protected Button buttonFree;
	[SerializeField] protected Button buttonAds;

	protected T info;
	protected bool limitOver;
	protected string _lockMessage;
	protected CurrencyType currencyType;
	private void OnEnable()
	{

	}
	private void OnDisable()
	{

	}

	protected virtual void OnPurchaseCompleted()
	{

	}
	protected virtual void Awake()
	{
		buttonLock.SetButtonEvent(OnClickLock);

		buttonCash.SetButtonEvent(OnClickCash);

		buttonItem.SetButtonEvent(OnClickItem);

		buttonAds.SetButtonEvent(OnClickAds);

		buttonFree.SetButtonEvent(OnClickFree);
	}

	public virtual void OnUpdate(UIShopBase<T> _parent, T _info)
	{
		info = _info;

		bool isOpen = info.rawData.openData.openCondition.IsFulFillCondition(out _lockMessage);

		uiTextTitle.SetKey(info.rawData.name);

		currencyType = info.rawData.cost.currency;
		IdleNumber price = (IdleNumber)info.rawData.cost.cost;

		buttonCash.gameObject.SetActive(false);
		buttonItem.gameObject.SetActive(false);
		buttonAds.gameObject.SetActive(false);
		buttonFree.gameObject.SetActive(false);
		buttonLock.gameObject.SetActive(false);
		int left = info.LimitCount - info.BuyCount;

		limitOver = left <= 0 && info.LimitCount > 0;
		if (isOpen == false)
		{
			buttonLock.gameObject.SetActive(true);
			return;
		}

		if (limitOver)
		{
			buttonLock.gameObject.SetActive(true);
			return;
		}

		switch (currencyType)
		{
			case CurrencyType.CASH:
				buttonCash.gameObject.SetActive(true);

				var product = PurchaseManager.Instance.GetProduct(info.rawData.productIDs);
				if (product != null)
				{

					textPriceCash.text = product.metadata.localizedPriceString;
				}
				else
				{
					textPriceCash.text = info.rawData.cost.cost;
				}
				break;
			case CurrencyType.ADS:
				{
					buttonAds.gameObject.SetActive(true);
				}
				break;
			default:

				if (currencyType == CurrencyType.FREE)
				{
					imagePriceItem.enabled = false;
					buttonFree.gameObject.SetActive(true);
				}
				else
				{
					buttonItem.gameObject.SetActive(true);
					var item = PlatformManager.UserDB.inventory.FindCurrency(currencyType);
					if (item == null)
					{
						imagePriceItem.enabled = false;
					}
					else
					{
						imagePriceItem.enabled = true;
						imagePriceItem.sprite = item.IconImage;
					}

					textPriceItem.text = price.ToString();
					if (price > item.Value)
					{
						textPriceItem.color = Color.red;
					}
					else
					{
						textPriceItem.color = Color.white;
					}

				}
				break;
		}
	}

	public virtual void OnReceivedItem()
	{
		info.OnPurchaseSuccess();

	}

	public abstract void OnClickCash();
	public abstract void OnClickItem();
	public abstract void OnClickAds();
	public abstract void OnClickFree();
	public void OnClickLock()
	{
		if (limitOver)
		{
			if (currencyType == CurrencyType.ADS)
			{
				ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_no_more_buy_ads"]);
			}
			else
			{
				ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_no_more_buy"]);
			}

			return;
		}
		ToastUI.Instance.Enqueue(_lockMessage);

	}
}


public class UIShopItemNormal : UIShopItemBase<RuntimeData.ShopInfo>
{

	[SerializeField] private TextMeshProUGUI textInfo;
	[SerializeField] private UIItemReward itemImage;
	private UIShopNormal parent;
	public override void OnUpdate(UIShopBase<RuntimeData.ShopInfo> _parent, RuntimeData.ShopInfo _info)
	{
		base.OnUpdate(_parent, _info);
		parent = _parent as UIShopNormal;
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
