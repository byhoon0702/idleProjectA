using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupTradingPost : UIBase
{
	[SerializeField] private UIPopupTradingList uiPopupTradingList;
	[SerializeField] private TradingPost tradingPost;
	protected override void OnActivate()
	{
		var list = PlatformManager.UserDB.tradeContainer.tradeList;
		tradingPost.OnUpdate(this, list);
	}

	public void ShowBuyPopup(RuntimeData.TradeInfo info)
	{
		uiPopupTradingList.Activate();
		uiPopupTradingList.OnUpdate(this, info);
	}
}
