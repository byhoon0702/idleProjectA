using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CostumeType
{
	HEAD = 0,
	BODY = 1,
	WEAPON = 2,

	_END,

}

[CreateAssetMenu(fileName = "CostumeItem", menuName = "ScriptableObject/CostumeItem", order = 1)]
public class CostumeItemObject : ItemObject
{
	[SerializeField] private Grade itemGrade;
	[SerializeField] private int starLv;
	[SerializeField] private CostumeType type;
	public CostumeType Type => type;

	[SerializeField] private GameObject costumeObject;
	public GameObject CostumeObject => costumeObject;

	[SerializeField] private AnimatorOverrideController animatorOverrideController;
	public AnimatorOverrideController OverrideAnimator => animatorOverrideController;

	[SerializeField] private AbilityInfo[] equipAbilities;
	public AbilityInfo[] EquipAbilities => equipAbilities;

	[SerializeField] private AbilityInfo[] ownedAbilities;
	public AbilityInfo[] OwnedAbilities => ownedAbilities;


	public void SetBasicData(long tid, string name, string description, CostumeType type, Grade grade, int starLv)
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
