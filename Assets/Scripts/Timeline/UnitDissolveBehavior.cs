using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UnitDissolveBehavior : PlayableBehaviour
{
	public float from;
	public float to;
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		UnitAnimation unitAnimation = playerData as UnitAnimation;

		if (unitAnimation == null)
		{
			return;
		}

		double duration = playable.GetDuration();
		double time = playable.GetTime();

		unitAnimation.PlayDissolve(Mathf.Lerp(from, to, (float)(time / duration) * 1.5f));
	}
}
