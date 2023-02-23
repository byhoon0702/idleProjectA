using System.Collections;
using System.Collections.Generic;
using System.Linq;
using h_info;
using h_story_group_meta;
using log4net.Util;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee Skill", menuName = "ScriptableObject/Skill/Melee", order = 2)]
public class MeleeActionSO : SkillEffectActionSO
{
	public override void OnUpdate(SkillEffectObject skillEffectObj, float time)
	{
		var infoList = skillEffectObj.baseAbilitySO.infoList;
		if (infoList.Count > skillEffectObj.index)
		{
			SkillEffectInfo _data = infoList[skillEffectObj.index];

			if (time >= _data.timing)
			{
				if (skillEffectObj.caster != null)
				{
					ShowHitEffect(skillEffectObj.caster, _data.hitFxResource.name);
					_data.appliedDamageBehavior.ApplyingDamage(_data, skillEffectObj.caster.target, skillEffectObj.hitinfo, skillEffectObj.baseAbilitySO);
				}

				skillEffectObj.index++;
			}
		}
		else
		{
			skillEffectObj.Reset();
		}
	}



}

