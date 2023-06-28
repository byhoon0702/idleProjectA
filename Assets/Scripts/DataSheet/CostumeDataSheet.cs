using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CostumeData : ItemData
{
	public int starLevel;
	public CostumeType costumeType;
	public int point;
	//public List<ItemStats> equipValues;
	//public List<ItemStats> ownValues;

	//public override List<AbilityInfo> EquipAbilityInfos()
	//{
	//	List<AbilityInfo> abilityInfo = new List<AbilityInfo>();
	//	for (int i = 0; i < equipValues.Count; i++)
	//	{
	//		var equip = equipValues[i];
	//		AbilityInfo info = new AbilityInfo(equip.type, (IdleNumber)equip.value, (IdleNumber)equip.perLevel, equip.isMultiply);
	//		abilityInfo.Add(info);
	//	}

	//	return abilityInfo;
	//}

	//public override List<AbilityInfo> OwnAbilityInfos()
	//{
	//	List<AbilityInfo> abilityInfo = new List<AbilityInfo>();
	//	for (int i = 0; i < ownValues.Count; i++)
	//	{
	//		var equip = ownValues[i];
	//		AbilityInfo info = new AbilityInfo(equip.type, (IdleNumber)equip.value, (IdleNumber)equip.perLevel, equip.isMultiply);
	//		abilityInfo.Add(info);
	//	}

	//	return abilityInfo;
	//}
}
[System.Serializable]
public class CostumeDataSheet : DataSheetBase<CostumeData>
{
	public List<CostumeData> GetByItemType(CostumeType _itemType)
	{
		List<CostumeData> outData = new List<CostumeData>();

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].costumeType == _itemType)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}
}
