using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public enum AttackType
{
	Melee,
	Ranged,
	Magic,
}

[Serializable]
public class CharacterData
{
	public string name;
	public string resource;

	public IdleNumber hp;
	public float attackRange = 1;
	public float attackSpeed = 1;
	public float attackTime = 1;
	[Range(0.1f, 2f)]
	public float attackRate = 1;
	public IdleNumber attackDamage;
	public float moveSpeed = 1;
	public float pursuitDistance = 1;
	public float searchTime = 1f;

	public CharacterData Clone()
	{
		CharacterData data = new CharacterData();
		data.resource = resource;
		data.hp = hp;
		data.attackRange = attackRange;
		data.attackSpeed = attackSpeed;
		data.attackTime = attackTime;
		data.moveSpeed = moveSpeed;
		data.attackRate = attackRate;
		data.attackDamage = attackDamage;
		data.pursuitDistance = pursuitDistance;
		data.searchTime = searchTime;
		return data;
	}
}

public interface CharacterFSM : FiniteStateMachine
{
}

public class IdleState : CharacterFSM
{
	private Character owner;
	public void Init(Character owner)
	{
		this.owner = owner;
	}
	public void OnEnter()
	{
	}

	public void OnExit()
	{

	}

	public void OnUpdate(float time)
	{

	}

	private void FindTarget()
	{

	}
}

public class AttackState : CharacterFSM
{
	private Character owner;
	public void Init(Character owner)
	{
		this.owner = owner;
	}
	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void OnUpdate(float time)
	{
		owner.Attack(time);

		if (owner.IsTargetAlive() == false)
		{
			owner.ChangeState(StateType.MOVE);
		}
	}
}


public class MoveState : CharacterFSM
{
	private Character owner;
	public void Init(Character owner)
	{
		this.owner = owner;
	}
	public void OnEnter()
	{
		owner.characterAnimation.PlayAnimation("walk");
	}

	public void OnExit()
	{
		owner.characterAnimation.PlayAnimation("idle");
	}

	public void OnUpdate(float time)
	{
		if (owner.IsTargetAlive() == false)
		{
			owner.FindTarget(time);
			owner.Move(time);
		}
		else
		{
			var direction = (owner.target.transform.position - owner.transform.position).normalized;
			var distance = Vector3.Distance(owner.target.transform.position, owner.transform.position);
			if (distance < owner.data.pursuitDistance)
			{
				if (distance > owner.data.attackRange)
				{
					owner.transform.Translate(direction * owner.data.moveSpeed * time);
				}
				else
				{
					owner.ChangeState(StateType.ATTACK);
				}
			}
			else
			{
				owner.Move(time);
			}
		}
	}
}

public enum StateType
{
	IDLE,
	MOVE,
	ATTACK,
	HIT,
	SKILL

}

public class Character : MonoBehaviour
{
	public CharacterData data;
	public Character target;

	public CharacterAnimation characterAnimation;

	public FiniteStateMachine currentfsm;

	public IdleState idleState;
	public MoveState moveState;
	public AttackState attackState;


	protected float attackInterval = 0;
	protected float searchInterval = 0;
	protected class Info
	{
		public int index;
		public float distance;
	}

	// Start is called before the first frame update
	void Start()
	{
		idleState = new IdleState();
		moveState = new MoveState();
		attackState = new AttackState();


		idleState.Init(this);
		moveState.Init(this);
		attackState.Init(this);


		currentfsm = moveState;
	}

	public void ChangeState(StateType stateType)
	{
		currentfsm?.OnExit();
		switch (stateType)
		{
			case StateType.IDLE:
				currentfsm = idleState;
				break;
			case StateType.MOVE:
				currentfsm = moveState;
				break;
			case StateType.ATTACK:
				currentfsm = attackState;
				break;

		}
		currentfsm?.OnEnter();
	}

	public virtual void Spawn(CharacterData data)
	{

	}
	public bool IsTargetAlive()
	{
		if (target == null)
		{
			return false;
		}

		return target.data.hp > 0;
	}
	// Update is called once per frame
	void Update()
	{

		float delta = Time.deltaTime;
		currentfsm?.OnUpdate(delta);
		//if (target == null)
		//{
		//	FindTarget(delta);
		//	//Move(delta);
		//}
		//else
		//{
		//	var direction = (target.transform.position - transform.position).normalized;
		//	var distance = Vector3.Distance(target.transform.position, transform.position);
		//	if (distance < data.pursuitDistance)
		//	{
		//		if (distance > data.attackRange)
		//		{
		//			transform.Translate(direction * data.moveSpeed * delta);
		//		}
		//		else
		//		{
		//			Attack(delta);
		//		}
		//	}
		//	else
		//	{
		//		//Move(delta);
		//	}

		//}
	}
	public virtual void Move(float delta)
	{

	}
	public virtual void Hit(IdleNumber damage)
	{

	}

	public virtual void Attack(float time)
	{

	}

	public virtual void FindTarget(float time)
	{

	}

	private void OnDrawGizmosSelected()
	{

	}


}
