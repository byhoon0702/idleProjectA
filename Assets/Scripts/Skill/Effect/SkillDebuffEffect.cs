using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Skill Debuff Effect", menuName = "Skill/Status Effect/Debuff")]
[Serializable]
public class SkillDebuffEffect : SkillStatusEffect
{
	public string fourArithmetic;
	public StatsType[] calculator = new StatsType[0];
	public float duration;

	public override void ApplyEffect(HittableUnit targets)
	{

	}
}
