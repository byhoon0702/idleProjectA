using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ParticleEffect : MonoBehaviour
{
	private IObjectPool<ParticleEffect> managedPool = null;
	public ParticleBehavior particleBehavior;

	bool isRelease;
	public void Set(IObjectPool<ParticleEffect> _managedPool)
	{
		managedPool = _managedPool;
		if (particleBehavior == null)
		{
			particleBehavior = GetComponentInChildren<ParticleBehavior>();
		}
		if (particleBehavior == null)
		{
			VLog.LogError("ParticleBehavior not found");
		}

		isRelease = false;
		particleBehavior.Init(Release);
	}

	public void Play(float speed = 1f)
	{
		particleBehavior.SetSpeed(speed);
		particleBehavior?.Play();
	}
	public void Stop()
	{

		particleBehavior?.Stop();
		Release();
	}

	public void Release()
	{
		if (isRelease)
		{
			return;
		}
		isRelease = true;
		managedPool?.Release(this);
		if (managedPool == null)
		{
			Destroy(gameObject);
		}
	}
}
