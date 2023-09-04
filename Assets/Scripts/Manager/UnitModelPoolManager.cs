using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UnitModelPoolManager : VObjectPool<UnitAnimation>
{
	private static UnitModelPoolManager instance;
	public static UnitModelPoolManager Instance => instance;
	public static UnitModelPoolManager it => instance;

	[SerializeField] private Transform modelParentTransform;

	private void Awake()
	{
		instance = this;
	}

	protected override void SetObject(UnitAnimation _object, IObjectPool<UnitAnimation> _pool)
	{
		_object.Set(_pool);
	}

	protected override string GetPath(string _path, string _name)
	{
		string result = $"{_path}/{_name}";
		return result;
	}

	protected override UnitAnimation OnCreateObject(string _path, string _name)
	{
		try
		{

			UnitAnimation model = GetResource(_path, _name);
			model.name = _name;
			return model;
		}
		catch (System.Exception ex)
		{
			Debug.LogError($"{_name}\n{ex}");
			return null;
		}

	}

	protected override void OnGetObject(UnitAnimation _object)
	{
		base.OnGetObject(_object);
		_object.Get();
		_object.gameObject.SetActive(true);
	}

	protected override void OnReleaseObject(UnitAnimation _object)
	{
		base.OnReleaseObject(_object);
		_object.gameObject.SetActive(false);
		_object.transform.SetParent(modelParentTransform, false);
		_object.ResetAnimation();

		_object.gameObject.SetActive(false);
	}

	protected override void OnDestroyObject(UnitAnimation _object)
	{
		Destroy(_object.gameObject);
	}
}
