using UnityEngine;

public class BezierProjectile : Projectile
{

	public override void Spawn(Transform _origin, UnitBase _attacker, UnitBase _targetCharacter, IdleNumber _attackPower)
	{
		base.Spawn(_origin, _attacker, _targetCharacter, _attackPower);
		behavior.Init(this, _origin.position, targetPos);


		projectileType = ProjectileType.BEZIER;
	}

	//public override void Spawn(Vector3 origin, Vector3 targetPos, IdleNumber _attackPower)
	//{
	//	base.Spawn(origin, targetPos, _attackPower);

	//	projectileType = ProjectileType.BEZIER;
	//}

	public override void OnMove(Vector3 velocity)
	{
		//float x = FourPointBezierCurve(points[0].x, points[1].x, points[2].x, points[3].x, elapsedTime * speed);
		//float y = FourPointBezierCurve(points[0].y, points[1].y, points[2].y, points[3].y, elapsedTime * speed);
		//float z = FourPointBezierCurve(points[0].z, points[1].z, points[2].z, points[3].z, elapsedTime * speed);

		transform.SetPositionAndRotation(velocity, Quaternion.identity);
	}


}
