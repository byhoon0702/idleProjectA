using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class CanvasGroupBehavior : PlayableBehaviour
{
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{

		CanvasGroup canvasGroup = playerData as CanvasGroup;
		canvasGroup.alpha = info.weight;
	}
}
