using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/Item", order = 10)]
public class ItemObject : ScriptableObject
{
	[SerializeField][ReadOnly(false)] protected long tid;
	public long Tid => tid;
	[SerializeField] protected Sprite icon;
	public Sprite Icon => icon;

	//[SerializeField] private ItemType type;
	[SerializeField] protected string itemName;
	public string ItemName => itemName;
	[SerializeField] protected string description;
	public string Description => description;
}

[System.Serializable]
public struct ItemBuff
{
	[SerializeField] private Ability type;
	[SerializeField] private IdleNumber value;
}
