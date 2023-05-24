using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class LoginState : RootState
{
	public override void Init()
	{

	}
	public override FSM RunNextState(float time)
	{
		return this;
	}

	public override FSM OnEnter()
	{
		elapsedTime = 0;

		Intro.it.SetActiveTabToStart(true);
		if (Intro.it.userDB.LoadLoginData() == false)
		{
			Intro.it.uiPopupLogin.OnUpdate();
		}

		return this;
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
	}
}
