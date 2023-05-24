using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StatusData : BaseData
{
	public string name;
	public Ability type;
	public string uiName;

	[SerializeField] private string minValue;
	[SerializeField] private string maxValue;

	public bool isPercentage;

	public StatusData()
	{
		type = Ability._NONE;
		minValue = "1";
		maxValue = "999ZZ";
		isPercentage = false;
	}
	public IdleNumber MinValue()
	{
		return (IdleNumber)minValue;
	}
	public IdleNumber MaxValue()
	{
		return (IdleNumber)maxValue;
	}


	public StatusData Clone()
	{
		StatusData clone = new StatusData();
		clone.tid = tid;
		clone.description = description;
		clone.type = type;
		clone.name = name;
		clone.uiName = uiName;

		clone.minValue = minValue;
		clone.maxValue = maxValue;
		clone.isPercentage = isPercentage;
		return clone;
	}
}
[System.Serializable]
public class StatusDataSheet : DataSheetBase<StatusData>
{
	public StatusData GetData(long _tid)
	{
		if (_tid == 0)
		{
			return null;
		}
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == _tid)
			{
				return infos[i].Clone();
			}
		}
		return null;
	}
	public StatusData GetData(Ability _type)
	{
		if (_type == Ability._NONE)
		{
			return null;
		}


		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].type == _type)
			{
				return infos[i].Clone();
			}
		}
		return null;
	}
}
