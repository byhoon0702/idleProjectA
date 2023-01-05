using UnityEngine;

public class ParabolaProjectile : Projectile
{
	public float angle;
	private Vector3 velocityXZ;
	private Vector3 calculateVector;
	public override void Spawn(Transform _origin, Character _attacker, Character _targetCharacter, IdleNumber _damage)
	{
		base.Spawn(_origin, _attacker, _targetCharacter, _damage);

		distance = Vector3.Distance(_origin.position, targetPos);
		float velocity = distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);

		Vector3 normalizeDirection = (targetPos - _origin.position).normalized;
		normalizeDirection.y = 0;

		velocityXZ = Mathf.Sqrt(velocity) * Mathf.Cos(angle * Mathf.Deg2Rad) * normalizeDirection;
		vy = Mathf.Sqrt(velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);

		duration = distance / velocityXZ.magnitude;


	}

	public override void Spawn(Vector3 _origin, Vector3 _targetPos, IdleNumber _damage)
	{
		base.Spawn(_origin, _targetPos, _damage);

		distance = Vector3.Distance(_origin, targetPos);
		float velocity = distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);
		Vector3 normalizeDirection = (targetPos - _origin).normalized;
		normalizeDirection.y = 0;
		velocityXZ = Mathf.Sqrt(velocity) * Mathf.Cos(angle * Mathf.Deg2Rad) * normalizeDirection;
		vy = Mathf.Sqrt(velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);

		duration = distance / velocityXZ.magnitude;
	}

	void Update()
	{
		if (dontmove)
		{
			return;
		}

		if (elapsetimd < duration)
		{
			calculateVector.x = velocityXZ.x;
			calculateVector.y = (vy - (gravity * elapsetimd));
			calculateVector.z = velocityXZ.z;
			projectileView.transform.Translate(calculateVector * Time.deltaTime, Space.Self);
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
