using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StageInfo : BaseData
{
	public StageType stageType;

	public string areaName;
	public int act;
	public int stage;

	public int bgTid;

	public int waveCount;
	public int waveUnitCount;
	public List<int> enemyTidList;
	public int bossTid;

	public StageInfo Clone()
	{
		StageInfo info = new StageInfo();

		info.stageType = stageType;

		info.areaName = areaName;
		info.act = act;
		info.stage = stage;

		info.bgTid = bgTid;

		info.waveCount = waveCount;
		info.waveUnitCount = waveUnitCount;
		info.enemyTidList = new List<int>(enemyTidList);
		info.bossTid = bossTid;

		return info;
	}
}

[Serializable]
public class StageWaveData
{
	public List<WaveRawData> waveList;
}

[Serializable]
public class WaveRawData
{
	public long tid;
	public int count;
}

public enum StageType
{
	NONE,
	NORMAL
}
