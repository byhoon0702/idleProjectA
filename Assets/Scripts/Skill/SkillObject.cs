
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Pool;

public class SkillObject : MonoBehaviour
{
	public SkillAbility[] skillAbility;
	public AnimatorOverrideController animatorOverrideController;
	public SkillAbility first
	{
		get
		{
			if (skillAbility == null || skillAbility.Length == 0)
			{
				return null;
			}
			return skillAbility[0];
		}
	}

	private int index = 0;

	IObjectPool<GameObject> manangedPool;
	public void Set(IObjectPool<GameObject> _manangedPool)
	{
		manangedPool = _manangedPool;
	}


	public void Trigger(Unit caster, Vector3 _targetPos, AffectedInfo affectedInfo, Action onComplete)
	{

		index = 0;

		StartCoroutine(DO(caster, _targetPos, affectedInfo, onComplete));
	}



	private IEnumerator DO(Unit caster, Vector3 targetPos, AffectedInfo affectedInfo, Action onComplete)
	{
		while (skillAbility.Length > index)
		{
			yield return StartCoroutine(skillAbility[index].DO(transform, caster, targetPos, affectedInfo, null));
			index++;
		}

		onComplete?.Invoke();
		yield return new WaitForSeconds(1);
		Release();
	}


	public void Release()
	{
		if (gameObject != null)
		{
			Destroy(gameObject);
		}

	}

}
