using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Move State", menuName = "Unit State/Move", order = 1)]
public class UnitMoveState : UnitFSM
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
		if (SpawnManager.it != null && owner is EnemyUnit)
		{

		}
		else
		{
			owner.PlayAnimation(StateType.MOVE);
			owner.unitAnimation.PlayParticle();
		}
	}

	public override void OnExit()
	{
		if (SpawnManager.it != null && owner is EnemyUnit)
		{

		}
		else
		{
			owner.PlayAnimation(StateType.IDLE);
			owner.unitAnimation.StopParticle();
		}
	}

	public override void OnUpdate(float time)
	{
		owner.PlayAnimation(StateType.MOVE);
		//if (owner.IsTargetAlive())
		//{
		//	Vector3 direction = (owner.target.transform.position - owner.transform.position).normalized;
		//	direction.z = 0;
		//	direction.y = 0;
		//	float distance = Mathf.Abs(owner.target.transform.position.x - owner.transform.position.x);

		//	if (distance <= owner.SearchRange)
		//	{
		//		if (owner.canSkillAutoAttack)
		//		{
		//			owner.ChangeState(StateType.SKILL);
		//		}
		//		else
		//		{
		//			owner.ChangeState(StateType.ATTACK);
		//		}
		//	}
		//}
		//else
		//{
		//	owner.FindTarget(time, false);
		//}
	}
}
