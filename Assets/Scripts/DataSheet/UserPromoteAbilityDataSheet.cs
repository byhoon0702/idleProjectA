using System;
using System.Collections.Generic;

[Serializable]
public class UserPromoteAbilityData : BaseData
{
	public AbilityType abilityType;
	public float min;
	public float max;
}

[Serializable]
public class UserPromoteAbilityDataSheet : DataSheetBase<UserPromoteAbilityData>
{
	private List<AbilityType> abilities;


	public UserPromoteAbilityData Get(long tid)
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

	public UserPromoteAbilityData GetByAbilityType(AbilityType _ability)
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

	public List<AbilityType> GetAbilityTypes()
	{
		if (abilities == null || abilities.Count == 0)
		{
			abilities = new List<AbilityType>();

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
