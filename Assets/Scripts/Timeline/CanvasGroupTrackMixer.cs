using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CanvasGroupTrackMixer : PlayableBehaviour
{
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		CanvasGroup canvasGroup = playerData as CanvasGroup;

		float alpha = 0;


		if (canvasGroup == null)
		{
			return;
		}

		int inputCount = playable.GetInputCount();
		for (int i = 0; i < inputCount; i++)
		{
			float inputWeight = playable.GetInputWeight(i);
			if (inputWeight > 0f)
			{
				ScriptPlayable<CanvasGroupBehavior> inputPlayable = (ScriptPlayable<CanvasGroupBehavior>)playable.GetInput(i);
				CanvasGroupBehavior input = inputPlayable.GetBehaviour();
				alpha = inputWeight;

			}
		}


		canvasGroup.alpha = alpha;
	}

}
