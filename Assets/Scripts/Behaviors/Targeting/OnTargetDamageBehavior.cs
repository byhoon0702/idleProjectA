using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

[CreateAssetMenu(fileName = "Damage On Target Behavior", menuName = "ScriptableObject/Damage/On Target")]
public class OnTargetDamageBehavior : AppliedDamageBehavior
{
	public float checkDistance = 1f;

	public override bool ReachedTarget(Vector3 pos, Vector3 targetPos)
	{
		float distance = Vector3.Distance(pos, targetPos);

		if (distance <= checkDistance)
		{
			return true;
		}

		return false;
	}
	public override void ApplyingDamageToTarget(SkillEffectInfo skillinfo, UnitBase unit, Vector3 casterPos, AffectedInfo hitInfo, BaseAbilitySO baseAbilitySO)
	{
		baseAbilitySO?.OnTrigger(skillinfo, unit as Unit, hitInfo);

		//for (int i = 0; i < units.Count; i++)
		//{
		//	Vector3 pos = units[i].position;
		//	pos.y = 0;
		//	float distance = Vector3.Distance(pos, casterPos);
		//	if (distance <= checkDistance)
		//	{
		//		baseAbilitySO?.OnTrigger(skillinfo, units[i], hitInfo);
		//	}

		//}
	}

	public override void ApplyingDamage(SkillEffectInfo skillinfo, UnitBase unit, AffectedInfo hitInfo, BaseAbilitySO baseAbilitySO)
	{
		baseAbilitySO?.OnTrigger(skillinfo, unit as Unit, hitInfo);
		//for (int i = 0; i < units.Count; i++)
		//{

		//}
	}
}
