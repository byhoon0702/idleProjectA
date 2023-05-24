using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class VeterancyObject : ItemObject
{
	[SerializeField] private StatsType type;


	public void SetData(VeterancyData data)
	{
		tid = data.tid;
		itemName = data.name;
		description = data.description;
		type = data.buff.type;


	}

}
