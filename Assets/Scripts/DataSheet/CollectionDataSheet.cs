using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectionTab
{
	EQUIP,
	PET,
	SKILL,

}

[System.Serializable]
public class CollectionItem
{
	public RewardCategory category;
	public long tid;
}

[System.Serializable]
public class CollectionCondition
{
	public int level;
	public ItemStats reward;
}

[System.Serializable]
public class CollectionData : BaseData
{

	public CollectionTab collectionTab;
	public List<CollectionItem> itemList;
	public List<CollectionCondition> rewards;
}

[System.Serializable]
public class CollectionDataSheet : DataSheetBase<CollectionData>
{
	public List<CollectionData> GetByType(CollectionTab tab)
	{
		return infos.FindAll(x => x.collectionTab == tab);

	}
}
