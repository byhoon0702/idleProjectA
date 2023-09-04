using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IntroState : RootState
{
	[SerializeField] private DataLoadingState dataLoadingState;
	public override void Init()
	{

	}
	public override async Task<IntroFSM> OnEnter()
	{
		elapsedTime = 0;
		Intro.it.SetActiveProgressBar(false);
		return this;
	}

	public override void OnExit()
	{
	}
	public override IntroFSM RunNextState(float time)
	{
		elapsedTime += time;

		if (elapsedTime > 0.1f)
		{
			return dataLoadingState;
		}
		else
		{
			return this;
		}

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
