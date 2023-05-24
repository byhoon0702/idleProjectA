
//using System.Drawing;
//using UnityEngine;

//[CreateAssetMenu(fileName = "HeadOverBezier Movement", menuName = "Behaviors/Action/HeadOverBezier", order = 2)]
//public class HeadOverBezierBehavior : SkillActionBehavior
//{
//	public float yOver;
//	public float startRange;
//	public Vector2 xRandom;
//	public Vector2 yRandom;

//	public Vector2 size;

//	public override void SetPostionAndTarget(SkillObject projectile, Vector3 pos, Vector3 targetPos)
//	{
//		pos.y += yOver + Random.Range(0, startRange);
//		Vector3[] points = new Vector3[4];
//		points[0] = pos;
//		points[1] = SetPoint(pos);
//		points[2] = SetPoint(pos);
//		points[3] = targetPos;

//		projectile.duration = 1 / projectile.speed;
//		projectile.points = points;

//		projectile.transform.position = pos;

//	}

//	public override void OnUpdate(SkillObject projectile, float elapsedTime, float deltaTime)
//	{
//		float speed = projectile.speed;

//		Vector3[] points = projectile.points;
//		float x = ProjectileHelper.ThreePointBezierCurve(points[0].x, points[1].x, points[3].x, elapsedTime * speed);
//		float y = ProjectileHelper.ThreePointBezierCurve(points[0].y, points[1].y, points[3].y, elapsedTime * speed);
//		float z = ProjectileHelper.ThreePointBezierCurve(points[0].z, points[1].z, points[3].z, elapsedTime * speed);

//		Vector3 moveto = new Vector3(x, y, z);
//		projectile.projectileView.right = (moveto - projectile.transform.position).normalized;

//		projectile.transform.SetPositionAndRotation(moveto, Quaternion.identity);
//	}

//	private Vector3 SetPoint(Vector3 origin)
//	{
//		Vector3 point = origin;

//		point.x += Mathf.Cos(UnityEngine.Random.Range(xRandom.x, xRandom.y) * Mathf.Deg2Rad) * size.x;
//		point.y += Mathf.Sin(UnityEngine.Random.Range(yRandom.x, yRandom.y) * Mathf.Deg2Rad) * size.y;

//		return point;
//	}

//}
