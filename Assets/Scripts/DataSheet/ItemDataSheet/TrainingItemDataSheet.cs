using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TrainingItemData : ItemData
{
	public StatsValue ownValue;
	public long userTrainingTid;

	[NonSerialized] private AbilityInfo ownAbilityInfo;
	public override AbilityInfo OwnAbilityInfo
	{
		get
		{
			if (ownAbilityInfo == null)
			{
				ownAbilityInfo = new AbilityInfo(ownValue.type, (IdleNumber)ownValue.value, (IdleNumber)ownValue.perLevel);



			}

			return ownAbilityInfo;
		}
	}


	public long basicCost;
	public float basicCostInc;
	public float basicCostWeight;

}

[System.Serializable]
public class TrainingItemDataSheet : DataSheetBase<TrainingItemData>
{

}

