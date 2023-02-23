using System;
using System.Collections.Generic;

[Serializable]
public class UserTrainingData : BaseData
{
	public string name;

	/// <summary>
	/// 최대레벨
	/// </summary>
	public Int32 maxLevel;

	/// <summary>
	/// 골드 기본소모량
	/// </summary>
	public long consumeDefaultGold;
	/// <summary>
	/// 골드 상승폭
	/// </summary>
	public double consumeGoldIncrease;
	/// <summary>
	/// 가중치
	/// </summary>
	public double consumeWeight;

	/// <summary>
	/// 선행 어빌리티
	/// </summary>
	public long precedingAbilityTid;

	/// <summary>
	/// 선행어벨리티 레벨
	/// </summary>
	public int precedingLevel;
}

[Serializable]
public class UserTrainingDataSheet : DataSheetBase<UserTrainingData>
{
	public UserTrainingData Get(long tid)
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

	public IdleNumber LevelupConsume(long _userTrainingTid, Int32 _level)
	{
		// ({기본 소모량}+(({현재 레벨}-1) * {상승폭})) * (1+({가중치} * {레벨}))
		var data = DataManager.Get<UserTrainingDataSheet>().Get(_userTrainingTid);


		long result = (long)((data.consumeDefaultGold + ((_level - 1) * data.consumeGoldIncrease)) * (1 + (data.consumeWeight * _level)));

		return new IdleNumber(result);
	}
}
