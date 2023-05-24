using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyItemObject : ItemObject
{
	public CurrencyType currencyType;
	public IdleNumber maxValue;

	public void SetBasicData(CurrencyData data)
	{
		tid = data.tid;
		name = data.name;
		description = data.description;
		maxValue = (IdleNumber)data.maxValue;

	}

}
