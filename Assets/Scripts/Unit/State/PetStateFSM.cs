using UnityEngine;


public class PetMoveState : UnitFSM
{
	private new Pet owner;

	public void Init(Pet _owner)
	{
		owner = _owner;
	}

	public override FSM OnEnter()
	{
		owner.unitAnimation.PlayParticle();
		return this;
	}




	public override void OnExit()
	{
		//owner.PlayAnimation(StateType.IDLE);
		owner.unitAnimation.StopParticle();
	}

	public override void OnUpdate(float time)
	{

		//owner.Move(time);
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
	public override void OnFixedUpdate(float fixedTime)
	{

	}
}

public class PetAttackState : UnitFSM
{
	private new Pet owner;
	//private SkillModule skillModule => owner.skillModule;



	public void Init(Pet _owner)
	{
		owner = _owner;
	}
	public override FSM OnEnter()
	{
		return this;
	}


	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		bool isAttacking = owner.unitAnimation.IsAttacking();
		if (isAttacking)
		{
			return;
		}

		//}
	}
	public override void OnFixedUpdate(float fixedTime)
	{

	}
}
public class PetIdleState : UnitFSM
{
	private new Pet owner;


	public void Init(Pet _owner)
	{
		owner = _owner;
	}
	public override FSM OnEnter()
	{
		return this;

	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{

	}
	public override void OnFixedUpdate(float fixedTime)
	{

	}
}
