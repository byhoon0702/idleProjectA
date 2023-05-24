using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

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
		if (SpawnManager.it != null && owner is EnemyUnit)
		{
			owner.unitAnimation.SetParameter("moveSpeed", Mathf.Min(owner.MoveSpeed, 2f));
			owner.OnMove(fixedTime);
		}
		else
		{
			owner.unitAnimation.SetParameter("moveSpeed", Mathf.Min(owner.MoveSpeed, 2f));
			if (owner.unitAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("run") == false)
			{
				owner.PlayAnimation(StateType.MOVE);
				return;
			}
			owner.OnMove(fixedTime);
		}


		float distance = Mathf.Abs((owner.target.transform.position - owner.transform.position).magnitude);

		if (distance <= 1)
		{
			owner.ChangeState(StateType.ATTACK);
		}
	}
}
