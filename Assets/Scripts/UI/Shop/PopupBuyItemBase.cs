using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupBuyItemBase<T> : UIBase where T : RuntimeData.ShopInfo
{
	[SerializeField] protected UIItemReward item;
	[SerializeField] protected TextMeshProUGUI textTitle;
	[SerializeField] protected TextMeshProUGUI textCount;
	[SerializeField] protected UIEconomyButton uiEconomyButton;
	[SerializeField] protected Slider slider;

	protected int buyCount;
	protected const int min = 1;
	protected int max;
	protected RuntimeData.RewardInfo reward;
	protected RuntimeData.CurrencyInfo currency;
	protected RuntimeData.ShopInfo _shopInfo;
	protected UIShopBase<T> _parent;
	protected IdleNumber _cost;
	public IdleNumber Cost
	{
		get => _cost;

		set
		{
			_cost = value * Mathf.Max(buyCount, 1);
		}
	}


	protected void Awake()
	{
		uiEconomyButton.onlyOnce = true;
		uiEconomyButton.SetButtonEvent(OnClickBuy);
	}
	public void OnUpdate(UIShopBase<T> parent, T shopInfo)
	{
		_parent = parent;
		buyCount = 1;
		_shopInfo = shopInfo;
		reward = null;
		if (shopInfo.rewardList.Count > 0)
		{
			reward = shopInfo.rewardList[0];
		}

		if (reward != null)
		{
			item.Set(new AddItemInfo(reward));
		}


		currency = PlatformManager.UserDB.inventory.FindCurrency(shopInfo.CurrencyType);
		if (_shopInfo.LimitCount == 0)
		{
			max = Mathf.FloorToInt(currency.Value / _shopInfo.Price);
		}
		else
		{
			max = Mathf.Min(Mathf.FloorToInt(currency.Value / _shopInfo.Price), _shopInfo.LimitCount - _shopInfo.BuyCount);
		}

		slider.wholeNumbers = true;
		slider.minValue = min;
		slider.maxValue = max;
		slider.value = min;
		buyCount = Mathf.FloorToInt(slider.value);


		textTitle.text = PlatformManager.Language[_shopInfo.rawData.name];
		textCount.text = $"x{buyCount}";

		OnUpdateButton();
	}

	public void OnValueChanged(float value)
	{
		buyCount = Mathf.FloorToInt(value);
		textCount.text = $"x{buyCount}";
		OnUpdateButton();
	}
	protected void OnUpdateButton()
	{
		Cost = _shopInfo.Price;
		uiEconomyButton.SetButton(currency.type, currency.IconImage, $"{currency.Value.ToString()}/{Cost.ToString()}", currency.Value >= Cost);
	}

	public bool OnClickBuy()
	{
		if (currency.Check(Cost) == false)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_lack_of_currency"]);
			return false;
		}

		_shopInfo.OnPurchaseSuccess(buyCount);
		_parent.Refresh();

		Close();
		return true;
	}

	public void OnClickPlus()
	{
		if (buyCount == max)
		{
			return;
		}

		slider.value += 1;
		OnUpdateButton();
	}

	public void OnClickMinus()
	{
		if (buyCount == min)
		{
			return;
		}
		slider.value -= 1;
		OnUpdateButton();
	}
}
