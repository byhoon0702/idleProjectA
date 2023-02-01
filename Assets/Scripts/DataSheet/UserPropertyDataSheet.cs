using System;
using System.Collections.Generic;

[Serializable]
public class UserPropertyData : BaseData
{
	public AbilityType abilityType;
	public Int32 maxLevel;
	public float incValue;
	public Int32 consumePoint;
}

[Serializable]
public class UserPropertyDataSheet : DataSheetBase<UserPropertyData>
{
	private List<AbilityType> abilities;

	public UserPropertyData Get(long tid)
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


	public UserPropertyData Get(AbilityType _abilityType)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].abilityType == _abilityType)
			{
				return infos[i];
			}
		}

		return null;
	}

	public List<AbilityType> GetAbilityTypes()
	{
		if(abilities == null || abilities.Count == 0)
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
