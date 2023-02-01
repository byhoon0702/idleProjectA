using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class UserTrainingConsumeData : BaseData
{
	public AbilityType abilityType;
	public Int32 startLevel;
	public Int32 endLevel;
	public IdleNumber consume;
}

[Serializable]
public class UserTrainingConsumeDataSheet : DataSheetBase<UserTrainingConsumeData>
{
	private Dictionary<AbilityType, List<UserTrainingConsumeData>> dic;

	public UserTrainingConsumeData Get(long tid)
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

	public List<UserTrainingConsumeData> Get(AbilityType _abilityType)
	{
		if (dic == null)
		{
			dic = new Dictionary<AbilityType, List<UserTrainingConsumeData>>();

			for (Int32 i = 0 ; i < infos.Count ; i++)
			{
				if (dic.ContainsKey(infos[i].abilityType) == false)
				{
					dic.Add(infos[i].abilityType, new List<UserTrainingConsumeData>());
				}

				dic[infos[i].abilityType].Add(infos[i]);
			}

			foreach (var v in dic.Values)
			{
				v.Sort((a, b) => a.startLevel.CompareTo(b.startLevel));
			}
		}

		return dic[_abilityType];
	}
}
