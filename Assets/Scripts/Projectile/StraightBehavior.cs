
using UnityEngine;
[CreateAssetMenu(fileName = "Straight Movement", menuName = "Behaviors/Projectile/Straight", order = 2)]
public class StraightBehavior : ProjectileBehavior
{
	public override void Init(Projectile projectile, Vector3 pos, Vector3 targetPos)
	{
		projectile.velocityXZ = (targetPos - pos).normalized;

		float distance = Vector3.Distance(pos, targetPos);

		float duration = distance / projectile.speed;

		projectile.duration = duration;
	}

	public override void OnUpdate(Projectile projectile, float elapsedTime, float deltaTime)
	{
		projectile.transform.Translate(projectile.velocityXZ * projectile.speed * deltaTime);
		projectile.projectileView.right = projectile.velocityXZ.normalized;
		Vector3 euler = projectile.projectileView.eulerAngles;
		euler.x = 45;
		projectile.projectileView.eulerAngles = euler;
		elapsedTime += Time.deltaTime;
	}
}
