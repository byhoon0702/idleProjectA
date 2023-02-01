
using System;
using System.Collections.Generic;

[Serializable]
public class ItemData : BaseData
{
	public string name;
	public ItemType itemType;
	public Grade itemGrade;
	public string hashTag;
	public long refillLinkTid;
	public long relicLinkTid;
	//장착/장착 레벨 상승율, 보유 효과/보유 효과 레벨상승률

	// 장착효과
	public AbilityType equipAbility; 
	public string equipAbilityValue;
	[NonSerialized] private AbilityInfo equipAbilityInfo;
	public AbilityInfo EquipAbilityInfo
	{
		get
		{
			if (equipAbilityInfo == null)
			{
				equipAbilityInfo = new AbilityInfo();
				equipAbilityInfo.type = equipAbility;
				equipAbilityInfo.value = (IdleNumber)equipAbilityValue;
			}

			return equipAbilityInfo;
		}
	}

	// 보유효과
	public AbilityType toOwnAbility;
	public string toOwnValue;
	[NonSerialized] private AbilityInfo toOwnAbilityInfo;
	public AbilityInfo ToOwnAbilityInfo
	{
		get
		{
			if (toOwnAbilityInfo == null)
			{
				toOwnAbilityInfo = new AbilityInfo();
				toOwnAbilityInfo.type = toOwnAbility;
				toOwnAbilityInfo.value = (IdleNumber)toOwnValue;
			}

			return toOwnAbilityInfo;
		}
	}
}

[Serializable]
public class ItemDataSheet : DataSheetBase<ItemData>
{
	private Dictionary<string/* hashtag */, ItemData> cachedTidList = new Dictionary<string, ItemData>();

	public ItemData Get(long tid)
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

	public ItemData GetByHashTag(string _hashTag)
	{
		if(cachedTidList.TryGetValue(_hashTag, out var data))
		{
			return data;
		}

		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].hashTag == _hashTag)
			{
				cachedTidList.Add(_hashTag, infos[i]);
				return infos[i];
			}
		}

		return null;
	}
}
