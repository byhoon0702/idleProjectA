using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class YouthBuffObject : ItemObject
{
	[SerializeField] private StatsType type;
	public StatsType Type => type;
	[SerializeField] private IdleNumber basicValue;
	public IdleNumber BasicValue => basicValue;
	[SerializeField] private IdleNumber perLevel;
	public IdleNumber PerLevel => perLevel;
	[SerializeField] private StatModeType modeType;
	public StatModeType ModeType => modeType;

	public void SetData(YouthBuffData data)
	{
		//tid = data.tid;
		itemName = data.name;
		//description = data.description;
		type = data.buff.type;
		basicValue = (IdleNumber)data.buff.value;
		perLevel = (IdleNumber)data.buff.perLevel;
		modeType = data.buff.modeType;
	}

}
