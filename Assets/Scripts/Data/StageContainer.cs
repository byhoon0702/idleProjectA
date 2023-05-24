using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;


[System.Serializable]
public class StageInfo
{

	public int playCount;
	public bool isClear;
	public int totalSpawnCount;
	public int totalBossSpawnCount;

	#region Static Property
	public StageType stageType => dungeonData.stageType;
	public int AreaNumber => dungeonData.areaNumber;
	public int StageNumber => data.stageNumber;

	public string StageName => $"{Name} {AreaNumber}-{StageNumber}";
	public StageDifficulty Difficulty => dungeonData.difficulty;
	public string Name => dungeonData.name;
	public float TimeLimit => dungeonData.timelimit;
	public int DisplayUnitCount => dungeonData.enemyDisplayCount;
	public int CountLimit => dungeonData.enemyCountLimit;
	public int BossCountLimit => dungeonData.bossCountLimit;
	#endregion
	public float SpawnMinDistance = 3;
	public float SpawnMaxDistance = 5;

	public List<UnitData> spawnEnemyInfos { get; private set; }
	public List<UnitData> spawnBoss { get; private set; }
	public StageData data { get; private set; }
	public DungeonStageData dungeonData { get; private set; }
	public StageInfo(DungeonStageData _dungeonData, StageData _data)
	{
		dungeonData = _dungeonData;
		data = _data;
		spawnEnemyInfos = new List<UnitData>();
		spawnBoss = new List<UnitData>();
		for (int i = 0; i < dungeonData.spawnList.Count; i++)
		{
			var unit = DataManager.Get<EnemyUnitDataSheet>().Get(dungeonData.spawnList[i].tid);
			if (dungeonData.spawnList[i].isBoss)
			{
				spawnBoss.Add(unit);
			}
			else
			{
				spawnEnemyInfos.Add(unit);
			}

		}

		if (_data.overrideSpawnList != null && _data.overrideSpawnList.Count > 0)
		{
			spawnEnemyInfos.Clear();
			spawnBoss.Clear();
			for (int i = 0; i < _data.overrideSpawnList.Count; i++)
			{
				var unit = DataManager.Get<EnemyUnitDataSheet>().Get(_data.overrideSpawnList[i].tid);
				if (_data.overrideSpawnList[i].isBoss)
				{
					spawnBoss.Add(unit);
				}
				else
				{
					spawnEnemyInfos.Add(unit);
				}
			}
		}

	}

	public IdleNumber UnitAttackPower(bool isBoss)
	{
		//int difficultLv = ((int)Difficulty + 1);
		float weight_1 = GameManager.Config.UNIT_STATE_WEIGHT_1;
		float weight_2 = GameManager.Config.UNIT_STATE_WEIGHT_2;
		float weight_3 = GameManager.Config.UNIT_STATE_WEIGHT_3;
		float atkWeight = GameManager.Config.UNIT_STATE_ATTACK_POWER_WEIGHT;

		IdleNumber defaultValue = (IdleNumber)dungeonData.monsterStats.attackPower;
		float levelWeight = data.stageNumber + data.levelWeight;
		float bossWeight = isBoss ? 1 + data.bossWeight : 1;

		var result = (defaultValue * (levelWeight * weight_2) * (bossWeight * weight_3) * atkWeight);
		//var result = ((defaultValue + (defaultValue * ((difficultLv - 1) * weight_1)))) * (1 + ((levelWeight - 1) * weight_2)) * (bossWeight + ((bossWeight - 1)) * weight_3) * (StageLv + (StageLv - 1) * atkWeight);

		return result;
	}

