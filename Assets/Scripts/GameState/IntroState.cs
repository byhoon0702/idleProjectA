using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroState : RootState
{

	public override void OnEnter()
	{
		elapsedTime = 0;
		GameUIManager.it.mainUIObject.SetActive(false);
		GameUIManager.it.introObject.SetActive(true);
	}

	public override void OnExit()
	{
		GameUIManager.it.introObject.SetActive(false);
	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
	}
}
