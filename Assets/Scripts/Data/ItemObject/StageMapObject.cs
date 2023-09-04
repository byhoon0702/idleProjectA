using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[System.Serializable]
public class StageMapObject : ItemObject
{
	public StageType stageType;

	public GameObject mapPrefab;
	public AudioClip bgmClip;
	public StageRule rule;

	public override void SetBasicData<T>(T data)
	{
		var stageData = data as StageData;
		tid = stageData.tid;
		stageType = stageData.stageType;
		//	this.data = data;

	}
}
