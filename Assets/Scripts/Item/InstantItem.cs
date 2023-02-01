using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class InstantItem
{
	public long tid;
	public ItemType type;
	public Grade grade;
	public int level;
	public int exp;
	public IdleNumber count;
	public string nextRefillTime;
	public string nextResetTime;

	public InstantItem DeepClone()
	{
		InstantItem item = new InstantItem();

		item.tid = tid;
		item.type = type;
		item.grade = grade;
		item.exp = exp;
		item.count = count;
		item.nextRefillTime = nextRefillTime;
		item.nextResetTime = nextResetTime;

		return item;
	}
}
