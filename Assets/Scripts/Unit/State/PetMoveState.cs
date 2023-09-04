using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pet Move State", menuName = "Pet State/Move", order = 1)]
public class PetMoveState : PetFSM
{

	public override FSM OnEnter()
	{
		base.OnEnter();

		return this;
	}

	public override void OnExit()
	{

		owner.PlayAnimation(StateType.IDLE);

	}

	public override void OnUpdate(float time)
	{



	}

	public override void OnFixedUpdate(float fixedTime)
	{
		if (pet.IsFollowAlive() == false)
		{
			owner.ChangeState(StateType.IDLE, true);
			return;
		}

		if (pet.IsNextToFollow())
		{
			owner.ChangeState(StateType.IDLE, true);
			return;
		}

		if (owner.unitAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("run") == false)
		{
			owner.PlayAnimation(StateType.MOVE);
			return;
		}
		owner.OnMove(fixedTime);
	}
}
