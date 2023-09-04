using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[System.Serializable]
public class TrainingData : BaseData
{


	public ItemStats buff;

	public int maxLevel;
	public StatsType preconditionType;
	public long preconditionLevel;

	public long basicCost;
	public float basicCostInc;
	public float basicCostWeight;
}

[System.Serializable]
public class TrainingDataSheet : DataSheetBase<TrainingData>
{

}
