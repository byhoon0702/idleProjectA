using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public abstract class AppliedDamageBehavior : ScriptableObject
{
	public List<TargetFilterBehaviorSO> targetFilterBehavior;


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
	public virtual void ApplyingDamageToTarget(SkillEffectInfo skillinfo, UnitBase targets, Vector3 casterPos, AffectedInfo hitInfo, BaseAbilitySO baseAbilitySO)
	{

	}

	/// <summary>
	/// 근접 혹은 즉시 발동
	/// </summary>
	/// <param name="units"></param>
	/// <param name="hitInfo"></param>
	/// <param name="baseAbilitySO"></param>
	public virtual void ApplyingDamage(SkillEffectInfo skillinfo, UnitBase targets, AffectedInfo hitInfo, BaseAbilitySO baseAbilitySO)
	{
	}
	public virtual void ApplyingDamage(SkillEffectInfo skillinfo, Vector3 pos, AffectedInfo hitInfo, BaseAbilitySO baseAbilitySO)
	{
	}


}
