using System;
using System.Collections.Generic;

[Serializable]
public class ItemRelicData : BaseData
{
	public Int32 maxLevel;

	/// <summary>
	/// 시작 성공률
	/// </summary>
	public float startUpgradeRatio;
	/// <summary>
	/// 레벨당 성공률 감소량
	/// </summary>
	public float decreaseSuccessRatio;

	/// <summary>
	/// 소비재화개수
	/// </summary>
	public Int32 consumePoint;
}

[Serializable]
public class ItemRelicDataSheet : DataSheetBase<ItemRelicData>
{
	private List<AbilityType> abilities;


	public ItemRelicData Get(long tid)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].tid == tid)
			{
				return infos[i];
			}
		}
		return null;
	}
}
