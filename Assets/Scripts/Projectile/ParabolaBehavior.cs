
//using Unity.VisualScripting;
//using UnityEngine;

//[CreateAssetMenu(fileName = "Parabola Movement", menuName = "ScriptableObject/Action/Parabola", order = 2)]
//public class ParabolaBehavior : SkillActionBehavior
//{
//	private const float gravity = 9.8f;
//	public float angle;
//	public override void SetPostionAndTarget(SkillObject projectile, Vector3 pos, Vector3 targetPos)
//	{
//		angle = 30;
//		float distance = Vector3.Distance(pos, targetPos);
//		float velocity = distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);

//		Vector3 normalizeDirection = (targetPos - pos).normalized;
//		normalizeDirection.y = 0;

//		projectile.velocityXZ = Mathf.Sqrt(velocity) * Mathf.Cos(angle * Mathf.Deg2Rad) * normalizeDirection;
//		projectile.vy = Mathf.Sqrt(velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);

//		float duration = distance / (projectile.velocityXZ.magnitude);

//		projectile.duration = duration;
//		projectile.projectileView.right = new Vector3(projectile.velocityXZ.x, projectile.vy, projectile.velocityXZ.z).normalized;
//		Vector3 euler = projectile.projectileView.eulerAngles;
//		euler.x = 45;
//		projectile.projectileView.eulerAngles = euler;
//	}
//	public override void OnUpdate(SkillObject projectile, float elapsedTime, float deltaTime)
//	{
//		Vector3 calculateVector = Vector3.zero;
//		calculateVector.x = projectile.velocityXZ.x;
//		calculateVector.y = (projectile.vy - (gravity * elapsedTime));
//		calculateVector.z = projectile.velocityXZ.z;

//		projectile.transform.Translate(calculateVector * deltaTime, Space.Self);
//		projectile.projectileView.right = calculateVector.normalized;
//		Vector3 euler = projectile.projectileView.eulerAngles;
//		euler.x = 45;
//		projectile.projectileView.eulerAngles = euler;
//		//projectile.elapsedTime += Time.deltaTime;
//	}
//}
