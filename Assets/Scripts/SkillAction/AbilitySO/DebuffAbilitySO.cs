using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Debuff Skill", menuName = "ScriptableObject/Skill/Debuff", order = 2)]
public class DebuffAbilitySO : BaseAbilitySO
{
	public DebuffInfo debuffInfo;
	public override void OnSet(SkillEffectObject skillEffectObj, IdleNumber power)
	{
		base.OnSet(skillEffectObj, power);

		if (skillEffectObj.caster != null)
		{
			debuffInfo = new DebuffInfo();
			skillEffectObj.SetAffectedInfo(debuffInfo);
			skillEffectObj.transform.position = skillEffectObj.caster.position;
		}
	}

	public override void OnUpdate(SkillEffectObject skillEffectObj, float time)
	{

	}

	public override void OnTrigger(SkillEffectObject skillEffectObj)
	{

	}

	public override void OnTrigger(SkillEffectInfo info, Unit targets, AffectedInfo hitinfo)
	{
		DebuffInfo debuff = (DebuffInfo)hitinfo;


		targets.Debuff(debuff.infos);
	}
}
