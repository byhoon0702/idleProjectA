using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupTradingList : UIBase
{

	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;

	UIPopupTradingPost _parent;
	RuntimeData.TradeInfo _tradeInfo;
	public void OnUpdate(UIPopupTradingPost parent, RuntimeData.TradeInfo info)
	{
		_parent = parent;
		_tradeInfo = info;


		SetGrid();
	}
	void SetGrid()
	{
		var list = _tradeInfo.GetOutputItemInfo();
		content.CreateListCell(list.Count, itemPrefab);

		for (int i = 0; i < content.childCount; i++)
		{
			var child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < list.Count)
			{
				UIListCellItemTradingOutput item = child.GetComponent<UIListCellItemTradingOutput>();
				item.OnUpdate(_tradeInfo.GetInputItemInfo(), list[i]);
				child.gameObject.SetActive(true);
			}
		}
	}
}
