using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class StageRecordData
{
	public long tid;
	public StageType stageType;

	public int stageNumber;

	public string cumulativeDamage;

	public int killCount;


	private void SaveData(StageRecordData data)
	{
		tid = data.tid;
		stageType = data.stageType;

		stageNumber = data.stageNumber;
		cumulativeDamage = data.cumulativeDamage;
	}
	public void Save(StageRecordData data)
	{
		if (stageNumber <= data.stageNumber)
		{
			SaveData(data);
		}
	}
}


[CreateAssetMenu(fileName = "Stage Container", menuName = "ScriptableObject/Container/StageContainer", order = 1)]

public class StageContainer : BaseContainer
{
	[SerializeField] private StageRecordData stageRecords = new StageRecordData();
	[SerializeField] private List<StageRecordData> dungeonRecords = new List<StageRecordData>();
	[SerializeField] private StageRecordData towerRecords = new StageRecordData();
	[SerializeField] private List<StageRecordData> guardianRecords = new List<StageRecordData>();

	public Dictionary<StageType, List<RuntimeData.StageInfo>> stageDataList = new Dictionary<StageType, List<RuntimeData.StageInfo>>();

	public int SelectedStageNumber = 0;
	public override void Dispose()
	{

	}
	public override void DailyResetData()
	{

	}
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
		dungeonRecords = temp.dungeonRecords;
		towerRecords = temp.towerRecords;
		guardianRecords = temp.guardianRecords;
		SelectedStageNumber = temp.SelectedStageNumber;
	}


	public override void UpdateData()
	{
		stageDataList = new Dictionary<StageType, List<RuntimeData.StageInfo>>();

		var list = DataManager.Get<StageDataSheet>().infos;

		for (int i = 0; i < list.Count; i++)
		{
			if (stageDataList.ContainsKey(list[i].stageType) == false)
			{

				stageDataList.Add(list[i].stageType, new List<RuntimeData.StageInfo>());
			}
			for (int ii = 0; ii < list[i].stageListData.Length; ii++)
			{
				stageDataList[list[i].stageType].Add(new RuntimeData.StageInfo(list[i], list[i].stageListData[ii]));
			}
		}
	}

	public override void Load(UserDB _parent)
	{
		LoadScriptableObject();

		stageRecords = new StageRecordData();
		dungeonRecords = new List<StageRecordData>();
		towerRecords = new StageRecordData();
		guardianRecords = new List<StageRecordData>();
	}

	public StageMapObject GetStageMap(StageType type, long tid)
	{
		if (stageDataList.ContainsKey(type) == false)
		{
			return null;
		}

		foreach (var map in stageDataList[type])
		{

			if (map.stageData.tid == tid)
			{
				return map.itemObject;
			}
		}
		return null;
	}
	public RuntimeData.StageInfo GetNormalStage(int stageNumber)
	{
		return stageDataList[StageType.Normal].Find(x => x.StageNumber == stageNumber);
	}

	public RuntimeData.StageInfo GetStage(long dungeonTid, int stageNumber)
	{

		foreach (var stageList in stageDataList.Values)
		{

			foreach (var stage in stageList)
			{
				if (stage.stageData.dungeonTid == dungeonTid)
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


	public RuntimeData.StageInfo GetStage(StageType type, int stageNumber)
	{
		if (stageDataList.ContainsKey(type) == false)
		{
			return null;
		}

		var list = stageDataList[type];

		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].StageNumber == stageNumber)
			{
				return list[i];
			}
		}

		return null;
	}

	public void SavePlayStage(RuntimeData.StageInfo info, IdleNumber cumulativeDamage, int _killCount)
	{
		SavePlayStage(info.stageData.dungeonTid, info.StageType, info.StageNumber, cumulativeDamage, _killCount);
	}

	public void SavePlayStage(long _tid, StageType _type, int _stageNumber, IdleNumber _cumulativeDamage, int _killCount)
	{
		StageRecordData data = new StageRecordData()
		{
			tid = _tid,
			stageType = _type,
			stageNumber = _stageNumber,
			cumulativeDamage = _cumulativeDamage.ToString(),
			killCount = _killCount
		};


		switch (_type)
		{
			case StageType.Normal:
				stageRecords.Save(data);
				break;
			case StageType.Dungeon:
				{
					var record = dungeonRecords.Find(x => x.tid == _tid);
					if (record != null)
					{
						record.Save(data);
					}
					else
					{
						dungeonRecords.Add(data);
					}
				}
				break;
			case StageType.Tower:
				towerRecords.Save(data);

				break;
			case StageType.Guardian:
				{
					var record = guardianRecords.Find(x => x.tid == _tid);
					if (record != null)
					{
						record.Save(data);
					}
					else
					{
						guardianRecords.Add(data);
					}
				}

				break;
		}
	}

	public StageRecordData GetLastStage(StageType type, long tid)
	{
		StageRecordData data = new StageRecordData()
		{
			tid = tid,
			stageType = type,
			stageNumber = 1,
		};
		switch (type)
		{
			case StageType.Normal:
				data = stageRecords;
				break;
			case StageType.Dungeon:
				{
					var record = dungeonRecords.Find(x => x.tid == tid);
					if (record != null)
					{
						data = record;
					}
				}
				break;
			case StageType.Tower:
				data = towerRecords;
				break;
			case StageType.Guardian:
				{
					var record = guardianRecords.Find(x => x.tid == tid);
					if (record != null)
					{
						data = record;
					}
				}
				break;
		}
		return data;
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		foreach (var type in Enum.GetValues(typeof(StageType)))
		{
			string typename = type.ToString().ToLower().FirstCharacterToUpper();
			var mapList = Resources.LoadAll<StageMapObject>($"RuntimeDatas/Maps/{typename}s");
			if (mapList != null)
			{
				AddDictionary(scriptableDictionary, mapList);
			}

			var dungeonList = Resources.LoadAll<DungeonItemObject>($"RuntimeDatas/Dungeon/{typename}s");
			if (dungeonList != null)
			{
				AddDictionary(scriptableDictionary, dungeonList);
			}
		}


	}

	public StageRecordData GetStageRecordData(StageType _type, long _tid)
	{
		StageRecordData data = new StageRecordData();
		switch (_type)
		{
			case StageType.Normal:
				data = stageRecords;
				break;
			case StageType.Dungeon:
				data = dungeonRecords.Find(x => x.tid == _tid);
				break;
			case StageType.Tower:
				data = towerRecords;
				break;
			case StageType.Guardian:
				data = guardianRecords.Find(x => x.tid == _tid);
				break;
		}
		return data;
	}


	public StageRecordData GetStageRecordData(RuntimeData.StageInfo stageInfo)
	{
		StageRecordData data = GetStageRecordData(stageInfo.StageType, stageInfo.stageData.dungeonTid);

		if (data != null && data.stageNumber == stageInfo.StageNumber)
		{
			return data;
		}
		return null;
	}

	public List<RuntimeData.StageInfo> GetStageList(StageType _stageType, long _tid)
	{
		List<RuntimeData.StageInfo> stageList = new List<RuntimeData.StageInfo>();
		switch (_stageType)
		{
			case StageType.Normal:
			case StageType.Tower:
				stageList = stageDataList[_stageType];
				break;
			case StageType.Dungeon:
			case StageType.Guardian:
				if (_tid == 0)
				{
					stageList = stageDataList[_stageType];
				}
				else
				{
					stageList = stageDataList[_stageType].FindAll(x => x.stageData.dungeonTid == _tid);
				}
				break;
		}


		stageList.Sort((x, y) =>
		{
			return x.StageNumber.CompareTo(y.StageNumber);
		});

		return stageList;
	}

	public RuntimeData.StageInfo LastPlayedStage(StageType _stageType, long _battleTid)
	{

		var stageList = GetStageList(_stageType, _battleTid);

		StageRecordData data = GetStageRecordData(_stageType, _battleTid);

		int stageNumber = data == null ? 0 : data.stageNumber;
		if (stageNumber == 0)
		{
			return stageList[0];
		}

		RuntimeData.StageInfo current = null;
		for (int i = 0; i < stageList.Count; i++)
		{
			if (stageList[i].StageNumber == stageNumber)
			{
				current = stageList[i];
				break;
			}
		}

		if (current == null)
		{
			current = stageList[stageList.Count - 1];
		}

		return GetNextStage(current);
	}

	public RuntimeData.StageInfo LastPlayedNormalStage()
	{
		return LastPlayedStage(StageType.Normal, 0);
	}
	public RuntimeData.StageInfo GetNextStage(RuntimeData.StageInfo currentStage)
	{
		var list = stageDataList[currentStage.StageType].FindAll(x => x.stageData.dungeonTid == currentStage.stageData.dungeonTid);

		list.Sort((x, y) =>
		{
			return x.StageNumber.CompareTo(y.StageNumber);
		});

		int index = list.FindIndex(x => x.StageNumber == currentStage.StageNumber);
		int nextIndex = index + 1;
		if (nextIndex < list.Count)
		{
			return list[nextIndex];
		}
		return list[index];
	}

	public RuntimeData.StageInfo GetNextNormalStage(RuntimeData.StageInfo currentStage)
	{
		var list = stageDataList[StageType.Normal];

		list.Sort((x, y) =>
		{
			return x.StageNumber.CompareTo(y.StageNumber);
		});


		int index = list.FindIndex(x => x.StageNumber == currentStage.StageNumber);
		int nextIndex = index + 1;
		if (nextIndex < list.Count)
		{
			return list[nextIndex];
		}
		return list[index];
	}

	public bool IsCleared(StageType stageType, long tid, int stageNumber)
	{
		var stage = GetStageRecordData(stageType, tid);

		if (stage == null)
		{
			return false;
		}
		//저장된 지역 스테이지 번호가 같거나 높으면 클리어
		return stage.stageNumber >= stageNumber;
	}
}
