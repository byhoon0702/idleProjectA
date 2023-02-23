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

	public List<EnemyInfo> enemyInfos;

	public int bossTid;
	public int bossLevel;

	public List<StageRewardInfo> stageRewardInfoList;
	public List<StageRewardInfo> bossRewardInfoList;

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

		info.enemyInfos = new List<EnemyInfo>(enemyInfos);

		info.bossTid = bossTid;
		info.bossLevel = bossLevel;

		info.stageRewardInfoList = new List<StageRewardInfo>(stageRewardInfoList);
		info.bossRewardInfoList = new List<StageRewardInfo>(bossRewardInfoList);

		return info;
	}
}

[Serializable]
public class StageRewardInfo
{
	public long tid;
	public int count;
	public float dropRate;
}

[Serializable]
public class EnemyInfo
{
	public long tid;
	public int level;
}

public enum StageType
{
	NONE,

	/// <summary>
	/// 일반 스테이지
	/// </summary>
	NORMAL,

	/// <summary>
	/// 괴인추격전
	/// </summary>
	CHASING,

	/// <summary>
	/// 타락한 용사의 성
	/// </summary>
	TIMEATTACK,

	/// <summary>
	/// 마왕 침공
	/// </summary>
	BOSSRELAY
}
