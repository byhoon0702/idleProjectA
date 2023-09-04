using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class HyperClassObject : ItemObject
{


	[SerializeField] private TimelineAsset finishTimeline;

	[SerializeField] private GameObject hitEffectObject;
	[SerializeField] private GameObject attackEffectObject;

	[SerializeField] private long finishSkillTid;

	public long FinishSkillTid => finishSkillTid;



	public TimelineAsset FinishTimeline => finishTimeline;

	public GameObject HitEffectObject => hitEffectObject;
	public GameObject AttackEffectObject => attackEffectObject;


	public override void SetBasicData<T>(T data)
	{
		var hyperData = data as HyperData;
		tid = hyperData.tid;
		finishSkillTid = hyperData.finalSkillTid;
	}

}
