using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Heal Skill", menuName = "ScriptableObject/Skill/Heal", order = 2)]
public class HealAbilitySO : BaseAbilitySO
{
	public override void OnSet(SkillEffectObject skillEffectObj, IdleNumber power)
	{
		base.OnSet(skillEffectObj, power);

		if (skillEffectObj.caster != null)
		{
			int count = 1;
			if (skillEffectObj.baseAbilitySO.infoList != null && skillEffectObj.baseAbilitySO.infoList.Count > 0)
			{
				count = skillEffectObj.baseAbilitySO.infoList.Count;
			}
			var hitinfo = new HealInfo(AttackerType.Player, power / count);
			skillEffectObj.SetAffectedInfo(hitinfo);
			skillEffectObj.transform.position = skillEffectObj.caster.position;
		}
		else
		{
			int count = 1;
			if (skillEffectObj.baseAbilitySO.infoList != null && skillEffectObj.baseAbilitySO.infoList.Count > 0)
			{
				count = skillEffectObj.baseAbilitySO.infoList.Count;
			}
			var healInfo = new HealInfo(AttackerType.Player, power / count);

			skillEffectObj.SetAffectedInfo(healInfo);
		}
	}
	public override void OnUpdate(SkillEffectObject skillEffectObj, float time)
	{
		skillEffectActionSO?.OnUpdate(skillEffectObj, time);
	}
	public override void OnTrigger(SkillEffectObject skillEffectObj)
	{

	}

	public override void OnTrigger(SkillEffectInfo info, Unit targets, AffectedInfo hitinfo)
	{

		if (targets.IsAlive())
		{
			targets.Heal((HealInfo)hitinfo);
		}
	}


}
