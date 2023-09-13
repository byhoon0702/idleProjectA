using System;
using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;


[System.Serializable]
public class ContentsInfo : BaseInfo
{
	[SerializeField] private bool isOpen;
	[SerializeField] private bool isContentUpdated;

	public bool ShowReddot => isOpen && isContentUpdated;
	public string Description { get; private set; }
	public bool IsOpen => isOpen;
	public ContentType type => rawData.Type;
	public ContentsData rawData { get; private set; }

	public override void Load<T>(T info)
	{
		if (info == null)
		{
			return;
		}
		ContentsInfo temp = info as ContentsInfo;
		isOpen = temp.isOpen;
		isContentUpdated = temp.isContentUpdated;
	}

	public override void SetRawData<T>(T data)
	{
		rawData = data as ContentsData;
		tid = rawData.tid;
		if (rawData.Condition.type == ConditionType.NONE)
		{
			isContentUpdated = false;
		}
	}

	public void UpdateOpenState()
	{
		isContentUpdated = true;
		//UpdateData();
	}

	public List<ContentsInfo> GetSubContent(List<ContentsInfo> infos)
	{
		List<ContentsInfo> relatives = new List<ContentsInfo>();
		string typeName = type.ToString();
		for (int i = 0; i < infos.Count; i++)
		{
			var info = infos[i];
			string sub = info.type.ToString();
			if (sub.Contains(typeName))
			{
				relatives.Add(info);
			}
		}

		return relatives;
	}


	public List<ContentsInfo> GetRootContent(List<ContentsInfo> infos)
	{
		string[] typeName = type.ToString().Split('_');
		if (typeName.Length == 1)
		{
			Debug.Log($"{type} is Root Content");
			return null;
		}
		List<ContentsInfo> relatives = new List<ContentsInfo>();
		for (int i = 0; i < infos.Count; i++)
		{
			string[] subNames = infos[i].type.ToString().Split('_');

		}


		return relatives;
	}
	public void EnterContent(List<ContentsInfo> infos)
	{
		//for (int i = 0; i < types.Length; i++)
		//{
		//	ContentType contentType;
		//	if (Enum.TryParse(types[i], out contentType))
		//	{
		//		var item = infos.Find((x) =>
		//		{
		//			x.type.ToString().Split('_')
		//			if (x.type.ToString())

		//				return true;

		//		}

		//		);
		//		if (item != null)
		//		{
		//			if (item.ShowReddot == true)
		//			{
		//				isContentUpdated = true;
		//				return;
		//			}
		//		}
		//	}
		//}
		//return;


		isContentUpdated = false;
	}

	public override void UpdateData()
	{
		OpenCondition condition = rawData.Condition;
		switch (condition.type)
		{
			case ConditionType.NONE:
				isOpen = true;
				break;
			case ConditionType.QUEST:
				{
					var questInfo = PlatformManager.UserDB.questContainer.MainQuestList.Find(x => x.Tid == condition.tid);
					if (questInfo == null)
					{
						isOpen = true;
					}
					else
					{
						isOpen = questInfo.progressState == QuestProgressState.END;

						Description = $"{PlatformManager.Language[questInfo.rawData.questTitle]} 완료시 오픈";
					}
				}
				break;
			case ConditionType.USELEVEL:
				{
					isOpen = PlatformManager.UserDB.userInfoContainer.userInfo.UserLevel >= condition.parameter;

					Description = $"유저 레벨 {condition.parameter} 달성시 오픈";
				}
				break;
			case ConditionType.STAGE:
				{
					long dungeonTid = condition.tid;
					int stageNumber = condition.parameter;

					var stage = PlatformManager.UserDB.stageContainer.GetNormalStage(stageNumber);

					if (stage == null)
					{
						isOpen = false;
						VLog.LogError($"잘못된 데이터 :{dungeonTid}  {stageNumber}");
						return;
					}
					isOpen = stage.isClear;

					Description = $"STAGE {stage.StageNumber} {stage.StageName} 클리어시 오픈";
				}
				break;
			case ConditionType.CONTENT:
				{
					isOpen = PlatformManager.UserDB.contentsContainer.IsOpen(condition.content);
					Description = $"{condition.content.ToUIString()} 오픈시 오픈";
				}
				break;
		}
	}
}

