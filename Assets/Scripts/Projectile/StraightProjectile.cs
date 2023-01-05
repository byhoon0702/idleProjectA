using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StraightProjectile : Projectile
{
	public float speed;

	private Vector3 direction;
	public override void Spawn(Transform _origin, Character _attacker, Character _targetCharacter, IdleNumber _attackPower)
	{
		base.Spawn(_origin, _attacker, _targetCharacter, _attackPower);


		direction = (targetPos - _origin.position).normalized;

		distance = Vector3.Distance(_origin.position, targetPos);

		duration = distance / speed;
	}
	public override void Spawn(Vector3 origin, Vector3 targetPos, IdleNumber _attackPower)
	{
		base.Spawn(origin, targetPos, _attackPower);

		direction = (targetPos - origin).normalized;

		distance = Vector3.Distance(origin, targetPos);

		duration = distance / speed;
	}

	void Update()
	{
		if (dontmove)
		{
			return;
		}

		if (elapsetimd < duration)
		{
			projectileView.transform.Translate(direction * speed * Time.deltaTime);
			elapsetimd += Time.deltaTime;

		}
		else
		{
			ReachedDestination();
			dontmove = true;
			elapsetimd = 0;
		}
	}

}
