using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public struct ContentOpenMessage
{
	public string message;
	public List<AddItemInfo> displayItems;
	public ContentOpenMessage(string message)
	{
		this.message = message;
		displayItems = null;
	}
	public ContentOpenMessage(string message, List<AddItemInfo> items)
	{
		this.message = message;
		displayItems = new List<AddItemInfo>(items);
	}

	public ContentOpenMessage(string message, AddItemInfo item)
	{
		this.message = message;
		displayItems = new List<AddItemInfo>() { item };
	}

	public ContentOpenMessage(string message, RuntimeData.RewardInfo item)
	{
		this.message = message;
		displayItems = new List<AddItemInfo>() { new AddItemInfo(item) };
	}
	public ContentOpenMessage(string message, List<RuntimeData.RewardInfo> item)
	{
		this.message = message;
		displayItems = new List<AddItemInfo>();
		for (int i = 0; i < item.Count; i++)
		{
			displayItems.Add(new AddItemInfo(item[i]));
		}

	}

}

public class ContentOpenMessagePool : PoolContainer<ContentOpenMessageItem>
{
	private GameObject _prefab;
	public Transform parent;
	public ContentOpenMessagePool(GameObject prefab)
	{
		_prefab = prefab;
	}

	protected override ContentOpenMessageItem CreatePoolItem()
	{
		GameObject go = Object.Instantiate(_prefab);
		go.transform.SetParent(parent);
		ContentOpenMessageItem item = go.GetComponent<ContentOpenMessageItem>();

		RectTransform rectTransform = go.GetComponent<RectTransform>();
		rectTransform.anchoredPosition3D = Vector3.zero;
		rectTransform.localScale = Vector3.one;

		item.pool = this;
		return item;
	}

	protected override void OnReturnedPool(ContentOpenMessageItem obj)
	{
		obj.gameObject.SetActive(false);
	}

	protected override void OnTakeFromPool(ContentOpenMessageItem obj)
	{
		obj.gameObject.SetActive(true);
	}

	public void Release(ContentOpenMessageItem obj)
	{
		_pool.Release(obj);

	}

}

public abstract class PoolContainer<T> where T : Component
{
	public int maxPoolSize = 10;
	public bool collectionChecks;
	protected IObjectPool<T> _pool;
	public IObjectPool<T> Pool
	{
		get
		{
			if (_pool == null)
			{
				_pool = new ObjectPool<T>(CreatePoolItem, OnTakeFromPool, OnReturnedPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
			}
			return _pool;
		}
	}
	protected abstract T CreatePoolItem();
	protected abstract void OnTakeFromPool(T obj);

	protected abstract void OnReturnedPool(T obj);
	protected virtual void OnDestroyPoolObject(T obj)
	{
		Object.Destroy(obj.gameObject);
	}

}


public class ContentOpenMessageSystem : MonoBehaviour
{
	public float DisplayRate = 0.5f;
	public GameObject prefab;
	private float _time;
	private Queue<ContentOpenMessage> _messages = new Queue<ContentOpenMessage>();

	private ContentOpenMessagePool _pool;
	public IObjectPool<ContentOpenMessageItem> Pool => _pool.Pool;

	public void Start()
	{
		_pool = new ContentOpenMessagePool(prefab);
		_pool.parent = transform;
	}

	public void AddMessage(ContentOpenMessage message)
	{
		_messages.Enqueue(message);
	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.GameStop)
		{
			return;
		}
		if (StageManager.it.CurrentStage == null || StageManager.it.currentRule == null)
		{
			return;
		}
		if (StageManager.it.CheckNormalStage() == false)
		{
			return;
		}

		if (StageManager.it.currentRule.currentFsm is BattleState || StageManager.it.currentRule.currentFsm is BattleStartState)
		{
			if (_messages.Count > 0)
			{
				if (_time >= DisplayRate)
				{
					Pool?.Get().Display(_messages.Dequeue());
					_time = 0;
				}
				_time += Time.deltaTime;
			}
		}
	}
}
