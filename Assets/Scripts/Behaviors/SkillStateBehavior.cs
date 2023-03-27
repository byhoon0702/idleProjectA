using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class SkillStateBehavior : StateMachineBehaviour
{
	public UnitBase unitbase;

#if UNITY_EDITOR
	public bool isLoop;
#else
	private bool isLoop = false;
#endif
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		unitbase = animator.transform.GetComponentInParent<UnitBase>();

		//unitbase.SetAttack();
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
	{

		animator.SetFloat("attackSpeed", unitbase.AttackSpeedMul);

		unitbase.OnUpdateAttack(stateInfo.normalizedTime);

	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (unitbase == null)
		{
			return;
		}
		unitbase.EndUpdateSkill();

#if UNITY_EDITOR
		if (isLoop)
		{
			animator.Play(stateInfo.fullPathHash);
		}
#endif
	}
}
