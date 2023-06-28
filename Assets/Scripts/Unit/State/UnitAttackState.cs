using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Attack State", menuName = "Unit State/Attack", order = 1)]
public class UnitAttackState : UnitFSM
{

	public override FSM OnEnter()
	{
		owner.unitAnimation.SetParameter("attackSpeed", owner.AttackSpeed);
		return this;
	}


	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{

		if (owner.IsTargetAlive() == false)
		{
			owner.ChangeState(StateType.IDLE, true);
			return;
		}
		if (owner.TargetInRange() == false)
		{
			owner.ChangeState(StateType.MOVE, true);
			return;
		}

		if (owner.unitAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
		{
			if (owner.unitAnimation.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
			{


				owner.NormalAttack();
			}
		}
		else
		{
			owner.NormalAttack();
		}


	}

	public override void OnFixedUpdate(float fixedTime)
	{

	}
}
