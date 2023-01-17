
using System;

[Serializable]
public class UserAbilityInfoData : BaseData
{
	public UserAbilityType ability;
}

[Serializable]
public class UserAbilityInfoDataSheet : DataSheetBase<UserAbilityInfoData>
{
	public UserAbilityInfoData Get(long tid)
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

	public Int64 GetTid(UserAbilityType _ability)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].ability == _ability)
			{
				return infos[i].tid;
			}
		}

		return 0;
	}
}
