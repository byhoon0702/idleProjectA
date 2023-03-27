using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AttackSkillAction : BaseSkillAction
{
	public override void Cancel()
	{
		if (particleEffect != null)
		{
			particleEffect.Stop();
		}
	}

	public override void OnSet(SkillEffectObject skillEffectObj, SkillObjectInfo skillInfo)
	{
		base.OnSet(skillEffectObj, skillInfo);

		int count = 1;
		if (skillInfo != null && skillInfo.infoList.Length > 0)
		{
			count = skillInfo.infoList.Length;
		}
		if (skillEffectObj.caster != null)
		{
			var hitinfo = new HitInfo(skillEffectObj.caster, skillEffectObj.caster.AttackPower);
			UnitStats stats = skillEffectObj.caster.GetStats.Clone();

			//stats[Ability.Attackpower] = hitinfo.TotalAttackPower;

			IdleNumber result = stats[Ability.Attackpower].GetValue();

			result = skillInfo.Calculate(result, stats, 1, new StatInfo(Ability.Attackpower, (IdleNumber)100));

			hitinfo = new HitInfo(skillEffectObj.caster, result, hitinfo.isSkill);

			skillEffectObj.SetAffectedInfo(hitinfo);
			ShowParticleEffect(hitinfo.IsPlayerCast, skillEffectObj.caster.position, skillEffectObj.caster.AttackSpeedMul, skillInfo.spawnFxResource, PathHelper.particleEffectPath);
		}
	}



	//애니메이션, Update 등에서 호출되는 함수
	public override void OnUpdate(SkillEffectObject skillEffectObj, float time)
	{
		skillEffectAction?.OnUpdate(skillEffectObj, time, OnTrigger);
	}

	public override void OnTrigger(SkillEffectObject skillEffectObj)
	{

	}

	//타겟에게 영향을 미치는 시점에 호출 
	public override void OnTrigger(SkillEffectInfo info, HittableUnit targets, AffectedInfo hitinfo)
	{
		if (targets == null)
		{
			return;
		}
		if (info.damageCount > 1)
		{
			MultiHit(info, targets, (HitInfo)hitinfo);
		}
		else
		{
			if (targets.IsAlive())
			{
				Vector3 pos = targets.GetPos(info.hitEffectPos);
				var hit = (HitInfo)hitinfo;
				ShowParticleEffect(targets is EnemyUnit, pos, 1, info.hitFxResource, PathHelper.particleEffectPath);
				targets.Hit(hit);
				if (info.skillNeutralizeEffects != null)
				{
					for (int i = 0; i < info.skillNeutralizeEffects.Length; i++)
					{
						info.skillNeutralizeEffects[i].ApplyEffect(targets, skillEffectObj.caster.position, 1);
					}
				}
				if (info.skillAdditionalDamages != null)
				{
					for (int i = 0; i < info.skillAdditionalDamages.Length; i++)
					{
						info.skillAdditionalDamages[i].ApplyEffect(skillEffectObj.caster as HittableUnit, targets);
					}
				}
			}
		}



	}

	async void MultiHit(SkillEffectInfo info, HittableUnit targets, HitInfo hitInfo)
	{
		int damageCount = info.damageCount > 0 ? info.damageCount : 1;
		IdleNumber result = hitInfo.TotalAttackPower / damageCount;

		HitInfo newHitInfo = new HitInfo(result);
		newHitInfo.IsPlayerCast = hitInfo.IsPlayerCast;

		for (int i = 0; i < damageCount; i++)
		{
			if (targets == null)
			{
				break;
			}
			if (targets.IsAlive() == false)
			{
				break;
			}
			targets.Hit(newHitInfo);
			Vector3 pos = targets.GetPos(info.hitEffectPos);
			ShowParticleEffect(newHitInfo.IsPlayerCast, pos, 1, info.hitFxResource, PathHelper.particleEffectPath);

			if (info.skillNeutralizeEffects != null)
			{
				for (int ii = 0; ii < info.skillNeutralizeEffects.Length; ii++)
				{
					info.skillNeutralizeEffects[ii].ApplyEffect(targets, skillEffectObj.caster.position, damageCount, i + 1 == damageCount);
				}
			}
			await Task.Delay((int)(1000 * info.damageInterval));
		}
	}
}
