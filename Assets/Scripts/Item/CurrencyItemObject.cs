using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyItemObject : ItemObject
{
	public CurrencyType currencyType;
	public IdleNumber maxValue;

	public override void SetBasicData<T>(T data)
	{
		var currencyData = data as CurrencyData;
		tid = data.tid;
		itemName = data.name;
		description = data.description;
		maxValue = (IdleNumber)currencyData.maxValue;
		currencyType = currencyData.type;
	}

}
