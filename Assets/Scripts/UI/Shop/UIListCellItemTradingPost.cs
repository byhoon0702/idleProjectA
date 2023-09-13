using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIListCellItemTradingPost : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textInfo;
	[SerializeField] private UIItemReward itemImage;
	[SerializeField] private GameObject objButtonBuy;
	[SerializeField] private GameObject objButtonLock;

	private UIPopupTradingPost _parent;
	RuntimeData.TradeInfo _info;
	public void OnUpdate(UIPopupTradingPost parent, RuntimeData.TradeInfo info)
	{
		_parent = parent;
		_info = info;

		var inputItem = info.GetInputItemInfo();
		itemImage.Set(new AddItemInfo(inputItem.itemInfo.Tid, (IdleNumber)inputItem.itemInfo.Count, inputItem.rawData.category));
		textTitle.text = inputItem.itemInfo.ItemName;
		textInfo.text = "";

		//int count = Mathf.Max(0, info.LimitCount - info.BuyCount);

		//if (_info.LimitCount == 0)
		//{
		//	textInfo.text = $"";
		//}
		//else
		//{
		//	textInfo.text = $"{count}/{_info.LimitCount}";
		//}

		//if (limitOver)
		//{
		//	textInfo.color = Color.red;
		//}
		//else
		//{
		//	textInfo.color = Color.white;
		//}

		objButtonLock.gameObject.SetActive(false);
		objButtonBuy.gameObject.SetActive(true);
	}

	public void OnClickItem()
	{
		//if (info.IsUnlock() == false)
		//{
		//	return;
		//}
		//if (limitOver)
		//{
		//	ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_buy_limit_over"]);
		//	return;
		//}
		//var cost = PlatformManager.UserDB.inventory.FindCurrency(_info.CurrencyType);
		//if (cost.Value < _info.Price)
		//{
		//	ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_lack_of_currency"]);
		//	return;
		//}


		_parent.ShowBuyPopup(_info);

	}
}
