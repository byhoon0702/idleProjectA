
using UnityEngine;

[CreateAssetMenu(fileName = "Parabola Movement", menuName = "Behaviors/Projectile/Parabola", order = 2)]
public class ParabolaBehavior : ProjectileBehavior
{
	private const float gravity = 9.8f;

	public override void Init(Projectile projectile, Vector3 pos, Vector3 targetPos)
	{
		float distance = Vector3.Distance(pos, targetPos);
		float velocity = distance / (Mathf.Sin(2 * projectile.angle * Mathf.Deg2Rad) / gravity);

		Vector3 normalizeDirection = (targetPos - pos).normalized;
		normalizeDirection.y = 0;

		projectile.velocityXZ = Mathf.Sqrt(velocity) * Mathf.Cos(projectile.angle * Mathf.Deg2Rad) * normalizeDirection;
		projectile.vy = Mathf.Sqrt(velocity) * Mathf.Sin(projectile.angle * Mathf.Deg2Rad);


		float duration = distance / (projectile.velocityXZ.normalized.magnitude * projectile.speed);

		projectile.duration = duration;
	}
	public override void OnUpdate(Projectile projectile, float elapsedTime, float deltaTime)
	{
		Vector3 calculateVector = Vector3.zero;
		calculateVector.x = projectile.velocityXZ.x;
		calculateVector.y = (projectile.vy - (gravity * elapsedTime));
		calculateVector.z = projectile.velocityXZ.z;

		projectile.transform.Translate(calculateVector.normalized * projectile.speed * deltaTime, Space.Self);
		projectile.projectileView.right = calculateVector.normalized;
		Vector3 euler = projectile.projectileView.eulerAngles;
		euler.x = 45;
		projectile.projectileView.eulerAngles = euler;
		projectile.elapsedTime += Time.deltaTime;
	}
}
