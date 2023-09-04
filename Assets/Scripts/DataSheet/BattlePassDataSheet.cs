using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattlePassType
{
	LEVEL,
	STAGE,
	HUNTINNG,
}
[System.Serializable]
public class BattlePassTier
{

	public Reward freeReward;
	public Reward passReward;
}

[System.Serializable]
public struct TierCondition
{
	public RequirementInfo info;
	public int incValuePerTier;
}


[System.Serializable]
public class BattlePassData : BaseData
{
	public string subText;
	public long persistentItemTid;
	public long shopTid;
	public BattlePassType type;
	//public RequirementInfo openCondition;
	public TierCondition tierCondition;

	public BattlePassTier[] tierList;
	public bool canReset;
}

[System.Serializable]
public class BattlePassDataSheet : DataSheetBase<BattlePassData>
{

}
