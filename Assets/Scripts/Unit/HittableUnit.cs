using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HittableUnit : UnitBase
{
	public abstract IdleNumber Hp { get; set; }
	public abstract IdleNumber MaxHp { get; }



	public GameObject hitEffect;


	public virtual bool IsAlive()
	{
		return Hp > 0;
	}

	public virtual void Hit(HitInfo _hitInfo)
	{

	}

	public virtual void Heal(HealInfo _healInfo)
	{

	}

	public virtual void Debuff(DebuffInfo _debuffInfo)
	{

	}
	public virtual void Buff()
	{

	}

	public virtual void Debuff(List<StatInfo> debufflist)
	{

	}

	public virtual void KnockBack(float power, Vector3 dir, int hitCount, bool isLastHit = true)
	{

	}
	public virtual void AirBorne(float power, int hitCount, bool isLastHit = true)
	{

	}

	public virtual void AdditionalDamage(AdditionalDamageInfo info, HitInfo hitinfo)
	{

	}
}