	public IdleNumber UnitHP(bool isBoss)
	{
		//int difficultLv = ((int)Difficulty + 1);
		float weight_1 = GameManager.Config.UNIT_STATE_WEIGHT_1;
		float weight_2 = GameManager.Config.UNIT_STATE_WEIGHT_2;
		float weight_3 = GameManager.Config.UNIT_STATE_WEIGHT_3;
		float hpWeight = GameManager.Config.UNIT_STATE_HP_WEIGHT;

		IdleNumber defaultValue = (IdleNumber)dungeonData.monsterStats.hp;
		float levelWeight = data.stageNumber + data.levelWeight;
		float bossWeight = isBoss ? 1 + data.bossWeight : 1;

		var result = (defaultValue * (levelWeight * weight_2) * (bossWeight * weight_3) * hpWeight);
		//var result = ((defaultValue + (defaultValue * ((StageLv - 1) + (StageLv - 1) * ((difficultLv - 1) * weight_1))))) * (1 + ((levelWeight - 1) * weight_2)) * (bossWeight + ((bossWeight - 1)) * weight_3) * (StageLv + (StageLv - 1) * hpWeight);

		return result;
	}

}
[System.Serializable]
public class StageRecordData
{
	public long tid;
	public StageType stageType;
	public StageDifficulty stageDifficulty;
	public int areaNumber;
	public int stageNumber;
}

[System.Serializable]
public class StageRecord : SerializableDictionary<StageType, StageRecordData>
{ }


[System.Serializable]
public class StageMapObjectList
{
	public List<StageMapObject> list;
	public StageMapObjectList()
	{
		this.list = new List<StageMapObject>();
	}
}

[System.Serializable]
public class StageMapObjectDictionary : SerializableDictionary<StageType, StageMapObjectList>
{ }



[CreateAssetMenu(fileName = "Stage Container", menuName = "ScriptableObject/Container/StageContainer", order = 1)]

