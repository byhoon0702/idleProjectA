using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UIBaseGrid<T> : MonoBehaviour
{
	public GameObject itemPrefab;

	[SerializeField] protected Transform itemRoot;

	protected UIManagementEquip parent;

	public abstract void Init(UIManagementEquip _parent);
	public abstract void OnUpdate(List<T> itemList);

}
