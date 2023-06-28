using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class VObjectPool<T> : MonoBehaviour where T : Object
{
	protected Dictionary<string, T> resourceDictionary = new Dictionary<string, T>();
	protected Dictionary<string, IObjectPool<T>> poolDictionary = new Dictionary<string, IObjectPool<T>>();

	private Dictionary<int, bool> releaseCheck = new Dictionary<int, bool>();

	public T Get(string _path, string _name)
	{
		IObjectPool<T> pool = GetPool(_path, _name);
		T t = pool.Get();
		SetObject(t, pool);
		SetRelease(t, false);
		return t;
	}
	public T Get(GameObject _obj)
	{
		if (_obj == null)
		{
			return null;
		}
		IObjectPool<T> pool = GetPool(_obj);
		T t = pool.Get();
		SetObject(t, pool);
		SetRelease(t, false);
		return t;
	}

	public bool IsReleased(T _t)
	{
		return releaseCheck.ContainsKey(_t.GetInstanceID()) && releaseCheck[_t.GetInstanceID()] == true;
	}

	protected IObjectPool<T> GetPool(string _path, string _name)
	{
		IObjectPool<T> pool;

		if (poolDictionary.TryGetValue(_name, out pool) == false)
		{
			pool = new ObjectPool<T>(() =>
			{
				return OnCreateObject(_path, _name);
			}, OnGetObject, OnReleaseObject, OnDestroyObject);
			poolDictionary.Add(_name, pool);
		}

		return pool;
	}
	protected IObjectPool<T> GetPool(GameObject _object)
	{
		IObjectPool<T> pool;

		if (poolDictionary.TryGetValue(_object.name, out pool) == false)
		{
			pool = new ObjectPool<T>(() =>
			{
				return OnCreateObject(_object);
			}, OnGetObject, OnReleaseObject, OnDestroyObject);
			poolDictionary.Add(_object.name, pool);
		}

		return pool;
	}

	protected virtual T GetResource(string _path, string _name)
	{
		if (_name.IsNullOrEmpty())
		{
			Debug.LogError("Name is Null");
			return null;
		}
		GameObject prefabResource;
		T result;

		if (resourceDictionary.TryGetValue(_name, out result) == false)
		{
			string path = GetPath(_path, _name);
			var resource = Resources.Load(path);
			if (resource == null)
			{
				return null;
			}

			prefabResource = Instantiate(resource, transform) as GameObject;
			prefabResource.name = _name;
			prefabResource.SetActive(false);
			result = prefabResource.GetComponent<T>();
			resourceDictionary.Add(_name, result);
		}

		return result;
	}

	protected void SetRelease(T _t, bool _isRelease)
	{
		if (_t == null)
		{
			return;
		}
		if (releaseCheck.ContainsKey(_t.GetInstanceID()) == false)
		{
			releaseCheck.Add(_t.GetInstanceID(), _isRelease);
		}
		else
		{
			releaseCheck[_t.GetInstanceID()] = _isRelease;
		}
	}

	protected abstract void SetObject(T _object, IObjectPool<T> _pool);
	protected abstract string GetPath(string _path, string _name);
	protected abstract T OnCreateObject(string _path, string _name);
	protected virtual T OnCreateObject(GameObject _object)
	{
		return null;
	}
	protected virtual void OnGetObject(T _object)
	{
		SetRelease(_object, false);
	}
	protected virtual void OnReleaseObject(T _object)
	{
		SetRelease(_object, true);
	}
	protected abstract void OnDestroyObject(T _object);

	public void ClearPool()
	{
		foreach (var pool in poolDictionary.Values)
		{
			pool.Clear();
		}
	}
}
