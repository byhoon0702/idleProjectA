﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Move State", menuName = "Unit State/Move", order = 1)]
public class UnitMoveState : UnitFSM
{

	public override FSM OnEnter()
	{
		if (SpawnManager.it != null && owner is EnemyUnit)
		{
			owner.PlayAnimation(StateType.MOVE);

			owner.unitAnimation.SetParameter("moveSpeed", Mathf.Min(owner.MoveSpeed, 2f));
			owner.unitAnimation.PlayParticle();
		}
		else
		{
			owner.PlayAnimation(StateType.MOVE);

			owner.unitAnimation.SetParameter("moveSpeed", Mathf.Min(owner.MoveSpeed, 2f));
			owner.unitAnimation.PlayParticle();
		}
		return this;
	}

	public override void OnExit()
	{
		if (SpawnManager.it != null && owner is EnemyUnit)
		{
			owner.PlayAnimation(StateType.IDLE);
			owner.unitAnimation.StopParticle();
		}
		else
		{
			owner.PlayAnimation(StateType.IDLE);
			owner.unitAnimation.StopParticle();
		}
	}

	public override void OnUpdate(float time)
	{



	}

	public override void OnFixedUpdate(float fixedTime)
	{
		if (owner.IsTargetAlive() == false)
		{
			owner.ChangeState(StateType.IDLE, true);
			return;
		}

		if (owner.IsTargetInPursuitRange() == false)
		{
			owner.FindTarget(fixedTime, true, owner.PursuitRange);
		}

		var targetCollider = owner.target.unitAnimation.collider2d;
		//float distance = Vector3.Distance(owner.target.transform.position, owner.transform.position);

		float distance;
		float additionalRange = 0;

		if (owner.TargetInRange(owner.target, out distance, out additionalRange))
		{
			owner.ChangeState(StateType.ATTACK);
			return;
		}

		owner.unitAnimation.SetParameter("moveSpeed", Mathf.Min(owner.MoveSpeed, 3f));
		if (owner.unitAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("run") == false)
		{
			owner.PlayAnimation(StateType.MOVE);
			return;
		}
		owner.OnMove(fixedTime);


	}
}
