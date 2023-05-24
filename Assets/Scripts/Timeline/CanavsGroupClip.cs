﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class CanavsGroupClip : PlayableAsset
{
	// Factory method that generates a playable based on this asset
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
	{
		return ScriptPlayable<CanvasGroupBehavior>.Create(graph);
	}
}