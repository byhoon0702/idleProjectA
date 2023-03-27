using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class AppliedSkillBehavior : ScriptableObject
{
	public List<TargetFilterBehaviorSO> targetFilterBehavior;

	public delegate void OnTriggerDamage(SkillEffectInfo skill, HittableUnit unit, AffectedInfo info);
	protected List<UnitBase> GetFilterd()
	{
		List<UnitBase> units = new List<UnitBase>();
		for (int i = 0; i < targetFilterBehavior.Count; i++)
		{
			units.AddRange(targetFilterBehavior[i].GetObject());
		}

		return units;
	}
	public virtual bool ReachedTarget(Vector3 pos, Vector3 targetPos)
	{
		return false;
	}

	/// <summary>
	/// 투사체가 있는 경우 혹은 거리 계산이 필요한 경우
	/// </summary>
	/// <param name="targets"></param>
	/// <param name="casterPos"></param>
	/// <param name="hitInfo"></param>
	/// <param name="baseAbilitySO"></param>
	public virtual void ApplyingDamageToTarget(SkillEffectInfo skillinfo, UnitBase targets, Vector3 casterPos, AffectedInfo hitInfo, OnTriggerDamage onAction)
	{

	}

	/// <summary>
	/// 근접 혹은 즉시 발동
	/// </summary>
	/// <param name="units"></param>
	/// <param name="hitInfo"></param>
	/// <param name="baseAbilitySO"></param>
	public virtual void ApplyingDamage(SkillEffectInfo skillinfo, UnitBase targets, AffectedInfo hitInfo, OnTriggerDamage onAction)
	{
	}
	public virtual void ApplyingDamage(SkillEffectInfo skillinfo, Vector3 pos, AffectedInfo hitInfo, OnTriggerDamage onAction)
	{
	}


}
