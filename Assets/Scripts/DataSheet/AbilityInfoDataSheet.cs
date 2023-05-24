
using System;

[Serializable]
public class AbilityInfoData : BaseData
{
	public Ability ability;
}

[Serializable]
public class AbilityInfoDataSheet : DataSheetBase<AbilityInfoData>
{
	public AbilityInfoData Get(long tid)
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

	public AbilityInfoData Get(Ability _ability)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].ability == _ability)
			{
				return infos[i];
			}
		}
		return null;
	}

	public Int64 GetTid(Ability _ability)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].ability == _ability)
			{
				return infos[i].tid;
			}
		}

		return 0;
	}
}
