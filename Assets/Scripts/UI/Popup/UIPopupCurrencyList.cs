using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupCurrencyList : UIBase
{

	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;

	private void Awake()
	{
		EventCallbacks.onCurrencyChanged += SetGrid;
	}
	private void OnDestroy()
	{
		EventCallbacks.onCurrencyChanged -= SetGrid;
	}
	protected override void OnActivate()
	{
		SetGrid(CurrencyType.NONE);
	}

	void SetGrid(CurrencyType type)
	{
		var list = PlatformManager.UserDB.inventory.currencyList;

		content.CreateListCell(list.Count, itemPrefab);


		for (int i = 0; i < content.childCount; i++)
		{
			Transform trans = content.GetChild(i);
			trans.gameObject.SetActive(false);
			if (i < list.Count)
			{
				if (list[i].rawData.hideUI)
				{
					continue;
				}
				UIListCellCurrency cell = trans.GetComponent<UIListCellCurrency>();
				cell.OnUpdate(list[i]);
				trans.gameObject.SetActive(true);
			}
		}

	}
}
