using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehavior : ScriptableObject
{
	//public abstract float CalculateDuration(T projectile, Vector3 pos, Vector3 targetPos);
	public abstract void Init(Projectile projectile, Vector3 pos, Vector3 targetPos);

	public abstract void OnUpdate(Projectile projectile, float elapsedTime, float deltaTime);

}
