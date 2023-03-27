
using System;

[Serializable]
public class ItemRefillData : BaseData
{
	/// <summary>
	/// 기본 보유량
	/// </summary>
	public int defaultAmount;

	/// <summary>
	/// 최대 소지량
	/// </summary>
	public int systemMaximumAmount;

	// refill
	public bool refill_isUse;
	public int refill_intervalMinutes;
	public int refill_amount;

	// reset
	public bool reset_isUse;
	public int reset_setHour;
	public int reset_setMinute;
	public int reset_amount;
}

[Serializable]
public class ItemRefillDataSheet : DataSheetBase<ItemRefillData>
{

}
