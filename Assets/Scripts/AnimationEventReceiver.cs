using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[RequireComponent(typeof(Animator))]
public class AnimationEventReceiver : MonoBehaviour
{

	[SerializeField] Unit owner;

	public void Init(Unit _owner)
	{
		owner = _owner;
	}
	public void Attack()
	{
		owner?.Attack();
	}

	public void UseSkill()
	{

		owner?.UseSkill();
	}

	public void EndHyper()
	{
		if (owner is PlayerUnit)
		{
			if ((owner as PlayerUnit).ignoreAnimationEndEvent)
			{
				return;
			}
		}
		owner?.EndHyper();
	}

	public void SpawnEffect()
	{

	}

	public void CameraShake(UnityEngine.Object effect)
	{
		if (effect is not SkillScreenEffect)
		{
			return;
		}
		(effect as SkillScreenEffect).DoEffect();
	}

	public void MinusLoopCount()
	{
		var animator = GetComponent<Animator>();
		int count = animator.GetInteger("attackLoop");
		count--;
		animator.SetInteger("attackLoop", count);
	}
}
