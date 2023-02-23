using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PetDataSheet : DataSheetBase<PetData>
{
	public PetData GetData(long _tid)
	{
		if (_tid == 0)
		{
			return null;
		}
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == _tid)
			{
				return infos[i];
			}
		}
		return null;
	}
	public List<PetData> GetInfos()
	{
		List<PetData> copy = new List<PetData>(infos);

		return copy;
	}
}


[System.Flags]
public enum PetCategory
{
	Human = 1 << 0,
	Animal = 1 << 1,
	Another = 1 << 2,

	Sky = 1 << 10,
	Fire = 1 << 11,
	Water = 1 << 12,
	Ground = 1 << 13,
	Leaf = 1 << 14,
}
