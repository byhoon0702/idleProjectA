using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CostumeData : ItemData
{
	public string acquiredMessage;
	public CostumeType costumeType;
	public int point;
	public long hyperTid;
	public Cost cost;
	public List<PlatformProductID> productIDs;
	public bool hideUI;
	public bool defaultGet;

}

[System.Serializable]
public class CostumeDataSheet : DataSheetBase<CostumeData>
{
	public List<CostumeData> GetByItemType(CostumeType _itemType)
	{
		List<CostumeData> outData = new List<CostumeData>();

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].costumeType == _itemType)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}
}
