using System;
using System.Collections.Generic;

[System.Serializable]
public struct StatsValue
{
	public Ability type;
	//UI 상에 같이 표현, 동일한 값을 적용해야 할 경우
	//public Stats pairType;
	public string value;
	public string perLevel;

	/// <summary>
	/// 버프(곱연산) 인지 아닌지
	/// </summary>
	public bool isBuff;
}

[System.Serializable]
public class EquipItemData : ItemData
{
	public long costumeTid;
	public EquipType equipType;
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

[System.Serializable]
public class EquipItemDataSheet : ItemDataBaseSheet<EquipItemData>
{

	public List<EquipItemData> GetByItemType(EquipType _itemType)
	{
		List<EquipItemData> outData = new List<EquipItemData>();

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].equipType == _itemType)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}

	public List<EquipItemData> GetByItemTypeAndGrade(ItemType _itemType, Grade _grade)
	{
		List<EquipItemData> outData = new List<EquipItemData>();

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].itemType == _itemType && infos[i].itemGrade == _grade)
			{
				outData.Add(infos[i]);
			}
		}

		return outData;
	}

	//public EquipItemData GetByUnitTid(long _unitTid)
	//{
	//	for (int i = 0; i < infos.Count; i++)
	//	{
	//		if (infos[i].unitTid == _unitTid)
	//		{
	//			return infos[i];
	//		}
	//	}

	//	return null;
	//}
}
