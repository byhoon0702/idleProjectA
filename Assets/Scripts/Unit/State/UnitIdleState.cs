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

		owner.FindTarget(time, false);

		if (owner.IsTargetAlive() == false)
		{
			owner.FindTarget(time, true);

			if (owner.IsTargetAlive() == false)
			{

				return;
			}
		}

		float distance = Mathf.Abs((owner.target.transform.position - owner.transform.position).magnitude);

		if (distance > 1)
		{
			owner.ChangeState(StateType.MOVE);
		}
		else
		{
			owner.ChangeState(StateType.ATTACK);
		}
	}

	public override void OnFixedUpdate(float fixedTime)
	{

	}
}
