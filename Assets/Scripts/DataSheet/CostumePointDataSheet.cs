using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CostumePointData : BaseData
{
	public int needPoint;
	public ItemStats rewardStats;

}


[System.Serializable]
public class CostumePointDataSheet : DataSheetBase<CostumePointData>
{

}
