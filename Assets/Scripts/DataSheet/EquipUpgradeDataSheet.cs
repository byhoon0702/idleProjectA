using System;
using System.Collections.Generic;

[Serializable]
public class EquipUpgradeData : BaseData
{
	public Int32 level;
	public Int32 needCount;
}

[Serializable]
public class EquipUpgradeDataSheet : DataSheetBase<EquipUpgradeData>
{
	/// <summary>
	/// 현재레벨에 맞는 레벨 데이터를 가져옴(모든 레벨 데이터가 1:1로 매칭되지 않음)
	/// </summary>
	public EquipUpgradeData FindLevelInfo(Int32 _level)
	{
		EquipUpgradeData outLevelData = null;

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
		var sheet = DataManager.Get<EquipUpgradeDataSheet>();
		var levelData = sheet.FindLevelInfo(1);

		return levelData.needCount;
	}
}
