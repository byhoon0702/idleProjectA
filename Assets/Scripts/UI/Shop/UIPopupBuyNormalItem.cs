using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupBuyNormalItem : UIBase
{
	[SerializeField] private UIItemReward item;
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textCount;
	[SerializeField] private UIEconomyButton uiEconomyButton;
	[SerializeField] Slider slider;

	private int buyCount;
	private const int min = 1;
	private int max;
	private RuntimeData.RewardInfo reward;
	private RuntimeData.CurrencyInfo currency;
	RuntimeData.ShopInfo _shopInfo;
	private UIShopBase _parent;
	private IdleNumber _cost;
	public IdleNumber Cost
	{
		get => _cost;

		set
		{
			_cost = value * Mathf.Max(buyCount, 1);
		}
	}


	private void Awake()
	{
		uiEconomyButton.onlyOnce = true;
		uiEconomyButton.SetButtonEvent(OnClickBuy);
	}
	public void OnUpdate(UIShopBase parent, RuntimeData.ShopInfo shopInfo)
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
		max = Mathf.Min(Mathf.FloorToInt(currency.Value / _shopInfo.Price), _shopInfo.LimitCount);
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
	private void OnUpdateButton()
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
