using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemObject : ScriptableObject
{
	[SerializeField][ReadOnly(true)] protected long tid;
	public long Tid => tid;
	[SerializeField] protected Sprite icon;
	public Sprite ItemIcon => icon;

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

	public virtual string ToJson()
	{
		string json = JsonUtility.ToJson(this);
		return json;
	}

	public virtual void FromJson(string json)
	{
		JsonUtility.FromJsonOverwrite(json, this);
	}

	public abstract void SetBasicData<T>(T data) where T : BaseData;

}


