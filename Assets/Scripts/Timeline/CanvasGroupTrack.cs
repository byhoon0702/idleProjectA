using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(CanvasGroup))]
[TrackClipType(typeof(CanavsGroupClip))]
public class CanvasGroupTrack : TrackAsset
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return ScriptPlayable<CanvasGroupTrackMixer>.Create(graph, inputCount);
	}
}