public class StageContainer : BaseContainer
{
	public StageRecord stageRecords = new StageRecord();
	public Dictionary<StageType, List<StageInfo>> stageDataList = new Dictionary<StageType, List<StageInfo>>();
	[SerializeField] private StageMapObjectDictionary mapObjectList;

#if UNITY_EDITOR
	public void SetDataList(StageMapObjectDictionary list)
	{
		mapObjectList = list;
	}
#endif
	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void FromJson(string json)
	{
		StageContainer temp = CreateInstance<StageContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);
		stageRecords = temp.stageRecords;
		//for (int i = 0; i < stageRecords.Count; i++)
		//{
		//	if (i < temp.stageRecords.Count)
		//	{
		//		stageRecords[i] = temp.stageRecords[i];
		//	}
		//}

	}
	public override void Load(UserDB _parent)
	{
		LoadScriptableObject();
		var list = DataManager.Get<DungeonStageDataSheet>().infos;
		for (int i = 0; i < list.Count; i++)
		{
			if (stageDataList.ContainsKey(list[i].stageType) == false)
			{

				stageDataList.Add(list[i].stageType, new List<StageInfo>());
			}
			for (int ii = 0; ii < list[i].stageData.Length; ii++)
			{
				stageDataList[list[i].stageType].Add(new StageInfo(list[i], list[i].stageData[ii]));
			}
		}
	}

	public StageMapObject GetStageMap(StageType type, long tid)
	{
		if (mapObjectList.ContainsKey(type) == false)
		{
			return null;
		}

		foreach (var map in mapObjectList[type].list)
		{

			if (map.tid == tid)
			{
				return map;
			}
		}
		return null;

	}

	public StageInfo GetStage(long dungeonTid, int stageNumber)
	{

		foreach (var stageList in stageDataList.Values)
		{

			foreach (var stage in stageList)
			{
				if (stage.dungeonData.tid == dungeonTid)
				{
					if (stage.StageNumber == stageNumber)
					{
						return stage;
					}
				}
			}
		}
		return null;
	}


	public StageInfo GetStage(StageType type, int level, StageDifficulty difficulty)
	{
		if (stageDataList.ContainsKey(type) == false)
		{
			return null;
		}

		var list = stageDataList[type];


		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].StageNumber == level && list[i].Difficulty == difficulty)
			{
				return list[i];
			}
		}


		return null;
	}

	public List<StageInfo> GetStages(StageType type, StageDifficulty difficulty)
	{
		if (stageDataList.ContainsKey(type) == false)
		{
			return null;
		}

		var list = stageDataList[type];
		List<StageInfo> filteredList = new List<StageInfo>();

		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Difficulty == difficulty)
			{
				filteredList.Add(list[i]);
			}
		}

		return filteredList;
	}


	public void SavePlayStage(StageInfo info)
	{
		SavePlayStage(info.dungeonData.tid, info.stageType, info.Difficulty, info.AreaNumber, info.StageNumber);
	}

	public void SavePlayStage(long _tid, StageType _type, StageDifficulty _difficulty, int _areaNumber, int _stageNumber)
	{
		if (stageRecords.ContainsKey(_type))
		{
			stageRecords[_type] = new StageRecordData() { tid = _tid, stageType = _type, stageDifficulty = _difficulty, areaNumber = _areaNumber, stageNumber = _stageNumber };
		}
		else
		{
			stageRecords.Add(_type, new StageRecordData() { tid = _tid, stageType = _type, stageDifficulty = _difficulty, areaNumber = _areaNumber, stageNumber = _stageNumber });
		}
	}

	public StageRecordData GetLastStage(StageType type)
	{
		if (stageRecords.ContainsKey(type) == false)
		{
			return new StageRecordData() { stageType = StageType.Normal, stageDifficulty = StageDifficulty.EASY, stageNumber = 1 };
		}
		return stageRecords[type];
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		foreach (var type in Enum.GetValues(typeof(StageType)))
		{
			string typename = type.ToString().ToLower().FirstCharacterToUpper();
			var mapList = Resources.LoadAll<StageMapObject>($"RuntimeDatas/Maps/{typename}s");

			AddDictionary(scriptableDictionary, mapList);
		}
	}

	public StageInfo LastPlayedStage()
	{
		if (stageRecords == null || stageRecords.Count == 0 || stageRecords.ContainsKey(StageType.Normal) == false)
		{
			return stageDataList[StageType.Normal][0];
		}

		if (stageRecords[StageType.Normal] == null)
		{
			stageRecords[StageType.Normal] = GetLastStage(StageType.Normal);
		}

		StageDifficulty difficulty = stageRecords[StageType.Normal].stageDifficulty;
		int stageNumber = stageRecords[StageType.Normal].stageNumber;

		long tid = stageRecords[StageType.Normal].tid;
		if (tid == 0)
		{
			return stageDataList[StageType.Normal][0];
		}

		var current = stageDataList[StageType.Normal].Find(x => x.dungeonData.tid == tid && x.StageNumber == stageNumber);

		return GetNextNormalStage(current);
	}


	public StageInfo GetNextNormalStage(StageInfo currentStage)
	{
		var list = stageDataList[StageType.Normal];

		list.Sort((x, y) =>
		{
			if (x.Difficulty.CompareTo(y.Difficulty) == 0)
			{
				return x.AreaNumber.CompareTo(y.AreaNumber);
			}
			else
			{
				return x.Difficulty.CompareTo(y.Difficulty);
			}
		});

		int index = list.FindIndex(x => x.AreaNumber == currentStage.AreaNumber && x.Difficulty == currentStage.Difficulty && x.StageNumber == currentStage.StageNumber);
		int nextIndex = index + 1;
		if (nextIndex < list.Count)
		{
			return list[nextIndex];
		}
		return list[index];
	}

	public bool IsCleared(StageType stageType, StageDifficulty difficulty, int areaNumber, int stageNumber)
	{
		if (stageRecords.ContainsKey(stageType) == false)
		{
			return false;
		}

		var stage = stageRecords[stageType];
		var data = DataManager.Get<DungeonStageDataSheet>().Get(stage.tid);
		if (data == null)
		{
			Debug.LogWarning($"{stage.tid} is null");
			return false;
		}
		if (data.difficulty < difficulty)
		{
			return false;
		}

		if (data.areaNumber < areaNumber)
		{
			return false;
		}

		if (stage.stageNumber < stageNumber)
		{
			return false;
		}

		return true;
	}
}
