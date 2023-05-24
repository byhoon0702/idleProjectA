using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[CreateAssetMenu(fileName = "Directional Attack", menuName = "Skill/Ability/Directional Attack", order = 1)]
public class DirectionalAttack : SkillAbility
{
	public bool fromCaster = true;

	public override IEnumerator DO(Transform parent, Unit caster, Vector3 targetPos, AffectedInfo hitInfo, System.Action onComplete)
	{
		int index = 0;
		float time = Time.realtimeSinceStartup;
		if (effect != null)
		{
			GameObject go = Instantiate(effect);
			go.transform.SetParent(parent);
			if (fromCaster)
			{
				go.transform.position = caster.transform.position;
				go.transform.forward = caster.headingDirection;
			}
			else
			{
				go.transform.position = targetPos;
			}
		}

		while (index <= 0)
		{
			if (Time.realtimeSinceStartup - time > affectInfo.timing)
			{
				affectInfo.screenEffect?.DoEffect();
				yield return action?.Action(parent, affectInfo, caster, targetPos, hitInfo, layerMask);
				index++;
			}
			if (isSync == false)
			{
				yield return null;
			}
		}

		onComplete?.Invoke();
	}
}
