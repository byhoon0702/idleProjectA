using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
[System.Serializable]
public class UnitDissolveClip : PlayableAsset
{
	public float from;
	public float to;
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		var playable = ScriptPlayable<UnitDissolveBehavior>.Create(graph);

		UnitDissolveBehavior dissolveBehavior = playable.GetBehaviour();
		dissolveBehavior.from = from;
		dissolveBehavior.to = to;

		return playable;
	}
}
