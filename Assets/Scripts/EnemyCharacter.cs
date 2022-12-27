using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
	protected override void Move(float delta)
	{
		transform.Translate(Vector3.left * moveSpeed * delta);
	}
	protected override void FindTarget(float time)
	{
		searchInterval += time;
		if (searchInterval > searchTime)
		{
			searchInterval = 0;

			PlayerCharacter[] enemies = GameObject.FindObjectsOfType<PlayerCharacter>();
			if (enemies != null && enemies.Length > 0)
			{
				target = enemies[Random.Range(0, enemies.Length)];
				Debug.Log(target.name);
			}
		}
	}
	public override void Hit(float damage)
	{
		hp -= damage;
		if (hp <= 0)
		{
			Destroy(gameObject);
		}
	}
}
