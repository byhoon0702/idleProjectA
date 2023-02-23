using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroState : RootState
{

	public override void OnEnter()
	{
		elapsedTime = 0;
		Intro.it.SetActiveProgressBar(false);
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;

		if (elapsedTime > 0.1f)
		{
			Intro.it.ChangeState(IntroState_e.DATALOADING);
		}
	}
}
