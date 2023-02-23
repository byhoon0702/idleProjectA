using System;
using System.Collections.Generic;
[System.Serializable]
public class StatsValue
{
	public Stats type;
	public string value;
	public string perLevel;
}

[System.Serializable]
public class EquipitemData : BaseData
{
	public string name;
	public string itemDesc;
	public ItemType itemType;
	public string hashTag;
	public Grade itemGrade;
	public int star;
	public List<StatsValue> equipValues;
	public List<StatsValue> ownValues;
}

[System.Serializable]
public class EquipItemDataSheet : DataSheetBase<EquipitemData>
{

}
