using System;
using System.Collections.Generic;

[Serializable]
public class UserPromoteData : BaseData
{
	public Int32 level;
	public string consumeLightDust;

	[NonSerialized] private IdleNumber _consumeLightDustCount;
	public IdleNumber consumeLightDustCount
	{
		get
		{
			if (_consumeLightDustCount == null)
			{
				_consumeLightDustCount = (IdleNumber)consumeLightDust;
			}

			return _consumeLightDustCount;
		}
	}
}

[Serializable]
public class UserPromoteDataSheet : DataSheetBase<UserPromoteData>
{
	public UserPromoteData Get(long tid)
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

	public UserPromoteData GetByLevel(long tid)
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
