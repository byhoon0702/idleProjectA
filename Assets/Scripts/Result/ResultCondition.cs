using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResultCondition : ScriptableObject
{
	public abstract bool IsWin();
	public abstract bool IsLose();
}
