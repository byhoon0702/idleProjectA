using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillScreenEffect : ScriptableObject
{
	public abstract void DoEffect(float multi = 1f);
}
