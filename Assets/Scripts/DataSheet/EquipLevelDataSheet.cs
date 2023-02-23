using System;
using System.Collections.Generic;

[Serializable]
public class EquipLevelData : BaseData
{
	public Int32 level;
	public Int32 needCount;
}

[Serializable]
public class EquipLevelDataSheet : DataSheetBase<EquipLevelData>
{
	public EquipLevelData Get(long tid)
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

	/// <summary>
	/// 현재레벨에 맞는 레벨 데이터를 가져옴(모든 레벨 데이터가 1:1로 매칭되지 않음)
	/// </summary>
	public EquipLevelData FindLevelInfo(Int32 _level)
	{
		EquipLevelData outLevelData = null;

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].level <= _level)
			{
				outLevelData = infos[i];
			}
			else
			{
				break;
			}
		}

		return outLevelData;
	}

	/// <summary>
	/// 레벨 1 -> 2레벨 필요 비용
	/// </summary>
	public int Level1NeedCount()
	{
		var sheet = DataManager.Get<EquipLevelDataSheet>();
		var levelData = sheet.FindLevelInfo(1);

		return levelData.needCount;
	}
}
