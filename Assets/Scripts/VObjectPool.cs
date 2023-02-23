using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class VObjectPool<T> : MonoBehaviour where T : Object
{
	protected Dictionary<string, T> resourceDictionary = new Dictionary<string, T>();
	protected Dictionary<string, IObjectPool<T>> poolDictionary = new Dictionary<string, IObjectPool<T>>();

	public T Get(string _name = "default")
	{
		IObjectPool<T> pool = GetPool(_name);
		T t = pool.Get();
		SetObject(t, pool);
		return t;
	}

	protected IObjectPool<T> GetPool(string _name)
	{
		IObjectPool<T> pool;

		if (poolDictionary.TryGetValue(_name, out pool) == false)
		{
			pool = new ObjectPool<T>(() => { return OnCreateObject(_name); }, OnGetObject, OnReleaseObject, OnDestroyObject);
			poolDictionary.Add(_name, pool);
		}

		return pool;
	}

	protected virtual T GetResource(string _name)
	{
		GameObject resource;
		T result;

		if (resourceDictionary.TryGetValue(_name, out result) == false)
		{
			string path = GetPath(_name);
			if (Resources.Load(path) == null)
			{
				return null;
			}

			resource = Instantiate(Resources.Load(path), transform) as GameObject;
			resource.name = _name;
			resource.SetActive(false);
			result = resource.GetComponent<T>();
			resourceDictionary.Add(_name, result);
		}

		return result;
	}

	/// <summary>
	/// 오브젝트 Get() 전의 처리과정. managedPool 등록 등.
	/// </summary>
	/// <param name="_object"></param>
	/// <param name="_pool"></param>
	protected abstract void SetObject(T _object, IObjectPool<T> _pool);
	protected abstract string GetPath(string _name);
	protected abstract T OnCreateObject(string _name);
	protected abstract void OnGetObject(T _object);
	protected abstract void OnReleaseObject(T _object);
	protected abstract void OnDestroyObject(T _object);

	public void ClearPool()
	{
		foreach (var pool in poolDictionary.Values)
		{
			pool.Clear();
		}
	}
}
