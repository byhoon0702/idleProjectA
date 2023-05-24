using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGameState : RootState
{
	public override void Init()
	{

	}
	public override FSM OnEnter()
	{
		elapsedTime = 0;
		return this;
	}

	public override void OnExit()
	{
	}
	public override FSM RunNextState(float time)
	{
		return this;
	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 0.1f)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Oldman");
		}
	}
}
