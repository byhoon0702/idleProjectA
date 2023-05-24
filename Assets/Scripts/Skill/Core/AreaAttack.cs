using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AreaAttack : SkillAbility
{

	public override bool TriggerAbility(Transform skillTransform, Unit caster, Vector3 targetPos)
	{
		if (bornEffect != null)
		{
			GameObject go = Instantiate(bornEffect);
			go.transform.position = targetPos;
		}


		//HitInfo hit = new HitInfo(affectValue);

		//var colliders = Physics.OverlapSphere(targetPos, hitRange, layerMask);

		//for (int i = 0; i < colliders.Length; i++)
		//{
		//	colliders[i].GetComponent<Unit>().Hit(hit);
		//}

		return true;
	}
}
