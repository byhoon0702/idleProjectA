using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkillAction : BaseSkillAction
{
	public override void Cancel()
	{
		particleEffect.Stop();
	}
	public override void OnSet(SkillEffectObject skillEffectObj, SkillObjectInfo skillinfo)
	{
		base.OnSet(skillEffectObj, skillinfo);


		int count = 1;
		if (skillinfo != null && skillinfo.infoList.Length > 0)
		{
			count = skillinfo.infoList.Length;
		}

		if (skillEffectObj.caster != null)
		{

			var hitinfo = new HealInfo(AttackerType.Player, power / count);
			skillEffectObj.SetAffectedInfo(hitinfo);
			skillEffectObj.transform.position = skillEffectObj.caster.position;
			ShowParticleEffect(skillEffectObj.caster is PlayerUnit, skillEffectObj.caster.position, 1, skillInfo.spawnFxResource, "");
		}
	}
	public override void OnUpdate(SkillEffectObject skillEffectObj, float time)
	{
		skillEffectAction?.OnUpdate(skillEffectObj, time, OnTrigger);
	}
	public override void OnTrigger(SkillEffectObject skillEffectObj)
	{

	}

	public override void OnTrigger(SkillEffectInfo info, HittableUnit targets, AffectedInfo hitinfo)
	{

		if (targets.IsAlive())
		{
			targets.Heal((HealInfo)hitinfo);
		}
	}


}
