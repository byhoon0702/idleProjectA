using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AdvancementData : BaseData
{

	public int normalStageNumber;
	/// <summary>
	/// 보상
	/// </summary>
	public long costumeTid;

	/// <summary>
	/// 승급던전 진입 재화
	/// </summary>
	public long currencyTid;
	public string currencyValue;

	public long battleTid;
	public int stageNumber;

	public List<ItemStats> stats;


}

[System.Serializable]
public class AdvancementDataSheet : DataSheetBase<AdvancementData>
{

}
