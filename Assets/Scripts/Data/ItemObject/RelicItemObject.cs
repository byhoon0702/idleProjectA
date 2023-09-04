using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Relic Item Object", menuName = "ScriptableObject/Item/Relic", order = 1)]
public class RelicItemObject : ItemObject
{
	public override void SetBasicData<T>(T data)
	{
		tid = data.tid;

	}

}
