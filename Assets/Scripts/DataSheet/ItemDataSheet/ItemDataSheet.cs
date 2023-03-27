
using System;
using System.Collections.Generic;

[Serializable]
public class ItemData : BaseData
{
	public string name;
	public string itemDesc;
	public ItemType itemType;
	public Grade itemGrade;
	public int starLv;
	public string hashTag;

	public string Icon => tid.ToString();

	public virtual AbilityInfo EquipbilityInfo { get; }
	public virtual AbilityInfo OwnAbilityInfo { get; }

	public virtual List<AbilityInfo> EquipAbilityInfos() { return new List<AbilityInfo>(); }
	public virtual List<AbilityInfo> OwnAbilityInfos() { return new List<AbilityInfo>(); }
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
			if (itemdata.hashTag == _hashTag)
			{
				cachedTidList.Add(_hashTag, itemdata);
				return itemdata as T;
			}
		}
		return null;
	}
}

[Serializable]
public class ItemDataSheet : ItemDataBaseSheet<ItemData>
{

	public List<ItemData> GetByItemType(ItemType _itemType)
	{
		List<ItemData> outData = new List<ItemData>();

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].itemType == _itemType)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}
	public List<ItemData> GetByItemTypeAndGrade(ItemType _itemType, Grade _grade)
	{
		List<ItemData> outData = new List<ItemData>();

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].itemType == _itemType && infos[i].itemGrade == _grade)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}

	public ItemData GetByMasteryTid(long _masteryTid)
	{


		return null;
	}
}
