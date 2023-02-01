using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompanionDataSheet : DataSheetBase<CompanionData>
{
	public CompanionData GetData(long _tid)
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
	public List<CompanionData> GetInfos()
	{
		List<CompanionData> copy = new List<CompanionData>(infos);

		return copy;
	}
}
