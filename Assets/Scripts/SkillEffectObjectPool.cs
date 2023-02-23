using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SkillEffectObjectPool : VObjectPool<SkillEffectObject>
{
	private static SkillEffectObjectPool instance;
	public static SkillEffectObjectPool it => instance;

	private void Awake()
	{
		instance = this;
	}

	protected override string GetPath(string _name)
	{
		return _name;
	}

	protected override SkillEffectObject OnCreateObject(string _name)
	{
		GameObject go = new GameObject();
		go.transform.SetParent(transform);
		SkillEffectObject skillEffectObject = go.AddComponent<SkillEffectObject>();
		go.name = _name;
		return skillEffectObject;
	}

	protected override void OnDestroyObject(SkillEffectObject _object)
	{
		Destroy(_object.gameObject);
	}

	protected override void OnGetObject(SkillEffectObject _object)
	{
		_object.gameObject.SetActive(true);
	}

	protected override void OnReleaseObject(SkillEffectObject _object)
	{
		_object.gameObject.SetActive(false);
	}

	protected override void SetObject(SkillEffectObject _object, IObjectPool<SkillEffectObject> _pool)
	{
		_object.SetManagedPool(_pool);
	}
}
