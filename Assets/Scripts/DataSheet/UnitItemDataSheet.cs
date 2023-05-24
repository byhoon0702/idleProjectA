using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitItemData : ItemData
{
	public long unitTid;
	public List<ItemStats> equipValues;
	public List<ItemStats> ownValues;
	public UnitItemData()
	{

	}
}

[Serializable]
public class UnitItemDataSheet : DataSheetBase<UnitItemData>
{
	private Dictionary<string/* hashtag */, UnitItemData> cachedTidList = new Dictionary<string, UnitItemData>();


	public UnitItemData GetByHashTag(string _hashTag)
	{
		if (cachedTidList.TryGetValue(_hashTag, out var data))
		{
			return data;
		}

		//for (int i = 0; i < infos.Count; i++)
		//{
		//	if (infos[i].hashTag == _hashTag)
		//	{
		//		cachedTidList.Add(_hashTag, infos[i]);
		//		return infos[i];
		//	}
		//}

		return null;
	}

}
