using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public class YouthOptionProbData
{
	public Grade grade;
	public float probability;

}

[System.Serializable]
public class YouthOptionPercentageData
{
	public Grade grade;
	public float percentage;
}


[System.Serializable]
public class YouthOptionData
{
	public StatsType type;
	public StatModeType modeType;

	public float min;
	public float max;

	public YouthOptionPercentageData[] percentageData;
}

[System.Serializable]
public class YouthBuffData
{
	public string name;
	public ItemStats buff;
}
[System.Serializable]
public class HyperBuffData
{
	public string name;
	public Grade grade;
	public int levelLimit;
	public List<ItemStats> buffList;

}

[System.Serializable]
public class YouthData : BaseData
{
	public HyperBuffData[] hyperBuffs;
	public YouthBuffData[] buffList;
	public YouthOptionData[] optionDatas;
	public YouthOptionProbData[] probabilityDatas;
}

[System.Serializable]
public class YouthDataSheet : DataSheetBase<YouthData>
{

}
