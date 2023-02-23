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

	protected override string GetPath(string _name)
	{
		string result = $"B/{_name}";
		return result;
	}

	protected override UnitAnimation OnCreateObject(string _name)
	{
		UnitAnimation model = Instantiate(GetResource(_name));
		model.name = _name;
		return model;
	}

	protected override void OnGetObject(UnitAnimation _object)
	{
		_object.gameObject.SetActive(true);
	}

	protected override void OnReleaseObject(UnitAnimation _object)
	{
		_object.transform.SetParent(modelParentTransform, false);
		_object.GetComponent<UnitAnimation>().ResetAnimation();
	}

	protected override void OnDestroyObject(UnitAnimation _object)
	{
		Destroy(_object.gameObject);
	}
}
