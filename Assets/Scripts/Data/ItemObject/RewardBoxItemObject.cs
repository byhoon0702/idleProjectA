﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBoxItemObject : ItemObject
{
	public override void SetBasicData<T>(T data)
	{
		tid = data.tid;
		itemName = data.name;
		description = data.description;
	}
}
