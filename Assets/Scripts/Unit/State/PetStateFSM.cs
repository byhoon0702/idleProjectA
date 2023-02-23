using UnityEngine;


public class PetMoveState : UnitFSM
{
	private Pet owner;

	public void Init(Pet _owner)
	{
		owner = _owner;
	}
	public void OnEnter()
	{
		owner.PlayAnimation(StateType.MOVE);
		owner.unitAnimation.PlayParticle();
	}

	public void OnExit()
	{
		owner.PlayAnimation(StateType.IDLE);
		owner.unitAnimation.StopParticle();
	}

	public void OnUpdate(float time)
	{

		owner.Move(time);
		//if (owner.IsTargetAlive() == false)
		//{
		//	owner.targetingBehavior.OnTarget(owner, owner.targeting);
		//
		//	owner.Move(time);
		//}
		//else
		//{
		//	Vector3 direction = (owner.target.transform.position - owner.transform.position).normalized;
		//	direction.z = 0;
		//	direction.y = 0;
		//
		//	float distance = Mathf.Abs(owner.target.transform.position.x - owner.transform.position.x);
		//	if (distance <= 10)
		//	{
		//		owner.ChangeState(StateType.ATTACK);
		//	}
		//}
	}
}

public class PetAttackState : UnitFSM
{
	private Pet owner;
	private SkillModule skillModule => owner.skillModule;



	public void Init(Pet _owner)
	{
		owner = _owner;
	}

	public void OnEnter()
	{

	}

	public void OnExit()
	{

	}

	public void OnUpdate(float time)
	{
		bool isAttacking = owner.unitAnimation.IsAttacking();
		if (isAttacking)
		{
			return;
		}

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

		//if (skillModule.skillAttack != null && skillModule.skillAttack.Usable())
		//{
		//	// 스킬을 사용할 수 있으면 무조건 스킬우선사용
		//	owner.AttackStart(skillModule.skillAttack);
		//}
		//else if (skillModule.defaultAttack != null && skillModule.defaultAttack.Usable())
		//{
		//	// 기본공격
		owner.AttackStart();
		//}
	}
}
public class PetIdleState : UnitFSM
{
	private Pet owner;


	public void Init(Pet _owner)
	{
		owner = _owner;
	}
	public void OnEnter()
	{
		owner.PlayAnimation(StateType.IDLE);
	}

	public void OnExit()
	{

	}

	public void OnUpdate(float time)
	{

	}
}
