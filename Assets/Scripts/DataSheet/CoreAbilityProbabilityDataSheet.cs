using System;
using System.Collections.Generic;

[Serializable]
public class CoreAbilityProbabilityData : BaseData
{
	public Grade grade;
	public double probability;
	public double rangeMax;
}

[Serializable]
public class CoreAbilityProbabilityDataSheet : DataSheetBase<CoreAbilityProbabilityData>
{
	public CoreAbilityProbabilityData Get(long tid)
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

	public CoreAbilityProbabilityData Get(Grade grade)
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
