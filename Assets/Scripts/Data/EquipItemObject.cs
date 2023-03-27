using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
	SWORD,
	AXE,
	SPEAR,
	HAMMER,
	FIST,

}



[CreateAssetMenu(fileName = "EquipItem", menuName = "ScriptableObject/EquipItem", order = 1)]
public class EquipItemObject : ItemObject
{
	[SerializeField] private Grade itemGrade;
	[SerializeField] private int starLv;
	[SerializeField] private EquipType type;

	[SerializeField] private AbilityInfo[] equipAbilities;
	public AbilityInfo[] EquipAbilities => equipAbilities;

	[SerializeField] private AbilityInfo[] ownedAbilities;
	public AbilityInfo[] OwnedAbilities => ownedAbilities;

	public void SetBasicData(long tid, string name, string description, EquipType type, Grade grade, int starLv)
	{
		this.tid = tid;
		itemName = name;
		this.type = type;
		this.description = description;
		this.itemGrade = grade;
		this.starLv = starLv;

	}

	public void SetEquipAbilities(AbilityInfo[] infos)
	{
		equipAbilities = infos;
	}

	public void SetOwnedAbilities(AbilityInfo[] infos)
	{
		ownedAbilities = infos;
	}
}
