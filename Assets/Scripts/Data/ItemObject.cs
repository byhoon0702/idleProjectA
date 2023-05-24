using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemObject : ScriptableObject
{
	[SerializeField][ReadOnly(true)] protected long tid;
	public long Tid => tid;
	[SerializeField] protected Sprite icon;
	public Sprite Icon => icon;

	//[SerializeField] private ItemType type;
	[SerializeField] protected string itemName;
	public string ItemName => itemName;
	[SerializeField] protected string description;
	public string Description => description;


	public virtual string tailChar
	{
		get
		{
			return "";
		}

	}

}

