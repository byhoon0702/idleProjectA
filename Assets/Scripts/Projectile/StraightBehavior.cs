
//using UnityEngine;
//[CreateAssetMenu(fileName = "Straight Movement", menuName = "ScriptableObject/Action/Straight", order = 2)]
//public class StraightBehavior : SkillActionBehavior
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
//		//projectile.projectileView.right = projectile.velocityXZ.normalized;
//		Vector3 euler = projectile.projectileView.eulerAngles;
//		euler.x = 45;
//		projectile.projectileView.eulerAngles = euler;

//	}
//}
