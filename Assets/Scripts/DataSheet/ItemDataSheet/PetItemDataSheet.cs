using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PetItemData : ItemData
{
	public long petTid;
	public List<ItemStats> ownValues;

	public PetItemData()
	{

	}




}

[Serializable]
public class PetItemDataSheet : DataSheetBase<PetItemData>
{
	public List<PetItemData> GetList(Grade _grade)
	{
		List<PetItemData> outData = new List<PetItemData>();

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
