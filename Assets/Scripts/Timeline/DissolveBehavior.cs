using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DissolveBehavior : PlayableBehaviour
{

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		var unitManager = playerData as UnitManager;
		var time = playable.GetTime();
		var duration = playable.GetDuration();
		if (unitManager != null)
		{
			unitManager.EnemyDissovle((float)(time / duration));
		}
	}
}
