using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HitEffectPoolManager : VObjectPool<HitEffect>
{
	private static HitEffectPoolManager instance;

	public static HitEffectPoolManager it => instance;

	private void Awake()
	{
		instance = this;
	}

	protected override void SetObject(HitEffect _object, IObjectPool<HitEffect> _pool)
	{
		if (_object == null)
		{
			return;
		}
		_object.Set(_pool);
	}

	protected override string GetPath(string _path, string _name)
	{
		return $"{PathHelper.hitEffectPath}/{_name}";
	}

	protected override HitEffect OnCreateObject(string _path, string _name)
	{
		if (GetResource("", _name) == null)
		{
			return null;
		}
		HitEffect effect = Instantiate(GetResource("", _name), transform);
		effect.name = _name;
		return effect;
	}

	protected override void OnGetObject(HitEffect _object)
	{
		base.OnGetObject(_object);

		if (_object == null)
		{
			return;
		}
		_object.gameObject.SetActive(true);
	}

	protected override void OnReleaseObject(HitEffect _object)
	{
		base.OnReleaseObject(_object);

		if (_object == null)
		{
			return;
		}
		_object.gameObject.SetActive(false);
	}

	protected override void OnDestroyObject(HitEffect _object)
	{
		if (_object == null)
		{
			return;
		}
		Destroy(_object.gameObject);
	}
}