[CreateAssetMenu(fileName = "Contents Container", menuName = "ScriptableObject/Container/Contents", order = 1)]
public class ContentsContainer : BaseContainer
{
	private static event Action<List<ContentsInfo>> OnContentsOpenEvent;
	private static event Action<ContentType> OnReddotEvent;

	[SerializeField] private List<ContentsInfo> infoList = new List<ContentsInfo>();
	public override void FromJson(string json)
	{
		ContentsContainer temp = CreateInstance<ContentsContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);
		LoadListTidMatch(ref infoList, temp.infoList);
	}
	public override void Dispose()
	{

	}
	public override void Load(UserDB _parent)
	{
		parent = _parent;
		SetListRawData(ref infoList, DataManager.Get<ContentsDataSheet>().GetInfosClone());
	}

	public override void LoadScriptableObject()
	{

	}
	public static void UpdateEvent(Action<List<ContentsInfo>> action)
	{
		RemoveEvent(action);
		AddEvent(action);
	}
	public static void AddEvent(Action<List<ContentsInfo>> action)
	{
		if (OnContentsOpenEvent.IsRegistered(action) == false)
		{
			OnContentsOpenEvent += action;
		}
	}
	public static void RemoveEvent(Action<List<ContentsInfo>> action)
	{
		if (OnContentsOpenEvent.IsRegistered(action))
		{
			OnContentsOpenEvent -= action;
		}
	}

	public static void AddReddotEvent(Action<ContentType> action)
	{
		if (OnReddotEvent.IsRegistered(action))
		{
			OnReddotEvent += action;
		}
	}
	public static void RemoveReddotEvent(Action<ContentType> action)
	{
		if (OnReddotEvent.IsRegistered(action))
		{
			OnReddotEvent -= action;
		}
	}
	public override void DailyResetData()
	{

	}
	public override string Save()
	{
		string json = JsonUtility.ToJson(this, true);
		return json;
	}

	public override void UpdateData()
	{
		for (int i = 0; i < infoList.Count; i++)
		{
			infoList[i].UpdateData();
		}
		OnContentsOpenEvent?.Invoke(infoList);

		StageManager.StageClearEvent += OpenContentsByStage;
		QuestContainer.OnQuestCompleteEvent += OpenContentsByQuest;
		UserInfoContainer.OnLevelUpEvent += OpenContentsByUserLevel;
	}

	public bool TryEnter(ContentType type)
	{
#if UNITY_EDITOR
		if (PlatformManager.ConfigMeta.CheckContent == false)
		{
			return true;
		}
#endif

		if (type == ContentType.NONE)
		{
			return true;
		}

		if (type == ContentType.EVENT)
		{
			string season = RemoteConfigManager.Instance.Season;
			if (season.IsNullOrEmpty() || season.Equals("Default") || season.Equals("default"))
			{
				return false;
			}
			return true;
		}

		var contents = infoList.Find(x => x.type == type);
		if (contents == null)
		{
			return false;
		}

		contents.UpdateData();
		if (contents.IsOpen == false)
		{
			ToastUI.Instance.Enqueue(contents.Description);
			return false;
		}
		contents.EnterContent(infoList);

		OnContentsOpenEvent?.Invoke(infoList);
		return true;
	}


	public ContentsInfo Get(ContentType type)
	{
		ContentType contentType = ContentType.NONE;
		if (Enum.IsDefined(typeof(ContentType), type) == false)
		{
			return null;
		}

		contentType = type;
		if (contentType == ContentType.NONE)
		{
			return null;
		}

		var contents = infoList.Find(x => x.type == contentType);
		return contents;
	}

	public bool IsOpen(ContentType type, bool showToast = false)
	{
#if UNITY_EDITOR
		if (PlatformManager.ConfigMeta.CheckContent == false)
		{
			return true;
		}
#endif

		ContentType contentType = ContentType.NONE;
		if (Enum.IsDefined(typeof(ContentType), type) == false)
		{
			return false;
		}


		contentType = type;
		if (contentType == ContentType.NONE)
		{
			return true;
		}

		if (type == ContentType.EVENT)
		{
			string season = RemoteConfigManager.Instance.Season;
			if (season.IsNullOrEmpty() || season.Equals("Default") || season.Equals("default"))
			{
				ToastUI.Instance.EnqueueKey("str_ui_warn_no_event");
				return false;
			}

			return true;
		}

		var contents = infoList.Find(x => x.type == contentType);
		if (contents == null)
		{
			if (showToast)
			{
				ToastUI.Instance.Enqueue($"{type} 데이터가 없습니다");
			}
			return false;
		}

		if (contents.IsOpen == false)
		{
			if (showToast)
			{
				ToastUI.Instance.Enqueue(contents.Description);
			}
		}

		return contents.IsOpen;
	}


	private void EnqueueOpenContentsPopup(List<ContentsInfo> infos)
	{
		List<RewardInfo> rewardInfos = new List<RewardInfo>();
		for (int i = 0; i < infos.Count; i++)
		{
			var info = infos[i];
			info.UpdateOpenState();
			for (int ii = 0; ii < info.rawData.RewardList.Count; ii++)
			{
				rewardInfos.Add(new RewardInfo(info.rawData.RewardList[ii]));
			}
			GameUIManager.it.AddContentOpenMessage(new ContentOpenMessage($"{info.rawData.Type.ToUIString()} 오픈!!", rewardInfos));
		}
		PlatformManager.UserDB.AddRewards(rewardInfos, false, false);

		//OnContentsOpenEvent?.Invoke(infoList);
	}



	public delegate bool Checker(ContentsInfo info);
	private List<ContentsInfo> OpenContents(List<ContentsInfo> list, Checker checker)
	{

		if (list == null || list.Count == 0)
		{
			return null;
		}

		List<ContentsInfo> openContents = new List<ContentsInfo>();
		for (int i = 0; i < list.Count; i++)
		{
			if (checker(list[i]))
			{
				if (list[i].IsOpen == false)
				{
					openContents.Add(list[i]);
				}

			}
			list[i].UpdateData();
		}

		OnContentsOpenEvent?.Invoke(list);
		EnqueueOpenContentsPopup(openContents);

		return openContents;
	}
	public void OpenContentsByStage(StageInfo info)
	{
		var list = infoList.FindAll(x => x.rawData.Condition.type == ConditionType.STAGE);
		if (list == null)
		{
			return;
		}

		var openedList = OpenContents(list, (content) =>
		{
			return content.rawData.Condition.tid == info.stageData.tid && content.rawData.Condition.parameter == info.StageNumber;
		});
		OpenContentsByContents(openedList);
	}
	public void OpenContentsByQuest(QuestInfo quest)
	{
		if (quest == null)
		{
			return;
		}
		var list = infoList.FindAll(x => x.rawData.Condition.type == ConditionType.QUEST);
		if (list == null)
		{
			return;
		}

		var openedList = OpenContents(list, (content) => { return content.rawData.Condition.tid == quest.Tid && quest.progressState == QuestProgressState.END; });
		OpenContentsByContents(openedList);
	}

	public void OpenContentsByUserLevel(int level)
	{
		var list = infoList.FindAll(x => x.rawData.Condition.type == ConditionType.USELEVEL);
		if (list == null)
		{
			return;
		}
		var openedList = OpenContents(list, (content) => { return content.rawData.Condition.parameter <= level; });
		OpenContentsByContents(openedList);
	}
	private void OpenContentsByContents(List<ContentsInfo> infos)
	{
		if (infos == null)
		{
			return;
		}
		if (infos.Count == 0)
		{
			return;
		}
		var list = infoList.FindAll(x => x.rawData.Condition.type == ConditionType.CONTENT);
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < infos.Count; i++)
		{
			OpenContents(list, (content) => { return content.rawData.Condition.content == infos[i].type; });
		}
	}
}
