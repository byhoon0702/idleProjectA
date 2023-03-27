using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class MetaStageMapEnemyInfo
{
	public long enemyUnitTid;
}

[Serializable]
public class StageMapData : BaseData
{
	public string name;
	public long bgTid;
	/// <summary>
	/// 지역 인덱스(UI용)
	/// </summary>
	public int areaIndex;
	public List<MetaStageMapEnemyInfo> spawnEnemies;
}


[System.Serializable]
public class StageMapDataSheet : DataSheetBase<StageMapData>
{
}
