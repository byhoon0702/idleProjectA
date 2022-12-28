using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
	Melee,
	Ranged,
	Magic,
}

[SerializeField]
public class CharacterData
{
	public float hp = 10;
	public float attackRange = 1;
	public float attackSpeed = 1;
	public float attackTime = 1;
	[Range(0.1f, 2f)]
	public float attackRate = 1;
	public float attackDamage = 1;
	public float moveSpeed = 1;
	public float pursuitDistance = 1;
	public float searchTime = 1f;
}

public interface CharacterFSM
{
	void OnEnter();
	void OnUpdate();
	void OnExit();

}

public class IdleState : CharacterFSM
{
	public CharacterData own_data;
	public void OnEnter()
	{
	}

	public void OnExit()
	{

	}

	public void OnUpdate()
	{

	}

	private void FindTarget()
	{

	}
}

public class AttackState : CharacterFSM
{
	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void OnUpdate()
	{
	}
}

public class DamageState : CharacterFSM
{
	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void OnUpdate()
	{
	}
}

public class MoveState : CharacterFSM
{
	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void OnUpdate()
	{
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
	public Sprite[] sprites;
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

	public Character target;


	protected float attackInterval = 0;
	protected float searchInterval = 0;

	// Start is called before the first frame update
	void Start()
	{

	}

	public virtual void Spawn()
	{

	}
	// Update is called once per frame
	void Update()
	{
		float delta = Time.deltaTime;
		if (target == null)
		{
			FindTarget(delta);
			Move(delta);
		}
		else
		{
			var direction = (target.transform.position - transform.position).normalized;
			var distance = Vector3.Distance(target.transform.position, transform.position);
			if (distance < pursuitDistance)
			{
				if (distance > attackRange)
				{
					transform.Translate(direction * moveSpeed * delta);
				}
				else
				{
					Attack(delta);
				}
			}
			else
			{
				Move(delta);
			}

		}
	}
	protected virtual void Move(float delta)
	{

	}
	public virtual void Hit(IdleNumber damage)
	{

	}

	protected virtual void Attack(float time)
	{

	}

	protected virtual void FindTarget(float time)
	{

	}


}
