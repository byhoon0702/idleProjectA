﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DissolveClip : PlayableAsset
{

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		var playable = ScriptPlayable<DissolveBehavior>.Create(graph);
		return playable;
	}
}
