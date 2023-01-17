using System;
using System.Collections.Generic;

[Serializable]
public class UserRelicData : BaseData
{
	public UserAbilityType abilityType;
	public Int32 maxLevel;
	public float incValue;

	/// <summary>
	/// 시작 성공률
	/// </summary>
	public float startUpgradeRatio;
	/// <summary>
	/// 레벨당 성공률 감소량
	/// </summary>
	public float decreaseSuccessRatio;


	/// <summary>
	/// 소비재화
	/// </summary>
	public Int64 consumeItem;
	/// <summary>
	/// 소비재화개수
	/// </summary>
	public Int32 consumePoint;
}

[Serializable]
public class UserRelicDataSheet : DataSheetBase<UserRelicData>
{
	private List<UserAbilityType> abilities;


	public UserRelicData Get(long tid)
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


	public UserRelicData Get(UserAbilityType _abilityType)
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

	public List<UserAbilityType> GetAbilityTypes()
	{
		if (abilities == null || abilities.Count == 0)
		{
			abilities = new List<UserAbilityType>();

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
