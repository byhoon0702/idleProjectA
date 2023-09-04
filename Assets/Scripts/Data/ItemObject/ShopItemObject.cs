using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Shop Item", menuName = "ScriptableObject/Item/Shop", order = 1)]
public class ShopItemObject : ItemObject
{

	public override void SetBasicData<T>(T data)
	{
		tid = data.tid;
		itemName = data.name;
		description = data.description;
	}


}
