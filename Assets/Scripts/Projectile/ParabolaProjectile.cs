using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileInfo
{
}


public class ParabolaProjectile : Projectile
{


	private Vector3 calculateVector;

	public override void Spawn(Transform _origin, UnitBase _attacker, UnitBase _targetCharacter, IdleNumber _attackPower)
	{
		base.Spawn(_origin, _attacker, _targetCharacter, _attackPower);

		//distance = Vector3.Distance(_origin.position, targetPos);
		//float velocity = distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);

		//Vector3 normalizeDirection = (targetPos - _origin.position).normalized;
		//normalizeDirection.y = 0;

		//velocityXZ = Mathf.Sqrt(velocity) * Mathf.Cos(angle * Mathf.Deg2Rad) * normalizeDirection;
		//vy = Mathf.Sqrt(velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);

		//duration = distance / (velocityXZ.magnitude);
		//duration = behavior.CalculateDuration(this, _origin.position, targetPos);

		projectileType = ProjectileType.PARABOLA;
	}

	//public override void Spawn(Vector3 _origin, Vector3 _targetPos, IdleNumber _attackPower)
	//{
	//	//base.Spawn(_origin, _targetPos, _attackPower);

	//	distance = Vector3.Distance(_origin, targetPos);
	//	float velocity = distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);
	//	Vector3 normalizeDirection = (targetPos - _origin).normalized;
	//	normalizeDirection.y = 0;
	//	velocityXZ = Mathf.Sqrt(velocity) * Mathf.Cos(angle * Mathf.Deg2Rad) * normalizeDirection;
	//	vy = Mathf.Sqrt(velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);

	//	duration = distance / velocityXZ.magnitude;

	//	projectileType = ProjectileType.PARABOLA;
	//}

	public override void OnMove(Vector3 velocity)
	{

		//calculateVector.x = velocityXZ.x * speed;
		//calculateVector.y = (vy * speed - (gravity * speed * elapsedTime));
		//calculateVector.z = velocityXZ.z * speed;

		transform.Translate(velocity * Time.deltaTime, Space.Self);

		//Vector3 angleVector = new Vector3(0, 0, angle - calculateVector.y);
		//projectileView.eulerAngles = angleVector;
		projectileView.right = velocity.normalized;
		Vector3 euler = projectileView.eulerAngles;
		euler.x = 45;
		projectileView.eulerAngles = euler;
	}

	//protected void Update()
	//{
	//	if (dontmove)
	//	{
	//		return;
	//	}


	//	if (CheckHitDistance())
	//	{
	//		ReachedDestination();
	//		return;
	//	}
	//	if (elapsedTime < duration)
	//	{
	//		behavior.OnMove(this, elapsedTime);
	//		elapsedTime += Time.deltaTime * speed;
	//	}

	//	else
	//	{
	//		ReachedDestination();
	//	}
	//}
}
