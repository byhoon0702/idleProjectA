using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 경직, 넉백, 에어본 등등
/// </summary>

[Serializable]
public abstract class SkillNeutralizeEffect : ScriptableObject
{
	public abstract void ApplyEffect(HittableUnit target, Vector3 centerPos, int hitCount, bool isLastHit = true);



}
