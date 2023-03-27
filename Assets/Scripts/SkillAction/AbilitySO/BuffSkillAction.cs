using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkillAction : BaseSkillAction
{
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

	}


}
