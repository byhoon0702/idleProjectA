using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[System.Serializable]
public class TrainingData : BaseData
{
	public string name;
	public Ability type;
	public string basicValue;
	public string perLevelValue;

	public int maxLevel;
	public Ability preconditionType;
	public long preconditionLevel;

	public long basicCost;
	public float basicCostInc;
	public float basicCostWeight;
}

[System.Serializable]
public class TrainingDataSheet : DataSheetBase<TrainingData>
{

}
