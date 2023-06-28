using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Idle State", menuName = "Unit State/Idle", order = 1)]
public class UnitIdleState : UnitFSM
{
	public override FSM OnEnter()
	{
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

		owner.FindTarget(time, false);
		if (owner.IsTargetAlive() == false)
		{
			owner.FindTarget(time, true);
			if (owner.IsTargetAlive() == false)
			{
				return;
			}
		}


		if (owner.IsTargetAlive())
		{
			owner.ChangeState(StateType.MOVE);
		}
	}

	public override void OnFixedUpdate(float fixedTime)
	{

	}
}
