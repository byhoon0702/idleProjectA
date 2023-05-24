using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DissolveBehavior : PlayableBehaviour
{

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		var unitManager = playerData as UnitManager;
		unitManager.EnemyDissovle(info.weight);
	}
}
