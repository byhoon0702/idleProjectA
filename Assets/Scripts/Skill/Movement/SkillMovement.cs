using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillMovement : ScriptableObject
{
	public float power;
	public float maxDistance;
	public bool toTarget;

	public Action onPreAction;
	public Action onAction;
	public Action onPostAction;

	public abstract IEnumerator OnMove(Unit caster, Vector3 targetPos, GameObject movementEffect = null);

}
