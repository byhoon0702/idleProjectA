using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HittableUnit : UnitBase
{
	public abstract IdleNumber Hp { get; set; }
	public abstract IdleNumber MaxHp { get; }



	public GameObject hitEffect;

	public GameObject hitEffectObject;
	public GameObject attackEffectObject;

	public virtual bool IsAlive()
	{
		return Hp > 0;
	}

	public virtual void Hit(HitInfo _hitInfo, RuntimeData.SkillInfo _skillInfo)
	{

	}

	public virtual void Heal(HealInfo _healInfo)
	{

	}

	public virtual void Debuff(DebuffInfo debuffInfo)
	{
		buffModule.AddDebuff(debuffInfo);
	}

	public virtual void Buff(BuffInfo buffInfo)
	{
		buffModule.AddBuff(buffInfo);
	}


	public virtual void AddBuff(AppliedBuff info)
	{

	}

	public virtual void AddDebuff(AppliedBuff info)
	{

	}
	public virtual void RemoveBuff(AppliedBuff key)
	{

	}

	public virtual void RemoveDebuff(AppliedBuff key)
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

	protected abstract void OnHit(HitInfo hitInfo);
	protected abstract IEnumerator OnHitRoutine(HitInfo hitInfo);


}
