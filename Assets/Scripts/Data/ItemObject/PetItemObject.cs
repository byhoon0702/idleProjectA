using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PetItem", menuName = "ScriptableObject/Item/PetItem", order = 1)]
public class PetItemObject : ItemObject
{


	[SerializeField] private GameObject petObject;
	public GameObject PetObject => petObject;


	public override void SetBasicData<T>(T data)
	{
		var petData = data as PetData;
		this.tid = petData.tid;

		itemName = petData.name;
		this.description = petData.description;

	}
}
