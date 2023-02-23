using System.Collections;
using System.Collections.Generic;
using h_story_group_meta;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Skill", menuName = "ScriptableObject/Skill/Projectile", order = 2)]
public class ProjectileActionSO : SkillEffectActionSO
{
	public override void OnUpdate(SkillEffectObject skillEffectObj, float time)
	{
		var infoList = skillEffectObj.baseAbilitySO.infoList;
		if (infoList.Count > skillEffectObj.index)
		{
			SkillEffectInfo _data = infoList[skillEffectObj.index];
			float timing = _data.timing;
			if (skillEffectObj.data.isDependent)
			{
				timing = _data.timing * skillEffectObj.duration;
			}
			if (time >= timing)
			{
				SkillObject skillAttackObject = ProjectileManager.it.Create(skillEffectObj.pos, skillEffectObj.caster.target, _data, skillEffectObj.hitinfo, skillEffectObj.data.speed);
				skillAttackObject.SetAction((behavior, pos, hit) =>
				{

					behavior.ApplyingDamageToTarget(_data, skillEffectObj.caster.target, pos, hit, skillEffectObj.baseAbilitySO);

					if (skillAttackObject.targetUnit == null || skillAttackObject.targetUnit.IsAlive() == false)
					{
						skillAttackObject.Release(true);
						return;
					}
				}

				, _data.appliedDamageBehavior.ReachedTarget);

				skillEffectObj.index++;
			}
		}
		else
		{
			skillEffectObj.Reset();
		}
	}
}
