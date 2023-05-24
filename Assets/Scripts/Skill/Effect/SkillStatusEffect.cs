using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SkillStatusEffect : ScriptableObject
{
	/// <summary>
	/// Enter 느낌
	/// </summary>
	public abstract void ApplyEffect(HittableUnit targets);

	//public abstract void OnUpdate(Unit caster, float deltaTime);
}
