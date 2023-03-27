using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Airborne Effect", menuName = "Skill/Status Effect/Neutralize/Airborne")]
public class SkillAirborne : SkillNeutralizeEffect
{
	public float power;
	public override void ApplyEffect(HittableUnit target, Vector3 centerPos, int hitCount, bool isLastHit = true)
	{
		target.AirBorne(power, hitCount, isLastHit);
	}
}
