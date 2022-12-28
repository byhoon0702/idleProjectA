using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
	public override void Spawn(CharacterData data)
	{
		this.data = data.Clone();
		this.data.hp /= 5;
		this.data.attackDamage /= 5;

		var model = Instantiate(Resources.Load("B/skeleton")) as GameObject;
		model.transform.SetParent(transform);
		model.transform.localPosition = Vector3.zero;
		model.transform.localScale = Vector3.one * 0.003f;
		characterAnimation = model.GetComponent<CharacterAnimation>();

	}
	public override void Move(float delta)
	{
		transform.Translate(Vector3.left * data.moveSpeed * delta);
	}
	public override void FindTarget(float time)
	{
		searchInterval += time;
		//if (searchInterval > searchTime)
		{
			searchInterval = 0;

			PlayerCharacter[] enemies = GameObject.FindObjectsOfType<PlayerCharacter>();
			List<Info> infos = new List<Info>();
			if (enemies != null && enemies.Length > 0)
			{
				for (int i = 0; i < enemies.Length; i++)
				{
					float distan = Vector3.Distance(enemies[i].transform.position, transform.position);
					Info info = new Info();
					info.index = i;
					info.distance = distan;
					infos.Add(info);
				}
				infos.Sort((a, b) => { return a.distance.CompareTo(b.distance); });

				target = enemies[infos[0].index];
			}

		}
	}
	public override void Attack(float time)
	{
		attackInterval += time;
		if (attackInterval * data.attackRate >= data.attackTime)
		{
			attackInterval = 0;
			if (target != null)
			{
				target.Hit(data.attackDamage);
			}
		}
	}


	public override void Hit(IdleNumber damage)
	{
		data.hp -= damage;

		//	characterView.transform.position += Vector3.right * 0.1f;

		GameUIManager.it.ShowFloatingText(damage.ToString(), Color.red, transform.position);
		if (data.hp <= 0)
		{
			gameObject.SetActive(false);
			//Destroy(gameObject);
		}
	}
}
