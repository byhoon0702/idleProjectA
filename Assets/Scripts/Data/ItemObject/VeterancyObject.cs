using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class VeterancyObject : ItemObject
{
	[SerializeField] private StatsType type;

	public override void SetBasicData<T>(T data)
	{

		VeterancyData vetData = data as VeterancyData;
		tid = vetData.tid;
		itemName = vetData.name;
		description = vetData.description;
		type = vetData.buff.type;
	}


}
