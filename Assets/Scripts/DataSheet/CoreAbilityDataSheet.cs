using System;
using System.Collections.Generic;

[Serializable]
public class CoreAbilityData : BaseData
{
	public Stats abilityType;
	public double min;
	public double max;
}

[Serializable]
public class CoreAbilityDataSheet : DataSheetBase<CoreAbilityData>
{
	private List<Stats> abilities;


	public CoreAbilityData Get(long tid)
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

	public CoreAbilityData GetByAbilityType(Stats _ability)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].abilityType == _ability)
			{
				return infos[i];
			}
		}

		return null;
	}

	public List<Stats> GetAbilityTypes()
	{
		if (abilities == null || abilities.Count == 0)
		{
			abilities = new List<Stats>();

			for (int i = 0 ; i < infos.Count ; i++)
			{
				if (abilities.Contains(infos[i].abilityType) == false)
				{
					abilities.Add(infos[i].abilityType);
				}
			}
		}

		return abilities;
	}
}
