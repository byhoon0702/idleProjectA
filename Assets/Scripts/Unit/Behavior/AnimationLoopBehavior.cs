using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimationLoopBehavior : StateMachineBehaviour
{

	private int currentLoopIndex = 0;
	private Animator myanimator;
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		currentLoopIndex = 0;
		if (myanimator == null)
		{
			myanimator = animator;
		}
	}
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
	{
		if (myanimator == null)
		{
			myanimator = animator;
		}
		if (Mathf.FloorToInt(stateInfo.normalizedTime) > currentLoopIndex)
		{
			currentLoopIndex++;



		}

	}



}
