using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalDamageModule
{
	public AdditionalDamageInfo info;
	public HitInfo hitInfo;
	public float interval;

	private int totalCount;
	private float elapsedTime;
	private int count = 0;

	protected HittableUnit unit;
	private GameObject particle;
	public void Set(HittableUnit _unit, AdditionalDamageInfo _info, HitInfo _hitInfo)
	{
		unit = _unit;
		info = _info;
		hitInfo = _hitInfo;


		totalCount = info.damageCount;
		interval = info.duration / totalCount;

		if (info.particle != null)
		{
			particle = Object.Instantiate(info.particle);
			particle.transform.position = unit.transform.position;
		}
	}

	public void AddTime(int damageCount)
	{
		totalCount += damageCount;
	}

	public void RemoveParticle()
	{
		if (particle != null)
		{
			Object.Destroy(particle);
		}
	}

	public virtual void OnUpdate(float delta)
	{
		if (count >= totalCount)
		{

			return;
		}
		if (elapsedTime > interval)
		{
			//unit.Hit(hitInfo);
			count++;
			elapsedTime = 0;
		}

		elapsedTime += delta;
	}

	public virtual bool isEnd()
	{
		return count >= totalCount;
	}
}
