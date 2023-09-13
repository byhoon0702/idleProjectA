using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RuntimeData
{
	[Serializable]
	public class EventInfo : BaseInfo
	{
		public List<EventShopInfo> eventNormalList;
		public List<EventShopInfo> eventPackageList;
		public List<StageInfo> stageList;

		public StageRecordData eventDungeonRecord = new StageRecordData();
		public EventData rawData { get; private set; }

		public override void SetRawData<T>(T data)
		{
			rawData = data as EventData;
			tid = rawData.tid;

			eventNormalList = new List<EventShopInfo>();
			eventPackageList = new List<EventShopInfo>();

			var dataSheet = DataManager.Get<EventShopDataSheet>();
			for (int i = 0; i < rawData.eventShopTid.Count; i++)
			{
				long tid = rawData.eventShopTid[i];
				var shopData = dataSheet.Get(tid);
				var info = new EventShopInfo();
				info.SetRawData(shopData);

				if (shopData.shopType == ShopType.NORMAL)
				{
					eventNormalList.Add(info);
				}
				else if (shopData.shopType == ShopType.PACKAGE)
				{
					eventPackageList.Add(info);
				}
			}

			stageList = new List<StageInfo>();
			var stageSheet = DataManager.Get<EventStageDataSheet>();
			for (int i = 0; i < stageSheet.infos.Count; i++)
			{
				var stagedata = stageSheet.infos[i];
				for (int ii = 0; ii < stagedata.stageListData.Length; ii++)
				{
					StageInfo stageInfo = new StageInfo(stagedata, stagedata.stageListData[ii], true);

					stageInfo.SetItemObject(PlatformManager.UserDB.eventContainer.GetScriptableObject<StageMapObject>(stagedata.tid));
					stageList.Add(stageInfo);
				}
			}
		}

		public override void Load<T>(T info)
		{
			base.Load(info);
			EventInfo temp = info as EventInfo;
			var container = PlatformManager.UserDB.shopContainer;
			container.LoadListTidMatch(ref eventNormalList, temp.eventNormalList);
			container.LoadListTidMatch(ref eventPackageList, temp.eventPackageList);

			var stageContainer = PlatformManager.UserDB.stageContainer;
			eventDungeonRecord = temp.eventDungeonRecord;
		}

		public override void UpdateData()
		{
			for (int i = 0; i < stageList.Count; i++)
			{
				var stage = stageList[i];
				stage.isClear = IsCleared(stage.stageData.dungeonTid, stage.StageNumber);
			}
		}

		public void SaveEventStage(RuntimeData.StageInfo stage, IdleNumber damage, int _killCount)
		{
			StageRecordData data = new StageRecordData()
			{
				tid = stage.stageData.dungeonTid,
				stageType = stage.StageType,
				stageNumber = stage.StageNumber,
				cumulativeDamage = damage.ToString(),
				killCount = _killCount
			};
			eventDungeonRecord.Save(data);
		}

		public bool IsCleared(long tid, int stageNumber)
		{
			if (eventDungeonRecord.tid != tid)
			{
				return false;
			}

			return eventDungeonRecord.stageNumber >= stageNumber;
		}

		public List<EventShopInfo> Get(ShopType type)
		{
			switch (type)
			{
				case ShopType.NORMAL:
					return eventNormalList;
				case ShopType.PACKAGE:
					return eventPackageList;
			}
			return new List<EventShopInfo>();
		}
	}
}

[Serializable]
public class EventContainer : BaseContainer
{
	public List<RuntimeData.EventInfo> eventInfos = new List<RuntimeData.EventInfo>();
	public override void DailyResetData()
	{

	}

	public override void Dispose()
	{

	}

	public RuntimeData.EventInfo GetCurrentEvent()
	{
		return Get(RemoteConfigManager.Instance.Season);
	}

	public RuntimeData.EventInfo Get(string season)
	{
		return eventInfos.Find(x => x.rawData.season == season);
	}

	public override void FromJson(string json)
	{
		EventContainer temp = CreateInstance<EventContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		LoadListTidMatch(ref eventInfos, temp.eventInfos);
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();
		SetListRawData(ref eventInfos, DataManager.Get<EventDataSheet>().GetInfosClone());
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		foreach (var type in Enum.GetValues(typeof(StageType)))
		{
			string typename = type.ToString().ToLower().FirstCharacterToUpper();
			var mapList = Resources.LoadAll<StageMapObject>($"RuntimeDatas/EventMaps/{typename}s");
			if (mapList != null)
			{
				AddDictionary(scriptableDictionary, mapList);
			}

			var dungeonList = Resources.LoadAll<DungeonItemObject>($"RuntimeDatas/EventDungeon/{typename}s");
			if (dungeonList != null)
			{
				AddDictionary(scriptableDictionary, dungeonList);
			}
		}
	}

	public override string Save()
	{
		string json = JsonUtility.ToJson(this, true);
		return json;
	}

	public override void UpdateData()
	{
		for (int i = 0; i < eventInfos.Count; i++)
		{
			eventInfos[i].UpdateData();
		}
	}
}
