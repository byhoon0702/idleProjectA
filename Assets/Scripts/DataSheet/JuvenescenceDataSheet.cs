using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum JuvenescenceType
{
	Youth,
	Skillful,
}

[System.Serializable]
public class JuvenescenceElement
{
	public JuvenescenceType type;
	public List<ItemStats> stats;
	public int maxPoint;
	public int maxLevel;
	public Cost cost;
	public int minLevelUp;
	public int maxLevelUp;

}

[System.Serializable]
public class JuvenescenceData : BaseData
{
	public string name;
	public int level;
	public int maxPoint;
	public RequirementInfo requirement;
	public List<JuvenescenceElement> elements;

}

[System.Serializable]
public class JuvenescenceDataSheet : DataSheetBase<JuvenescenceData>
{

}
