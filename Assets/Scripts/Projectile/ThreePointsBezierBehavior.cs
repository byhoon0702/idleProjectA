
//using System.Drawing;
//using UnityEngine;

//[CreateAssetMenu(fileName = "Three Point Bezier Movement", menuName = "ScriptableObject/Action/Three Point Bezier", order = 2)]
//public class ThreePointsBezierBehavior : SkillActionBehavior
//{
//	public Vector2 xRandom;
//	public Vector2 yRandom;
//	public float height;
//	public bool fixedMidPos;


//	public override void SetPostionAndTarget(SkillObject projectile, Vector3 pos, Vector3 targetPos)
//	{
//		Vector3[] points = new Vector3[3];

//		points[0] = pos;
//		if (fixedMidPos)
//		{
//			Vector3 middlePos = (targetPos + pos) / 2;
//			middlePos.y = height;
//			points[1] = middlePos;
//		}
//		else
//		{
//			points[1] = SetPoint(pos);
//		}

//		points[2] = targetPos;

//		projectile.duration = 1 / projectile.speed;
//		projectile.points = points;


//		float x = ProjectileHelper.ThreePointBezierCurve(points[0].x, points[1].x, points[2].x, 0.1f);
//		float y = ProjectileHelper.ThreePointBezierCurve(points[0].y, points[1].y, points[2].y, 0.1f);
//		float z = ProjectileHelper.ThreePointBezierCurve(points[0].z, points[1].z, points[2].z, 0.1f);

//		Vector3 moveto = new Vector3(x, y, z);
//		projectile.projectileView.right = (moveto - projectile.transform.position).normalized;

//	}

//	public override void OnUpdate(SkillObject projectile, float elapsedTime, float deltaTime)
//	{
//		float speed = projectile.speed;

//		Vector3[] points = projectile.points;
//		float x = ProjectileHelper.ThreePointBezierCurve(points[0].x, points[1].x, points[2].x, elapsedTime * speed);
//		float y = ProjectileHelper.ThreePointBezierCurve(points[0].y, points[1].y, points[2].y, elapsedTime * speed);
//		float z = ProjectileHelper.ThreePointBezierCurve(points[0].z, points[1].z, points[2].z, elapsedTime * speed);

//		Vector3 moveto = new Vector3(x, y, z);
//		projectile.projectileView.right = (moveto - projectile.transform.position).normalized;

//		projectile.transform.SetPositionAndRotation(moveto, Quaternion.identity);
//	}

//	private Vector3 SetPoint(Vector3 origin)
//	{
//		Vector3 point = origin;

//		point.x += Mathf.Cos(UnityEngine.Random.Range(xRandom.x, xRandom.y) * Mathf.Deg2Rad);
//		point.y += Mathf.Sin(UnityEngine.Random.Range(yRandom.x, yRandom.y) * Mathf.Deg2Rad);

//		return point;
//	}

//}
