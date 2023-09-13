using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradingPost : MonoBehaviour
{
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private Transform content;


	public void OnUpdate(UIPopupTradingPost parent, List<RuntimeData.TradeInfo> list)
	{

		content.CreateListCell(list.Count, itemPrefab);

		for (int i = 0; i < content.childCount; i++)
		{
			var child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < list.Count)
			{
				UIListCellItemTradingPost item = child.GetComponent<UIListCellItemTradingPost>();
				item.gameObject.SetActive(true);
				item.OnUpdate(parent, list[i]);
			}
		}

	}
}
