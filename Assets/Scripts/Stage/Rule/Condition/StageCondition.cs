
using System.Collections.Generic;
using UnityEngine;




public abstract class StageCondition : ScriptableObject
{
	public abstract bool CheckCondition();

	public abstract void OnUpdate(float time);
	public abstract void SetCondition();

}
