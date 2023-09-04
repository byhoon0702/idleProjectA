using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdsReward
{
	public CurrencyType type;
	public string value;
	public bool isFixed;
}

[System.Serializable]
public class AdsRewardChestData : BaseData
{
	public AdsReward reward;
	public int dailyViewCount;
	public float appearMinTime;
	public float appearMaxTime;
}

[System.Serializable]
public class AdsRewardChestDataSheet : DataSheetBase<AdsRewardChestData>
{

}
