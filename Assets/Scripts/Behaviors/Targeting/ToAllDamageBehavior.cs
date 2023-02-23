using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Damage To All Target Behavior", menuName = "ScriptableObject/Damage/To All")]
public class ToAllDamageBehavior : AppliedDamageBehavior
{

	public override bool ReachedTarget(Vector3 pos, Vector3 targetPos)
	{
		return false;
	}

	public override void ApplyingDamageToTarget(SkillEffectInfo skillinfo, UnitBase unit, Vector3 casterPos, AffectedInfo hitInfo, BaseAbilitySO baseAbilitySO)
	{
		List<UnitBase> units = GetFilterd();


		for (int i = 0; i < units.Count; i++)
		{
			baseAbilitySO?.OnTrigger(skillinfo, units[i] as Unit, hitInfo);
		}

	}
	public override void ApplyingDamage(SkillEffectInfo skillinfo, UnitBase targets, AffectedInfo hitInfo, BaseAbilitySO baseAbilitySO)
	{
		List<UnitBase> units = GetFilterd();


		for (int i = 0; i < units.Count; i++)
		{
			baseAbilitySO?.OnTrigger(skillinfo, units[i] as Unit, hitInfo);
		}
	}
	public override void ApplyingDamage(SkillEffectInfo skillinfo, Vector3 pos, AffectedInfo hitInfo, BaseAbilitySO baseAbilitySO)
	{
		List<UnitBase> units = GetFilterd();


		for (int i = 0; i < units.Count; i++)
		{
			baseAbilitySO?.OnTrigger(skillinfo, units[i] as Unit, hitInfo);
		}
	}

}
