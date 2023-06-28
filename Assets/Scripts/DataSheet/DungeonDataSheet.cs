using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing.Extension;

public enum StageDifficulty
{
	NONE,
	EASY,
	NORMAL,
	HARD,
	NIGHTMARE,


}

public enum DungeonType
{
	Normal,
	Dungeon,
	Challenge,
}

[System.Serializable]
public class DungeonData : BaseData
{
	public string name;
	public DungeonType type;
	public StageType stageType;

	public long dungeonItemTid;
	public int itemCount;
}

[System.Serializable]
public class DungeonDataSheet : DataSheetBase<DungeonData>
{


}
