using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleEffectPoolManager : VObjectPool<ParticleEffect>
{
	private static ParticleEffectPoolManager instance;

	public static ParticleEffectPoolManager it => instance;

	private void Awake()
	{
		instance = this;
	}

	protected override void SetObject(ParticleEffect _object, IObjectPool<ParticleEffect> _pool)
	{
		if (_object == null)
		{
			return;
		}
		_object.Set(_pool);
	}

	protected override string GetPath(string _name)
	{
		return $"{PathHelper.particleEffectPath}/{_name}";
	}

	protected override ParticleEffect OnCreateObject(string _name)
	{
		if (GetResource(_name) == null)
		{
			return null;
		}
		ParticleEffect effect = Instantiate(GetResource(_name), transform);
		effect.name = _name;
		return effect;
	}

	protected override void OnGetObject(ParticleEffect _object)

	{
		if (_object == null)
		{
			return;
		}

		base.OnGetObject(_object);
		_object.gameObject.SetActive(true);
	}

	protected override void OnReleaseObject(ParticleEffect _object)
	{
		if (_object == null)
		{
			return;
		}
		base.OnReleaseObject(_object);


		_object.gameObject.SetActive(false);
	}

	protected override void OnDestroyObject(ParticleEffect _object)
	{
		if (_object == null)
		{
			return;
		}
		Destroy(_object.gameObject);
	}
}
