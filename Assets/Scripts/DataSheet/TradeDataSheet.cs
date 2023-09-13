using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TradeItem
{
	public RewardCategory category;
	public long tid;
	public string count;
}

[System.Serializable]
public class TradeData : BaseData
{
	public TimeLimitType limitType;
	public int limitCount;

	public TradeItem inputItem;
	public List<TradeItem> outputItem;

	public RequirementInfo requirement;
}

[System.Serializable]
public class TradeDataSheet : DataSheetBase<TradeData>
{

}
