﻿
using System.Drawing;
using log4net.Util;
using UnityEngine;

[CreateAssetMenu(fileName = "Bezier Movement", menuName = "Behaviors/Projectile/Bezier", order = 2)]
public class BezierBehavior : ProjectileBehavior
{

	public override void Init(Projectile projectile, Vector3 pos, Vector3 targetPos)
	{
		Vector3[] points = new Vector3[4];
		points[0] = pos;
		points[1] = SetPoint(pos, projectile.size);
		points[2] = SetPoint(targetPos, projectile.size);
		points[3] = targetPos;

		projectile.duration = 1 / projectile.speed;
		projectile.points = points;
	}
	public override void OnUpdate(Projectile projectile, float elapsedTime, float deltaTime)
	{
		float speed = projectile.speed;

		Vector3[] points = projectile.points;
		float x = ProjectileHelper.FourPointBezierCurve(points[0].x, points[1].x, points[2].x, points[3].x, elapsedTime * speed);
		float y = ProjectileHelper.FourPointBezierCurve(points[0].y, points[1].y, points[2].y, points[3].y, elapsedTime * speed);
		float z = ProjectileHelper.FourPointBezierCurve(points[0].z, points[1].z, points[2].z, points[3].z, elapsedTime * speed);

		//projectile.OnMove();

		projectile.transform.SetPositionAndRotation(new Vector3(x, y, z), Quaternion.identity);
		elapsedTime += deltaTime;
	}
	private Vector3 SetPoint(Vector3 origin, Vector2 size)
	{
		Vector3 point = origin;

		point.x += Mathf.Cos(UnityEngine.Random.Range(0, 360f) * Mathf.Deg2Rad) * size.x;
		point.y += Mathf.Sin(UnityEngine.Random.Range(0, 360f) * Mathf.Deg2Rad) * size.y;

		return point;
	}

}
