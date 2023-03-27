using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PetItem", menuName = "ScriptableObject/PetItem", order = 1)]
public class PetItemObject : ItemObject
{
	[SerializeField] private Grade itemGrade;
	[SerializeField] private int starLv;

	[SerializeField] private GameObject petObject;
	public GameObject PetObject => petObject;
	[SerializeField] private AbilityInfo[] equipAbilities;
	public AbilityInfo[] EquipAbilities => equipAbilities;

	[SerializeField] private AbilityInfo[] ownedAbilities;
	public AbilityInfo[] OwnedAbilities => ownedAbilities;

	public void SetBasicData(long tid, string name, string description, Grade grade, int starLv)
	{
		this.tid = tid;
		itemName = name;
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
