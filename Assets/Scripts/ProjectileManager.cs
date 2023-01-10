using UnityEngine;
public class ProjectileManager : MonoBehaviour
{
	private static ProjectileManager instance;
	public static ProjectileManager it => instance;

	public GuidedProjectile guidedProjectileResource;
	public StraightProjectile straightProjectileResource;
	public ParabolaProjectile parabolaProjectileResource;
	public BezierProjectile bezierProjectileResource;
	private void Awake()
	{
		instance = this;
	}


	public Projectile Create(Character character)
	{
		Projectile projectile = null;
		switch (character.info.jobData.attackType)
		{
			case AttackType.RANGED:
				projectile = Instantiate(parabolaProjectileResource) as Projectile;
				break;
			case AttackType.MAGIC:
				projectile = Instantiate(bezierProjectileResource) as Projectile;
				break;
		}

		if (projectile == null)
		{
			return null;
		}


		projectile.Spawn(character.characterAnimation.CenterPivot, character, character.target, character.info.AttackPower());

		return projectile;
	}
	// Start is called before the first frame update

}
