using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GuidedProjectile : Projectile
{
	public float curveMultiflier;
	//public float amplitude;
	public AnimationCurve yAxisCurve;
	private Vector3 direction;


	public override void Spawn(Transform _origin, UnitBase _attacker, UnitBase _targetCharacter, IdleNumber _attackPower)
	{
		base.Spawn(_origin, _attacker, _targetCharacter, _attackPower);

		direction = (targetPos - _origin.position).normalized;

		distance = Vector3.Distance(_origin.position, _targetCharacter.transform.position);

		duration = distance / speed;

		projectileType = ProjectileType.GUIDED;
	}

	//public override void Spawn(Vector3 origin, Vector3 targetPos, IdleNumber _attackPower)
	//{
	//	base.Spawn(origin, targetPos, _attackPower);

	//	direction = (targetPos - origin).normalized;

	//	distance = Vector3.Distance(origin, targetPos);

	//	duration = distance / speed;

	//	projectileType = ProjectileType.GUIDED;
	//}

	void Update()
	{
		if (dontmove)
		{
			return;
		}

		if (elapsedTime < duration)
		{
			vy = yAxisCurve.Evaluate(elapsedTime / duration) * curveMultiflier;

			Vector3 transition = direction * speed * Time.deltaTime;

			transition.y += vy + transform.position.y;
			transition.x += projectileView.position.x;

			projectileView.SetPositionAndRotation(transition, Quaternion.identity);
			elapsedTime += Time.deltaTime;

		}
		else
		{
			ReachedDestination();
			dontmove = true;
			elapsedTime = 0;
		}
	}
}
