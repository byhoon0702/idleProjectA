using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Ground Area Behavior", menuName = "ScriptableObject/Damage/Ground Area")]
public class GroundAreaDamageBehavior : AppliedDamageBehavior
{
	public float splashAreaSize = 3f;
	public override bool ReachedTarget(Vector3 pos, Vector3 targetPos)
	{
		return false;
	}
	public override void ApplyingDamageToTarget(SkillEffectInfo skillinfo, UnitBase unit, Vector3 casterPos, AffectedInfo hitInfo, BaseAbilitySO baseAbilitySO)
	{
		List<UnitBase> units = new List<UnitBase>();
		for (int i = 0; i < targetFilterBehavior.Count; i++)
		{
			units.AddRange(targetFilterBehavior[i].GetObject());
		}
		for (int i = 0; i < units.Count; i++)
		{
			Vector3 pos = units[i].position;
			pos.y = 0;
			float distance = Vector3.Distance(pos, casterPos);
			if (distance < splashAreaSize)
			{
				baseAbilitySO?.OnTrigger(skillinfo, units[i] as Unit, hitInfo);

			}
		}
	}
	public override void ApplyingDamage(SkillEffectInfo skillinfo, UnitBase targets, AffectedInfo hitInfo, BaseAbilitySO baseAbilitySO)
	{
		List<UnitBase> units = GetFilterd();
		for (int i = 0; i < units.Count; i++)
		{
			Vector3 pos = units[i].position;
			pos.y = 0;
			float distance = Vector3.Distance(pos, targets.transform.position);
			if (distance < splashAreaSize)
			{
				baseAbilitySO?.OnTrigger(skillinfo, units[i] as Unit, hitInfo);

			}
		}
	}
	public override void ApplyingDamage(SkillEffectInfo skillinfo, Vector3 targetPos, AffectedInfo hitInfo, BaseAbilitySO baseAbilitySO)
	{
		List<UnitBase> units = GetFilterd();
		for (int i = 0; i < units.Count; i++)
		{
			Vector3 pos = units[i].position;
			pos.y = 0;
			float distance = Vector3.Distance(pos, targetPos);
			if (distance < splashAreaSize)
			{
				baseAbilitySO?.OnTrigger(skillinfo, units[i] as Unit, hitInfo);

			}
		}
	}
}
