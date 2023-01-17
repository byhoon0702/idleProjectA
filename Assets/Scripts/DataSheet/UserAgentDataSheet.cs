using System;
using System.Collections.Generic;

[Serializable]
public class UserAgentData : BaseData
{
	public Int32 level;
	public Int32 needUserLevel;
	public string consumeGold;

	[NonSerialized] private IdleNumber _consumeGoldCount;
	public IdleNumber consumeGoldCount
	{
		get
		{
			if(_consumeGoldCount == null)
			{
				_consumeGoldCount = (IdleNumber)consumeGold;
			}

			return _consumeGoldCount;
		}
	}

	/// <summary>
	/// 배치 가능 수
	/// </summary>
	public Int32 unitCount;

	/// <summary>
	/// 골드 획득량
	/// </summary>
	public float goldUpValue;

	/// <summary>
	/// 경험치 획득량
	/// </summary>
	public float expUpValue;

	/// <summary>
	/// 아이템 획득확률
	/// </summary>
	public float itemUpValue;
}

[Serializable]
public class UserAgentDataSheet : DataSheetBase<UserAgentData>
{
	public UserAgentData Get(long tid)
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

	public UserAgentData GetByLevel(long tid)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].level == tid)
			{
				return infos[i];
			}
		}

		return null;
	}
}
