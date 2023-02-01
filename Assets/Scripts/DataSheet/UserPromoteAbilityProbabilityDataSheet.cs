using System;
using System.Collections.Generic;

[Serializable]
public class UserPromoteAbilityProbabilityData : BaseData
{
	public Grade grade;
	public float probability;
	public float rangeMax;
}

[Serializable]
public class UserPromoteAbilityProbabilityDataSheet : DataSheetBase<UserPromoteAbilityProbabilityData>
{
	public UserPromoteAbilityProbabilityData Get(long tid)
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

	public UserPromoteAbilityProbabilityData Get(Grade grade)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].grade == grade)
			{
				return infos[i];
			}
		}

		return null;
	}
}
