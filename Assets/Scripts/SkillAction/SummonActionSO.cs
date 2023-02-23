using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
[CreateAssetMenu(fileName = "Summon Type Skill", menuName = "ScriptableObject/SkillType/Summon", order = 2)]
public class SummonActionSO : SkillEffectActionSO
{
	public override void OnUpdate(SkillEffectObject skillEffectObj, float time)
	{

		if (time > skillEffectObj.duration)
		{
			return;
		}
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
				//발사체가 특정 지점으로 날아간다.
				if (_data.objectResource == null)
				{
					if (skillEffectObj.caster != null && skillEffectObj.caster.target != null)
					{
						_data.appliedDamageBehavior.ApplyingDamage(_data, skillEffectObj.caster.target, skillEffectObj.hitinfo, skillEffectObj.baseAbilitySO);
					}
					else
					{
						_data.appliedDamageBehavior.ApplyingDamage(_data, skillEffectObj.targetPos, skillEffectObj.hitinfo, skillEffectObj.baseAbilitySO);
					}
					ShowHitEffect(skillEffectObj.caster, _data.hitFxResource.name);
				}
				else
				{
					SkillObject skillAttackObject = ProjectileManager.it.Create(skillEffectObj.pos, skillEffectObj.targetPos, _data, skillEffectObj.hitinfo, skillEffectObj.data.speed);
					skillAttackObject.SetAction((behavior, pos, hit) =>
					{
						behavior.ApplyingDamageToTarget(_data, skillEffectObj.caster.target, pos, hit, skillEffectObj.baseAbilitySO);
					}, _data.appliedDamageBehavior.ReachedTarget);
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
