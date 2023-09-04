using System.Collections;
using System.Collections.Generic;


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
public class HyperData : BaseData
{

	public long skillTid;
	public long finalSkillTid;

	public List<long> skillTidList;

	public int maxPhase;
}

[System.Serializable]
public class HyperDataSheet : DataSheetBase<HyperData>
{

}
