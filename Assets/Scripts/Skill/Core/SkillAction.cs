using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class SkillAction : ScriptableObject
{
	public abstract IEnumerator Action(Transform parent, SkillActionInfo info, Unit caster, Vector3 targetPos, AffectedInfo affectedInfo, LayerMask layerMask);
}
