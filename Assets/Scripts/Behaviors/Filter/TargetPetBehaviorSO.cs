using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Pet Filter Behavior", menuName = "ScriptableObject/Filter/Pet")]
public class TargetPetBehaviorSO : TargetFilterBehaviorSO
{

	public override UnitBase[] GetObject()
	{
		return FilterObject<Pet>();
	}
}
