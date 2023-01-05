using UnityEngine;

public class BezierProjectile : Projectile
{

	public Vector3[] points = new Vector3[4];
	public float speed = 1;
	public Vector3 size;
	public override void Spawn(Transform _origin, Character _attacker, Character _targetCharacter, IdleNumber _attackPower)
	{
		base.Spawn(_origin, _attacker, _targetCharacter, _attackPower);
		duration = 1 / speed;
		points[0] = _origin.position;
		points[1] = SetPoint(_origin.position);
		points[2] = SetPoint(targetPos);
		points[3] = targetPos;
	}

	public override void Spawn(Vector3 origin, Vector3 targetPos, IdleNumber _attackPower)
	{
		base.Spawn(origin, targetPos, _attackPower);
	}

	private Vector3 SetPoint(Vector3 origin)
	{
		Vector3 point = origin;

		point.x += Mathf.Cos(UnityEngine.Random.Range(0, 360f) * Mathf.Deg2Rad) * size.x;
		point.y += Mathf.Sin(UnityEngine.Random.Range(0, 360f) * Mathf.Deg2Rad) * size.y;

		return point;
	}

	float FourPointBezierCurve(float _p1, float _p2, float _p3, float p4, float t)
	{
		float a = Mathf.Pow((1 - t), 3) * _p1;
		float b = Mathf.Pow((1 - t), 2) * 3 * t * _p2;
		float c = Mathf.Pow(t, 2) * 3 * (1 - t) * _p3;
		float d = Mathf.Pow(t, 3) * p4;

		return a + b + c + d;
	}

	void Update()
	{
		if (dontmove)
		{
			return;
		}

		if (elapsetimd < duration)
		{

			float x = FourPointBezierCurve(points[0].x, points[1].x, points[2].x, points[3].x, elapsetimd * speed);
			float y = FourPointBezierCurve(points[0].y, points[1].y, points[2].y, points[3].y, elapsetimd * speed);
			float z = FourPointBezierCurve(points[0].z, points[1].z, points[2].z, points[3].z, elapsetimd * speed);

			transform.SetPositionAndRotation(new Vector3(x, y, z), Quaternion.identity);

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
