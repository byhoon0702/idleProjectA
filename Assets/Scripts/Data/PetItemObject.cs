using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PetItem", menuName = "ScriptableObject/Item/PetItem", order = 1)]
public class PetItemObject : ItemObject
{


	[SerializeField] private GameObject petObject;
	public GameObject PetObject => petObject;


	public void SetBasicData(long tid, string name, string description, Grade grade, int starLv)
	{
		this.tid = tid;
		itemName = name;
		this.description = description;

	}
}
