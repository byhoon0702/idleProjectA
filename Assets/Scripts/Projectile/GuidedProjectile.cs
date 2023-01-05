using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GuidedProjectile : Projectile
{
	public float speed;
	public float curveMultiflier;
	//public float amplitude;
	public AnimationCurve yAxisCurve;
	private Vector3 direction;


	public override void Spawn(Transform _origin, Character _attacker, Character _targetCharacter, IdleNumber _attackPower)
	{
		base.Spawn(_origin, _attacker, _targetCharacter, _attackPower);

		direction = (targetPos - _origin.position).normalized;

		distance = Vector3.Distance(_origin.position, _targetCharacter.transform.position);

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
			vy = yAxisCurve.Evaluate(elapsetimd / duration) * curveMultiflier;

			Vector3 transition = direction * speed * Time.deltaTime;

			transition.y += vy + transform.position.y;
			transition.x += projectileView.transform.position.x;

			projectileView.transform.SetPositionAndRotation(transition, Quaternion.identity);
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
