using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class EnterGameState : RootState
{
	public override void Init()
	{

	}
	public override async Task<IntroFSM> OnEnter()
	{
		elapsedTime = 0;

		return this;
	}

	public override void OnExit()
	{
	}
	public override IntroFSM RunNextState(float time)
	{
		return this;
	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 0.5f)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Oldman");
		}
	}
}
