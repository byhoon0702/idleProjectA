using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
	public UnitBase unit;

	public void Init(UnitBase _unit)
	{
		unit = _unit;
	}

	public void RightStepDown()
	{
		// GameUIManager.it.ShowStepFog(unit, true);
	}

	public void LeftStepDown()
	{
		// GameUIManager.it.ShowStepFog(unit, false);
	}

	public void SwordSwing()
	{

	}
}
