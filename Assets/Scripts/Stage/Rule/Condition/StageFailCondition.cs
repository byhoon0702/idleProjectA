using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FailType
{
	HP,
	TIME,

}


public abstract class StageFailCondition : StageCondition
{
	public override void OnUpdate(float time)
	{

	}
}
