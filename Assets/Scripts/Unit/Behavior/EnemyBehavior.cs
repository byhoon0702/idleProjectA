using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyBehavior : UnitBehavior
{
	public override void OnPreUpdate(Unit owner, float deltatime)
	{
		if (owner.currentState == StateType.DEATH)
		{
			return;
		}
		if (owner.currentState == StateType.DASH)
		{
			return;
		}
		if (owner.IsTargetAlive() == false)
		{
			owner.FindTarget(deltatime, true);
		}
	}

	public override void OnUpdate(Unit owner, float deltatime)
	{
		if (owner.currentState == StateType.DEATH)
		{
			return;
		}
		if (owner.currentState == StateType.NEUTRALIZE)
		{
			owner.ChangeState(StateType.NEUTRALIZE);

			return;
		}
		if (owner.currentState == StateType.DASH)
		{
			return;
		}
		//if (owner.IsTargetAlive())
		//{
		//	float distance = Mathf.Abs((owner.target.transform.position - owner.transform.position).magnitude);

		//	if (distance <= owner.SearchRange)
		//	{
		//		//if (owner.canSkillAutoAttack)
		//		//{
		//		//owner.ChangeState(StateType.SKILL, true);
		//		//	}
		//		//else
		//		//{
		//		owner.ChangeState(StateType.ATTACK);
		//		//}
		//	}
		//	else
		//	{
		//		owner.ChangeState(StateType.MOVE);
		//	}
		//}
		//else
		//{
		//	owner.ChangeState(StateType.IDLE);
		//}
	}

	public override void OnPostUpdate(Unit owner, float deltaTime)
	{
		if (owner.currentState == StateType.DEATH)
		{
			return;
		}
		owner.CheckDeathState();
	}
}
