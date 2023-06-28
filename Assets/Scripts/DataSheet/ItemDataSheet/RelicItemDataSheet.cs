using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RelicItemData : ItemData
{
	public int maxLevel;
	public ItemStats ownValue;

}


[System.Serializable]
public class RelicItemDataSheet : DataSheetBase<RelicItemData>
{
	public List<RelicItemData> GetList(Grade _grade)
	{
		List<RelicItemData> outData = new List<RelicItemData>();

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].itemGrade == _grade)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}
}
