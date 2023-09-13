using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Skill", menuName = "ScriptableObject/Skill/Heal", order = 1)]
public class Heal : SkillCore
{
	public override bool Trigger(Unit _caster, RuntimeData.SkillInfo _info)
	{
		if (_caster == null)
		{
			return false;
		}
		if (allowedUnitState != StateType.NONE)
		{
			if (allowedUnitState.HasFlag(_caster.currentState) == false)
			{
				return false;
			}
		}
		IdleNumber power = _caster.HitInfo.TotalAttackPower;
		if (_caster is Pet)
		{
			power = UnitManager.it.Player.HitInfo.TotalAttackPower;
		}

		HealInfo healInfo = new HealInfo(_caster.gameObject.layer, _caster, power);
		if (_info.skillAbility.Value > 0)
		{
			healInfo.amount = power * ((_info.skillAbility.Value + 100) / 100f);
		}

		_caster.StartCoroutine(Activation(_caster, _info, healInfo));
		return true;
	}

	protected override IEnumerator Activation(Unit caster, RuntimeData.SkillInfo info, AffectedInfo hitInfo)
	{
		if (caster.target == null)
		{
			yield break;
		}
		if (caster.IsAlive() == false)
		{
			yield break;
		}

		if (hitInfo.targetLayer == LayerMask.NameToLayer("Enemy"))
		{
			var list = UnitManager.it.GetRandomEnemies(caster.position, info.AttackRange, info.TargetCount);

			for (int i = 0; i < list.Count; i++)
			{
				caster.skillModule.StartCoroutine(AffectNonTarget(caster, info, hitInfo, list[i].position));
			}
		}
		else
		{
			caster.SetTarget(UnitManager.it.Player);
			caster.skillModule.StartCoroutine(AffectNonTarget(caster, info, hitInfo, caster.target.position));
		}

		yield return null;
	}
	protected override IEnumerator AffectNonTarget(Unit caster, RuntimeData.SkillInfo info, AffectedInfo hitInfo, Vector3 targetPos)
	{
		GameObject effect = ShowEffect(caster, targetPos, info.EvolutionLevel);
		Vector3 pos = GetPos(caster, effect, targetPos);

		if (caster.IsAlive() == false)
		{
			yield break;
		}
		if (effectPosType == EffectPosType.Self)
		{
			pos = GetPos(caster, effect, targetPos);
		}

		OnAffect(caster, pos, info.HitRange, pos - caster.position, hitInfo, info);

		yield return null;

	}



	protected override void OnAffectTarget(Unit caster, HittableUnit target, AffectedInfo hitInfo, float knockbackPower, Vector3 knockbackDir, SkillInfo skillInfo)
	{
		HealTarget(caster, target, skillInfo, hitInfo);
	}

	private void HealTarget(Unit caster, HittableUnit target, RuntimeData.SkillInfo skillInfo, AffectedInfo affectedInfo)
	{
		HealInfo healInfo = affectedInfo as HealInfo;
		healInfo.healRecovery = (target.MaxHp * skillInfo.skillAbility.Value) / 100f;
		target.Heal(healInfo);
	}
}
