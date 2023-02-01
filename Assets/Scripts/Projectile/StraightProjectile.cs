
using UnityEngine;

public class StraightProjectile : Projectile
{

	public override void Spawn(Transform _origin, UnitBase _attacker, UnitBase _targetCharacter, IdleNumber _attackPower)
	{
		base.Spawn(_origin, _attacker, _targetCharacter, _attackPower);

		behavior.Init(this, _origin.position, targetPos);

		projectileType = ProjectileType.STRAIGHT;
	}

}
