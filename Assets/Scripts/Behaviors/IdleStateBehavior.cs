
using UnityEngine;
using UnityEngine.Animations;


public class IdleStateBehavior : StateMachineBehaviour
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
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
	{




	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}
}
