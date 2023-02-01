using System;
using System.Collections.Generic;

[Serializable]
public class UserTrainingData : BaseData
{
	public AbilityType abilityType;
	public Int32 maxLevel;
	public float defaultValue;
	public float incValue;
}

[Serializable]
public class UserTrainingDataSheet : DataSheetBase<UserTrainingData>
{
	private List<AbilityType> abilities;


	public UserTrainingData Get(long tid)
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


	public UserTrainingData Get(AbilityType _abilityType)
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
