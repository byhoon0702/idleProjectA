using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{

	public SpriteRenderer characterView;

	public override void Spawn()
	{

	}

	protected override void Move(float delta)
	{
		transform.Translate(Vector3.right * moveSpeed * delta);
	}
	protected override void FindTarget(float time)
	{
		searchInterval += time;
		//if (searchInterval > searchTime)
		{
			searchInterval = 0;

			EnemyCharacter[] enemies = GameObject.FindObjectsOfType<EnemyCharacter>();

			if (enemies != null && enemies.Length > 0)
			{
				target = enemies[Random.Range(0, enemies.Length)];
			}
		}
	}

	protected override void Attack(float time)
	{
		attackInterval += time;
		if (attackInterval * attackRate >= attackTime)
		{
			attackInterval = 0;
			if (target != null)
			{
				target.Hit(attackDamage);
			}
		}
	}

	public override void Hit(IdleNumber damage)
	{
		hp -= damage;
		UIManager.it.ShowFloatingText(damage.ToString(), Color.red, transform.position);
		if (hp <= 0)
		{
			Destroy(gameObject);
		}
	}

}
