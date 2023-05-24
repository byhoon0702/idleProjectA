//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LaserBehavior : SkillActionBehavior
//{
//	public override void SetPostionAndTarget(SkillObject projectile, Vector3 pos, Vector3 targetPos)
//	{
//		projectile.velocityXZ = (targetPos - pos).normalized;

//		float distance = Vector3.Distance(pos, targetPos);

//		float duration = distance / projectile.speed;

//		projectile.duration = duration;
//		projectile.projectileView.right = Vector3.right;
//	}

//	public override void OnUpdate(SkillObject projectile, float elapsedTime, float deltaTime)
//	{
//		projectile.transform.Translate(projectile.velocityXZ * projectile.speed * deltaTime);

//		Vector3 euler = projectile.projectileView.eulerAngles;
//		euler.x = 45;
//		projectile.projectileView.eulerAngles = euler;
//	}
//}
