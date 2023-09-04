using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Awakening Item Object", menuName = "ScriptableObject/Item/Awakening ", order = 1)]
public class AwakeningItemObject : ItemObject
{
	[SerializeField] private Sprite[] _icons;
	public Sprite[] Icons => _icons;
	public override void SetBasicData<T>(T data)
	{
		tid = data.tid;
	}

}
