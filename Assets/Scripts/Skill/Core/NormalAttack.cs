using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu(fileName = "Normal Attack", menuName = "Skill/Ability/Normal Attack", order = 1)]
public class NormalAttack : SkillAbility
{
	public override IEnumerator DO(Transform parent, Unit caster, Vector3 targetPos, AffectedInfo hitInfo, System.Action onComplete)
	{
		int index = 0;
		float time = Time.realtimeSinceStartup;
		if (effect != null)
		{
			GameObject go = Instantiate(effect);
			go.transform.SetParent(parent);
			go.transform.position = caster.transform.position;
			Vector3 scale = Vector3.one;
			scale.x *= caster.currentDir;
			go.transform.localScale = scale;
			go.transform.localRotation = caster.unitAnimation.transform.localRotation;
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
