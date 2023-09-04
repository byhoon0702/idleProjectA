using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Debuff Skill", menuName = "ScriptableObject/Skill/Debuff", order = 1)]
public class Debuff : SkillCore
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

		DebuffInfo info = new DebuffInfo(_caster.gameObject.layer, _info.Tid, _info.Duration, _info.skillAbility);
		_caster.StartCoroutine(Activation(_caster, _info, info));
		return true;
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

	protected override void OnAffectTarget(Unit caster, HittableUnit target, AffectedInfo hitInfo, float knockbackPower, Vector3 knockbackDir, RuntimeData.SkillInfo skillInfo)
	{
		DebuffTarget(caster, target, skillInfo, hitInfo as DebuffInfo);
	}



	private void DebuffTarget(Unit caster, HittableUnit target, RuntimeData.SkillInfo skillInfo, DebuffInfo info)
	{
		SoundManager.Instance.PlayEffect(audioClip);
		target.Debuff(info);
	}
}
