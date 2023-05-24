using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tornado Movement", menuName = "Skill/Movement/Tornado")]
public class TornadoMovement : SkillMovement
{
	public override IEnumerator OnMove(Unit caster, Vector3 targetPos, GameObject movementEffect = null)
	{
		yield break;
	}
}
