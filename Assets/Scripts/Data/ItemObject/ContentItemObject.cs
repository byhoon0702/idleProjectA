using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ContentItemObject : ItemObject
{

	public override void SetBasicData<T>(T data)
	{
		var contentData = data as ContentsData;
		tid = contentData.tid;
		itemName = contentData.name;
		description = contentData.description;

	}
}
