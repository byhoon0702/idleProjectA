using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Character : MonoBehaviour
{

	public float hp = 10;

	public float attackRange = 1;
	public float attackSpeed = 1;
	public float attackTime = 1;
	[Range(0.1f, 2f)]
	public float attackRate = 1;
	public float attackDamage = 1;
	public float moveSpeed = 1;

	public Character target;
	public float pursuitDistance = 1;
	public float searchTime = 1f;

	protected float attackInterval = 0;
	protected float searchInterval = 0;

	// Start is called before the first frame update
	void Start()
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
	public virtual void Hit(float damage)
	{

	}

	protected virtual void Attack(float time)
	{

	}

	protected virtual void FindTarget(float time)
	{

	}


}
