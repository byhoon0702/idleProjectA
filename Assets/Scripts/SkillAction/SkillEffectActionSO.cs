using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillEffectInfo
{
	//public int level;
	public float timing;
	public GameObject spawnFxResource;
	public GameObject hitFxResource;
	public GameObject objectResource;

	public float damageRate;
	public int damageCount;
	public float damageInterval;

	public SkillActionBehavior actionBehavior;
	public AppliedDamageBehavior appliedDamageBehavior;

	public SkillEffectInfo()
	{
		timing = 0;
		damageRate = 1;
		damageCount = 1;
		damageInterval = 0;
	}
}

public abstract class SkillEffectActionSO : ScriptableObject
{
	public float speed = 1;

	public abstract void OnUpdate(SkillEffectObject skillEffectObj, float time);
	protected bool PlayFromPool(Vector3 pos, string hitEffect)
	{
		if (HitEffectPoolManager.it != null)
		{
			var effect = HitEffectPoolManager.it.Get(hitEffect);
			if (effect == null)
			{
				return false;
			}
			effect.transform.position = pos;
			effect.Play();
			return true;
		}
		else
		{
			GameObject effect = (GameObject)Instantiate(Resources.Load($"{PathHelper.hyperCasualFXPath}/{hitEffect}"));
			effect.transform.position = pos;
			HitEffect comp = effect.GetComponent<HitEffect>();
			if (comp != null)
			{
				comp.Play();
			}
		}
		return false;
	}
	protected void ShowHitEffect(UnitBase caster, string resource)
	{
		if (resource.IsNullOrEmpty() == false)
		{
			Vector3 pos = new Vector3(-3, 0, 0);
			if (caster != null)
			{
				pos = caster.target == null ? caster.position : caster.target.position;


			}

			PlayFromPool(pos, resource);
		}
	}
}
