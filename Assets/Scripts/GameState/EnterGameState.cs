using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGameState : RootState
{
	public override void OnEnter()
	{
		elapsedTime = 0;
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 0.1f)
		{
			string sceneName = PlayerPrefs.GetString("GameSceneName");
			if (sceneName.HasStringValue())
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("Oldman");
			}
			//VGameManager.it.ChangeState(GameState.LOADING);
		}
	}
}
