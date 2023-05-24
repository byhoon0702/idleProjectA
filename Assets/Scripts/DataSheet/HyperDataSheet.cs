﻿using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum HyperClass
{
	WARRIOR,
	MERCENARY_KING,
	KNIGHT_KING,
	SHARPSHOOTER,
}

public enum HyperRewardType
{
	NONE,
	COSTUME,
	SKILL,
	CURRENCY,
}

[System.Serializable]
public class HyperRewardInfo
{
	public int level;
	public HyperRewardType rewardType;
	public long rewardTid;
	public string value;
}



[System.Serializable]
public struct HyperClassData
{
	public int level;
	public int upgradeCount;
	public List<ItemStats> stats;

}

[System.Serializable]
public class HyperData : BaseData
{
	public string name;

	public HyperClass hyperClass;
	public int maxLevel;
	public Cost cost;
	public List<ItemStats> stats;
	public List<ItemStats> rewardStats;
	public List<HyperRewardInfo> rewardinfo;
}

[System.Serializable]
public class HyperDataSheet : DataSheetBase<HyperData>
{

}