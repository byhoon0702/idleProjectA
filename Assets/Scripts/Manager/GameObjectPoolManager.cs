using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class GameObjectPoolManager : MonoBehaviour
{
	public static GameObjectPoolManager it;
	public IObjectPool<FieldItem> fieldItemPool;

	private void Awake()
	{
		it = this;
		fieldItemPool = new ObjectPool<FieldItem>(OnCreateGameObject, OnGetFieldItem, OnReleaseFieldItem, OnDestroyFieldItem);
	}

	public FieldItem OnCreateGameObject()
	{
		GameObject go = (GameObject)Instantiate(Resources.Load("Item/FieldItem"));
		FieldItem fieldItem = go.GetComponent<FieldItem>();
		go.gameObject.SetActive(false);
		go.transform.SetParent(null);
		fieldItem.SetObjectPool(fieldItemPool);
		return fieldItem;
	}

	public void OnGetFieldItem(FieldItem fieldItem)
	{
		fieldItem.gameObject.SetActive(true);
	}

	public void OnReleaseFieldItem(FieldItem fieldItem)
	{
		fieldItem.gameObject.SetActive(false);
	}

	public void OnDestroyFieldItem(FieldItem fieldItem)
	{
		Destroy(fieldItem.gameObject);
	}
}
