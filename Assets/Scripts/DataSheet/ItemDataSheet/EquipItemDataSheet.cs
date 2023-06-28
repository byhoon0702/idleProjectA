using System;
using System.Collections.Generic;

[System.Serializable]
public struct ItemStats
{
	public StatsType type;
	public StatModeType modeType;

	public string value;
	public string perLevel;
	public bool isPercentage;
	public bool isHyper;

}

[System.Serializable]
public class EquipItemData : ItemData
{
	public int starLevel;

	public EquipType equipType;
	public List<ItemStats> equipValues;
	public List<ItemStats> ownValues;


}
public class ItemDataBaseSheet<T> : DataSheetBase<T> where T : BaseData, new()
{
	protected Dictionary<string/* hashtag */, ItemData> cachedTidList = new Dictionary<string, ItemData>();

	public T GetInfo(long tid)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == tid)
			{
				return infos[i];
			}
		}

		return null;
	}

	public override T Get(string _hashTag)
	{
		if (cachedTidList.TryGetValue(_hashTag, out var data))
		{
			return data as T;
		}

		for (int i = 0; i < infos.Count; i++)
		{
			ItemData itemdata = (infos[i] as ItemData);
			if (itemdata == null)
			{
				continue;
			}
			//if (itemdata.hashTag == _hashTag)
			//{
			//	cachedTidList.Add(_hashTag, itemdata);
			//	return itemdata as T;
			//}
		}
		return null;
	}
}

[System.Serializable]
public class EquipItemDataSheet : ItemDataBaseSheet<EquipItemData>
{

	public List<EquipItemData> GetByItemType(EquipType _itemType)
	{
		List<EquipItemData> outData = new List<EquipItemData>();

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].equipType == _itemType)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}



}
