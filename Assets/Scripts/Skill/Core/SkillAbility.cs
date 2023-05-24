using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class SkillActionInfo
{
	public float timing;
	public float hitRange;

	public SkillScreenEffect screenEffect;
	public SkillActionInfo()
	{
		timing = 0.1f;
	}

	public void Do()
	{

	}
}

[CreateAssetMenu(fileName = "Skill Ability", menuName = "Skill/Skill Ability", order = 1)]
public class SkillAbility : ScriptableObject
{
	public LayerMask layerMask;
	public Ability usingAbility;
	public GameObject effect;

	public float searchRange;
	public SkillAction action;
	public bool isSync;
	public SkillActionInfo affectInfo;

	public GameObject bornEffect;

	public SkillAdditionalDamage skillAdditionalDamages;
	public SkillStatusEffect skillStatusEffects;
	///public SkillNeutralizeEffect skillNeutralizeEffects;

	public virtual bool TriggerAbility(Transform skillTransform, Unit caster, Vector3 targetPos)
	{

		return false;
	}

	public virtual IEnumerator DO(Transform parent, Unit caster, Vector3 targetPos, AffectedInfo affectedInfo, System.Action onComplete)
	{
		int index = 0;
		float time = Time.realtimeSinceStartup;
		if (effect != null)
		{
			GameObject go = Instantiate(effect);
			go.transform.SetParent(parent);
			go.transform.position = targetPos;
			Vector3 scale = Vector3.one;
			go.transform.localScale = scale;
			//go.transform.localRotation = caster.unitAnimation.transform.localRotation;
		}

		while (index <= 0)
		{
			if (Time.realtimeSinceStartup - time > affectInfo.timing)
			{
				affectInfo.screenEffect?.DoEffect();
				yield return action?.Action(parent, affectInfo, caster, targetPos, affectedInfo, layerMask);
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
