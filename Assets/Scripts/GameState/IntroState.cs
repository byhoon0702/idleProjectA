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

		Intro.it.videoPlayer.loopPointReached += OnPointReached;
		Intro.it.videoPlayer.enabled = true;
		Intro.it.videoPlayer.Play();
		return this;
	}

	private void OnPointReached(UnityEngine.Video.VideoPlayer source)
	{
		Intro.it.ChangeState(IntroState_e.DATALOADING);
		Intro.it.videoPlayer.enabled = false;
	}

	public override void OnExit()
	{
		Intro.it.videoPlayer.loopPointReached -= OnPointReached;
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
		//if (Intro.it.videoPlayer)
		//	elapsedTime += time;

		//if (elapsedTime > 0.1f)
		//{
		//	Intro.it.ChangeState(IntroState_e.DATALOADING);
		//}
	}
}
