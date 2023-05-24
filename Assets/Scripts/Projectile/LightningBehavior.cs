//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//[CreateAssetMenu(fileName = "Lightning Movement", menuName = "ScriptableObject/Action/Lightning", order = 2)]
//public class LightningBehavior : SkillActionBehavior
//{
//	public float height;
//	public float startRange;
//	public float endRange=0;

//	public override void SetPostionAndTarget(SkillObject projectile, Vector3 pos, Vector3 targetPos)
//	{
//		float randomx = Random.Range(0, startRange);
//		float randomy = Random.Range(0, endRange);
//		Vector3 spawnPos = new Vector3(targetPos.x + randomx, pos.y + height, pos.z + Random.Range(-1, 1));
//		targetPos = new Vector3(targetPos.x + randomy, targetPos.y, targetPos.z + Random.Range(-2, 2));
//		projectile.velocityXZ = (targetPos - spawnPos).normalized;

//		float distance = Vector3.Distance(spawnPos, targetPos);

//		float duration = distance / projectile.speed;
//		projectile.transform.position = spawnPos;
//		projectile.duration = duration;
//		projectile.projectileView.right = Vector3.right;
//	}

//	public override void OnUpdate(SkillObject projectile, float elapsedTime, float deltaTime)
//	{
//		projectile.transform.Translate(projectile.velocityXZ * projectile.speed * deltaTime, Space.World);
//		Vector3 euler = projectile.projectileView.eulerAngles;
//		//euler.x = 45;
//		projectile.projectileView.eulerAngles = euler;
//	}
//}
