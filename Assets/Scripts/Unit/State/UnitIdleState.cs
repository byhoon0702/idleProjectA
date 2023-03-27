using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Idle State", menuName = "Unit State/Idle", order = 1)]
public class UnitIdleState : UnitFSM
{


	public void Init(Unit _owner)
	{
		owner = _owner;
	}
	public override void OnEnter()
	{

	}
	public override void OnEnter<T>(T data = default)
	{
		owner.PlayAnimation(StateType.IDLE);
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
				owner.ChangeState(StateType.MOVE);
				return;
			}

		}

		float distance = Mathf.Abs(owner.target.transform.position.x - owner.transform.position.x);

		if (distance > owner.SearchRange)
		{
			owner.ChangeState(StateType.MOVE);

		}
		else
		{
			owner.ChangeState(StateType.ATTACK);
		}
	}
}
