using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[System.Serializable]
public class StageMapObject : ScriptableObject
{
	public long tid;
	public StageType stageType;
	//public DungeonStageData data;
	public GameObject mapPrefab;
	public AudioClip bgmClip;

	public void SetBasicData(DungeonStageData data)
	{
		tid = data.tid;
		stageType = data.stageType;
		//	this.data = data;

	}
}
