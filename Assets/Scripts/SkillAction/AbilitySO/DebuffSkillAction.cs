using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffSkillAction : BaseSkillAction
{
	public DebuffInfo debuffInfo;
	public override void OnSet(SkillEffectObject skillEffectObj, SkillObjectInfo skillinfo)
	{
		base.OnSet(skillEffectObj, skillinfo);

		if (skillEffectObj.caster != null)
		{
			debuffInfo = new DebuffInfo();
			skillEffectObj.SetAffectedInfo(debuffInfo);
			skillEffectObj.transform.position = skillEffectObj.caster.position;

			ShowParticleEffect(skillEffectObj.caster is PlayerUnit, skillEffectObj.caster.position, 1, skillInfo.spawnFxResource, "");
		}
	}
	public override void Cancel()
	{
		particleEffect.Stop();
	}
	public override void OnUpdate(SkillEffectObject skillEffectObj, float time)
	{

	}

	public override void OnTrigger(SkillEffectObject skillEffectObj)
	{

	}

	public override void OnTrigger(SkillEffectInfo info, HittableUnit targets, AffectedInfo hitinfo)
	{
		DebuffInfo debuff = (DebuffInfo)hitinfo;


		targets.Debuff(debuff.infos);
	}
}
