using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : BaseData
{
	public string name;
	public Grade itemGrade;
	public string hashTag;
}

[System.Serializable]
public class PetData : ItemData
{

	public int starlevel;

	public PetCategory category;
	public List<ItemStats> equipValues;
	public List<ItemStats> ownValues;

	public PetData Clone()
	{
		PetData data = new PetData();
		data.name = name;
		data.itemGrade = itemGrade;
		data.starlevel = starlevel;
		//data.projectileResource = projectileResource;
		//data.moveSpeed = moveSpeed;
		return data;

	}
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
