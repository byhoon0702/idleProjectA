using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Skill Knock back Effect", menuName = "Skill/Status Effect/Neutralize/Knockback")]
public class SkillKnockback : SkillNeutralizeEffect
{
	public float power;

	public override void ApplyEffect(HittableUnit target, Vector3 centerPos, int hitCount, bool isLastHit = true)
	{
		Vector3 dir = (target.transform.position - centerPos).normalized;
		target.KnockBack(power, dir, hitCount, isLastHit);
	}
}
