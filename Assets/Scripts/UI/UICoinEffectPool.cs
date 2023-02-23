using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UICoinEffectPool : VObjectPool<SCUITweenPathMove>
{
	protected override string GetPath(string _name)
	{
		return "B/Fx_CoinEffect";
	}

	protected override SCUITweenPathMove OnCreateObject(string _name)
	{
		SCUITweenPathMove coinEffect = Instantiate(GetResource(_name), transform);
		coinEffect.name = _name;
		return coinEffect;
	}

	protected override void OnDestroyObject(SCUITweenPathMove _object)
	{
		Destroy(_object.gameObject);
	}

	protected override void OnGetObject(SCUITweenPathMove _object)
	{
		_object.gameObject.SetActive(true);
	}

	protected override void OnReleaseObject(SCUITweenPathMove _object)
	{
		_object.gameObject.SetActive(false);
	}

	protected override void SetObject(SCUITweenPathMove _object, IObjectPool<SCUITweenPathMove> _pool)
	{
		_object.Set(_pool);
	}
}
