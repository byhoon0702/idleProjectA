using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIListCellItemTradingOutput : MonoBehaviour
{
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI textTitle;
	System.Action<int> action;

	RuntimeData.TradeItemInfo _input;

	RuntimeData.TradeItemInfo _tradeItem;
	int count = 0;
	public void OnUpdate(RuntimeData.TradeItemInfo inputItem, RuntimeData.TradeItemInfo tradeItem, System.Action<int> callback = null)
	{
		_input = inputItem;
		_tradeItem = tradeItem;
		icon.sprite = tradeItem.itemInfo.IconImage;
		textTitle.text = tradeItem.itemInfo.ItemName;
	}

	public void OnClickNext()
	{
		if (_input.itemInfo.Check(_tradeItem.itemInfo.Count) == false)
		{
			return;
		}
		count++;
		_input.itemInfo.CalculateCount(_tradeItem.itemInfo.Count, false);

	}

	public void OnClickPrev()
	{
		if (count == 0)
		{
			return;
		}
		count--;
		_input.itemInfo.CalculateCount(_tradeItem.itemInfo.Count, true);
	}
}
