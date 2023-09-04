using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct AwakeningLevel
{
	public string Name;
	public int MaxLevel;
	public int BaseCost;
	public int IncPerLevel;

	public ItemStats Stat;
}

[System.Serializable]
public class AwakeningData : BaseData
{

	public long costitemTid;
	/// <summary>
	/// Unlock 보상용 코스튬
	/// </summary>
	public long costumeTid;
	//public long hyperTid;
	public string uiDescription;
	public ItemStats[] awakeningStats;
	public List<AwakeningLevel> awakeningLevels;

	public bool hideUI;
	public bool defaultGet;

}

[System.Serializable]
public class AwakeningDataSheet : DataSheetBase<AwakeningData>
{

}
