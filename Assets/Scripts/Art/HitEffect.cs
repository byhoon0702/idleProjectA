using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

[RequireComponent(typeof(FxSpriteEffectAuto))]
public class HitEffect : MonoBehaviour
{
	[SerializeField] private FxSpriteEffectAuto _fxSpriteEffectAuto = null;

	private IObjectPool<HitEffect> managedPool = null;

	public void Set(IObjectPool<HitEffect> _managedPool)
	{
		managedPool = _managedPool;
		if (_fxSpriteEffectAuto == null)
		{
			_fxSpriteEffectAuto = GetComponent<FxSpriteEffectAuto>();
		}
		if (_fxSpriteEffectAuto == null)
		{
			_fxSpriteEffectAuto = GetComponentInChildren<FxSpriteEffectAuto>();
		}
		if (_fxSpriteEffectAuto == null)
		{
			VLog.LogError("FxSpriteEffectAuto not found");
		}

		_fxSpriteEffectAuto.CopyMaterial();
	}

	public void Play()
	{
		_fxSpriteEffectAuto?.Play(Release);
	}

	public Tween PlayEditor()
	{
		return _fxSpriteEffectAuto?.PlayEditor(Release);
	}

	public void Release()
	{
		managedPool?.Release(this);
		if (managedPool == null)
		{
			Destroy(gameObject);
		}
	}
}
