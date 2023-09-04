using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPhaseInfo
{
	public GameObject Prefab;
	public SkillCore PhaseChangeSkill;
	public List<SkillCore> SkillList;
}

public class EnemyObject : UnitObject
{
	public GameObject prefab;
	public AudioClip deathSoundClip;
	public List<EnemyPhaseInfo> phaseInfoList;
	public override void SetBasicData<T>(T data)
	{
		var _Data = data as EnemyUnitData;
		tid = _Data.tid;
		itemName = _Data.name;
		description = _Data.description;

		//if (phaseInfoList == null || (phaseInfoList != null && phaseInfoList.Count )_Data.maxPhase > 0)
		//{
		//	phaseInfoList = new List<EnemyPhaseInfo>();
		//	for (int i = 0; i < _Data.maxPhase; i++)
		//	{
		//		phaseInfoList.Add(new EnemyPhaseInfo());
		//	}
		//}

		var resource = Resources.Load($"B/Enemy/{_Data.resource}");
		if (resource != null)
		{
			prefab = resource as GameObject;
		}
	}
}
