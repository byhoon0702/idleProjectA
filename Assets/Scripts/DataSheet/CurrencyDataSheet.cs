using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum CurrencyType
{
	NONE = 0,
	GOLD = 1 << 0,
	DIA = 1 << 1,

	LEVELUP_ITEM = 1 << 50,
	SKILL_POINT = 1 << 100,
}


[System.Serializable]
public class CurrencyData : BaseData
{
	public string name;
	public CurrencyType type;
	public string maxValue;
}

[System.Serializable]
public class CurrencyDataSheet : DataSheetBase<CurrencyData>
{

}
