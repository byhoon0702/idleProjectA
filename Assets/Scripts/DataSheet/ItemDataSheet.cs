
using System;
using System.Collections.Generic;

[Serializable]
public class ItemData : BaseData
{
	public string name;
	public string itemDesc;
	public ItemType itemType;
	public Grade itemGrade;
	public string hashTag;

	//장착/장착 레벨 상승율, 보유 효과/보유 효과 레벨상승률

	// 장착효과
	public Stats equipAbility;
	public string equipAbilityValue;
	public string equipAbilityInc;
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
				EquipAbilityInfo.inc = (IdleNumber)equipAbilityInc;
			}

			return equipAbilityInfo;
		}
	}

	// 보유효과
	public Stats toOwnAbility;
	public string toOwnValue;
	public string toOwnInc;
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
				toOwnAbilityInfo.inc = (IdleNumber)toOwnInc;
			}

			return toOwnAbilityInfo;
		}
	}



	public long unitTid;
	public long petTid;
	public long itemRefillTid;
	public long skillTid;
	public long userTrainingTid;
	public long userPropertyTid;
	public long userMasteryTid;


	public string Icon => tid.ToString();
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

	public List<ItemData> GetByItemType(ItemType _itemType)
	{
		List<ItemData> outData = new List<ItemData>();

		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].itemType == _itemType)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}
	public List<ItemData> GetByItemTypeAndGrade(ItemType _itemType, Grade _grade)
	{
		List<ItemData> outData = new List<ItemData>();

		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].itemType == _itemType && infos[i].itemGrade == _grade)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}

	public ItemData GetBySkillTid(long _skillTid)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].skillTid == _skillTid)
			{
				return infos[i];
			}
		}

		return null;
	}

	public ItemData GetByUnitTid(long _unitTid)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].unitTid == _unitTid)
			{
				return infos[i];
			}
		}

		return null;
	}

	public ItemData GetByPetTid(long _petTid)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].petTid == _petTid)
			{
				return infos[i];
			}
		}

		return null;
	}

	public ItemData GetByMasteryTid(long _masteryTid)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].userMasteryTid == _masteryTid)
			{
				return infos[i];
			}
		}

		return null;
	}
}
