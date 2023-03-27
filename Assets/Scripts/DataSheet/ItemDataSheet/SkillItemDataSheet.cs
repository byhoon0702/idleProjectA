using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class SkillItemData : ItemData
{
	public long skillTid;
	public List<StatsValue> equipValues;
	public List<StatsValue> ownValues;

	public override List<AbilityInfo> EquipAbilityInfos()
	{
		List<AbilityInfo> abilityInfo = new List<AbilityInfo>();
		for (int i = 0; i < equipValues.Count; i++)
		{
			var equip = equipValues[i];
			AbilityInfo info = new AbilityInfo(equip.type, (IdleNumber)equip.value, (IdleNumber)equip.perLevel);
			abilityInfo.Add(info);
		}

		return abilityInfo;
	}

	public override List<AbilityInfo> OwnAbilityInfos()
	{
		List<AbilityInfo> abilityInfo = new List<AbilityInfo>();
		for (int i = 0; i < ownValues.Count; i++)
		{
			var equip = ownValues[i];
			AbilityInfo info = new AbilityInfo(equip.type, (IdleNumber)equip.value, (IdleNumber)equip.perLevel);
			abilityInfo.Add(info);
		}

		return abilityInfo;
	}
}

[Serializable]
public class SkillItemDataSheet : DataSheetBase<SkillItemData>
{
	public List<SkillItemData> GetList(Grade _grade)
	{
		List<SkillItemData> outData = new List<SkillItemData>();

		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].itemGrade == _grade)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}

	//public SkillItemData GetByHashTag(string _hashTag)
	//{
	//	if (cachedTidList.TryGetValue(_hashTag, out var data))
	//	{
	//		return data;
	//	}

	//	for (int i = 0; i < infos.Count; i++)
	//	{
	//		if (infos[i].hashTag == _hashTag)
	//		{
	//			cachedTidList.Add(_hashTag, infos[i]);
	//			return infos[i];
	//		}
	//	}

	//	return null;
	//}
}
