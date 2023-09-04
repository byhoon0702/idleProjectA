using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pet Idle State", menuName = "Pet State/Idle", order = 1)]

public class PetFSM : UnitFSM
{
	protected Pet pet;

	public override FSM OnEnter()
	{
		pet = owner as Pet;
		return this;
	}

	public override void OnExit()
	{
	}

	public override void OnFixedUpdate(float fixedTime)
	{
	}

	public override void OnUpdate(float time)
	{
	}
}

public class PetIdleState : PetFSM
{

	public override FSM OnEnter()
	{
		base.OnEnter();
		owner.PlayAnimation(StateType.IDLE);

		return this;
	}


	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		if (GameManager.GameStop)
		{
			return;
		}

		if (pet.IsFollowAlive() == false)
		{
			return;
		}

		if (pet.IsNextToFollow() == false)
		{
			owner.ChangeState(StateType.MOVE);
		}
	}

	public override void OnFixedUpdate(float fixedTime)
	{

	}
}
