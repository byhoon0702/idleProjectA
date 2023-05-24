//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public enum DropType
//{
//	VERTICAL,
//	OFFSET,
//	RANDOM,
//}


//[CreateAssetMenu(fileName = "Meteor Movement", menuName = "ScriptableObject/Action/Meteor", order = 2)]
//public class MeteorBehavior : SkillActionBehavior
//{
//	public float height;
//	public float xRange;
//	public float zRange;

//	public float xOffset;
//	public DropType dropType;

//	public override void SetPostionAndTarget(SkillObject projectile, Vector3 pos, Vector3 targetPos)
//	{
//		Vector3 spawnPos = new Vector3(pos.x + Random.Range(0, xRange), pos.y + height, pos.z + Random.Range(-zRange, zRange));

//		switch (dropType)
//		{
//			case DropType.RANDOM:
//				targetPos = new Vector3(targetPos.x + Random.Range(0, xRange), targetPos.y, targetPos.z + Random.Range(-zRange, zRange));
//				break;
//			case DropType.VERTICAL:
//				targetPos = new Vector3(spawnPos.x, targetPos.y, spawnPos.z);
//				break;
//			case DropType.OFFSET:

//				targetPos = new Vector3(spawnPos.x + xOffset, targetPos.y, spawnPos.z);

//				break;

//		}

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
