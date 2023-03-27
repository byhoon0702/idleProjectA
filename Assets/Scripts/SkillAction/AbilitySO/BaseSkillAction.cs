
using System.Configuration;
using UnityEngine;

public abstract class BaseSkillAction
{
	public SkillEffectAction skillEffectAction;
	public SkillObjectInfo skillInfo;

	protected IdleNumber power;

	protected ParticleEffect particleEffect;
	protected SkillEffectObject skillEffectObj;
	public abstract void Cancel();

	public abstract void OnUpdate(SkillEffectObject skillEffectObj, float time);
	public virtual void OnSet(SkillEffectObject _skillEffectObj, SkillObjectInfo _skillInfo)
	{
		skillEffectObj = _skillEffectObj;
		skillEffectObj.index = 0;
		skillInfo = _skillInfo;
		switch (skillInfo.skillActionType)
		{
			case SkillActionType.MELEE:
				skillEffectAction = new MeleeEffectAction();
				break;
			case SkillActionType.RANGED:
				skillEffectAction = new ProjectileEffectAction();
				break;
			case SkillActionType.SUMMON:
				skillEffectAction = new SummonEffectAction();
				break;
		}

		skillEffectAction.InitInfo(skillInfo);
	}

	public abstract void OnTrigger(SkillEffectObject skillEffectObj);

	public abstract void OnTrigger(SkillEffectInfo info, HittableUnit targets, AffectedInfo hitinfo);

	protected bool PlayParticleEffect(bool isPlayer, Vector3 pos, float speed, GameObject particleEffect, string path = PathHelper.hyperCasualFXPath)
	{
		int side = 1;
		if (isPlayer == false)
		{
			side = -1;
		}
		if (ParticleEffectPoolManager.it != null)
		{
			var effect = ParticleEffectPoolManager.it.Get(particleEffect.name);
			if (effect == null)
			{
				GameObject deffect = GameObject.Instantiate(particleEffect);

				deffect.transform.position = pos;
				Vector3 sscale = deffect.transform.localScale;
				sscale.x = Mathf.Abs(sscale.x) * side;
				deffect.transform.localScale = sscale;
				ParticleEffect comp = deffect.GetComponent<ParticleEffect>();
				if (comp != null)
				{
					comp.Play(speed);
				}
				this.particleEffect = comp;
				return false;
			}
			effect.gameObject.SetActive(true);
			effect.transform.position = pos;
			Vector3 scale = effect.transform.localScale;
			scale.x = Mathf.Abs(scale.x) * side;
			effect.transform.localScale = scale;
			effect.Play();
			this.particleEffect = effect;
			return true;
		}
		else
		{

			GameObject effect = GameObject.Instantiate(particleEffect);

			effect.transform.position = pos;
			Vector3 scale = effect.transform.localScale;
			scale.x = Mathf.Abs(scale.x) * side;
			effect.transform.localScale = scale;
			ParticleEffect comp = effect.GetComponent<ParticleEffect>();
			if (comp != null)
			{
				comp.Play(speed);
			}
			this.particleEffect = comp;
		}
		return false;
	}

	protected bool PlayFromPool(Vector3 pos, string hitEffect, string path = PathHelper.hyperCasualFXPath)
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
			if (path.IsNullOrEmpty() == false)
			{
				path += "/";
			}
			GameObject effect = (GameObject)GameObject.Instantiate(Resources.Load($"{path}{hitEffect}"));
			effect.transform.position = pos;
			HitEffect comp = effect.GetComponent<HitEffect>();
			if (comp != null)
			{
				comp.Play();
			}
		}
		return false;
	}
	protected void ShowHitEffect(Vector3 pos, string resource)
	{
		if (resource.IsNullOrEmpty() == false)
		{
			PlayFromPool(pos, resource);
		}
	}

	protected void ShowParticleEffect(bool isPlayer, Vector3 pos, float speed, GameObject resource, string path = "")
	{
		if (resource != null)
		{
			PlayParticleEffect(isPlayer, pos, speed, resource, path);
		}
	}
}
