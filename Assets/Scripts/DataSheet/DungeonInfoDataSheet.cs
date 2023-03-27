using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DungeonInfoData : BaseData
{
	/// <summary>
	/// 던전 이름
	/// </summary>
	public string name;

	/// <summary>
	/// 필요 아이템 tid
	/// </summary>
	public long itemTidNeed;

	/// <summary>
	/// 필요 아이템 갯수
	/// </summary>
	public int itemCount;

	/// <summary>
	/// 던전 스테이지 타입
	/// </summary>
	public WaveType waveType;

	/// <summary>
	/// UI타입
	/// </summary>
	public DungeonInfoUiType uiType;

	public string MainImage => tid.ToString();
}

[Serializable]
public class DungeonInfoDataSheet : DataSheetBase<DungeonInfoData>
{
	public DungeonInfoData Get(long _tid)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == _tid)
			{
				return infos[i];
			}
		}

		return null;
	}

	public DungeonInfoData Get(WaveType _waveType)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].waveType == _waveType)
			{
				return infos[i];
			}
		}

		return null;
	}
}

public enum DungeonInfoUiType
{
	Hidden,
	Dungeon,
	Challenge,
	BossRush
}
