using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateBehavior : StateMachineBehaviour
{
	private int moveSpeedHash;
	private UnitBase unitbase;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		moveSpeedHash = Animator.StringToHash("moveSpeed");

		if (unitbase == null)
		{
			unitbase = animator.GetComponentInParent<UnitBase>();
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetFloat(moveSpeedHash, unitbase.MoveSpeed);
	}

}
