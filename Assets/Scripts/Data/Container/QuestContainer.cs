using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuestDictionary : SerializableDictionary<QuestType, List<QuestData>>
{ }


[CreateAssetMenu(fileName = "QuestContainer", menuName = "ScriptableObject/Container/QuestContainer", order = 1)]
public class QuestContainer : BaseContainer
{
	[SerializeField] private List<RuntimeData.QuestInfo> mainQuestList;
	[SerializeField] private List<RuntimeData.QuestInfo> dailyQuestList;
	[SerializeField] private List<RuntimeData.QuestInfo> repeatQuestList;

	public List<RuntimeData.QuestInfo> MainQuestList => mainQuestList;

	private RuntimeData.QuestInfo currentMainQuest;
	public RuntimeData.QuestInfo CurrentMainQuest
	{
		get
		{
			allQuestClear = true;
			if (mainQuestList == null || mainQuestList.Count == 0)
			{
				return null;
			}

			if (currentMainQuest == null || currentMainQuest.Tid == 0 || currentMainQuest.progressState == QuestProgressState.END)
			{
				for (int i = 0; i < mainQuestList.Count; i++)
				{
					if (mainQuestList[i].progressState != QuestProgressState.END)
					{
						allQuestClear = false;
						OnQuestCompleteEvent?.Invoke(currentMainQuest);
						currentMainQuest = mainQuestList[i];
						currentMainQuest.SetReward();
						break;
					}
				}
			}


			if (currentMainQuest == null || currentMainQuest.Tid == 0)
			{

				currentMainQuest = mainQuestList[0];
				currentMainQuest.SetReward();

			}
			if (currentMainQuest.progressState == QuestProgressState.END)
			{
				return null;
			}
			if (currentMainQuest.progressState == QuestProgressState.ACTIVE || currentMainQuest.progressState == QuestProgressState.NONE)
			{
				currentMainQuest.progressState = QuestProgressState.ONPROGRESS;
			}
			return currentMainQuest;
		}
	}
	public List<RuntimeData.QuestInfo> DailyQuestList => dailyQuestList;
	public List<RuntimeData.QuestInfo> RepeatQuestList => repeatQuestList;

	public static event Action OnUpdate;

	public bool allQuestClear { get; private set; }
	public override void Dispose()
	{

	}
	public override void FromJson(string json)
	{
		QuestContainer temp = CreateInstance<QuestContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		LoadListTidMatch(ref mainQuestList, temp.mainQuestList);
		LoadListTidMatch(ref dailyQuestList, temp.dailyQuestList);
		LoadListTidMatch(ref repeatQuestList, temp.repeatQuestList);
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		SetListRawData(ref mainQuestList, DataManager.Get<QuestDataSheet>().GetDataByType(QuestType.MAIN));
		SetListRawData(ref dailyQuestList, DataManager.Get<QuestDataSheet>().GetDataByType(QuestType.DAILY));
		SetListRawData(ref repeatQuestList, DataManager.Get<QuestDataSheet>().GetDataByType(QuestType.REPEAT));
		currentMainQuest = null;
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
	}

	public override string Save()
	{
		string json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void DailyResetData()
	{
		for (int i = 0; i < dailyQuestList.Count; i++)
		{
			dailyQuestList[i].Reset();
		}
	}
	public void ProgressAdd(QuestGoalType goal, long tid, IdleNumber count)
	{
		CurrentMainQuest?.OnChange(goal, tid, count);
		for (int i = 0; i < DailyQuestList.Count; i++)
		{
			DailyQuestList[i].OnChange(goal, tid, count);
		}
		for (int i = 0; i < RepeatQuestList.Count; i++)
		{
			RepeatQuestList[i].OnChange(goal, tid, count);
		}
		OnQuestUpdate();
	}
	public void ProgressOverwrite(QuestGoalType goal, long tid, IdleNumber count)
	{
		CurrentMainQuest?.OnChange(goal, tid, count, true);
		for (int i = 0; i < DailyQuestList.Count; i++)
		{
			DailyQuestList[i].OnChange(goal, tid, count, true);
		}
		for (int i = 0; i < RepeatQuestList.Count; i++)
		{
			RepeatQuestList[i].OnChange(goal, tid, count, true);
		}
		OnQuestUpdate();
	}



	public void OnQuestUpdate()
	{
		int completeCount = 0;
		for (int i = 0; i < DailyQuestList.Count; i++)
		{
			if (DailyQuestList[i].progressState == QuestProgressState.END || DailyQuestList[i].progressState == QuestProgressState.COMPLETE)
			{
				completeCount++;
			}
		}

		IdleNumber count = (IdleNumber)completeCount;
		for (int i = 0; i < DailyQuestList.Count; i++)
		{
			DailyQuestList[i].OnChange(QuestGoalType.DAILY_COMPLETE, 0, count, true);
		}

		OnUpdate?.Invoke();
	}

	public override void UpdateData()
	{
		SetReward();
		OnQuestUpdate();
	}
	public void SetReward()
	{
		CurrentMainQuest?.SetReward();
		for (int i = 0; i < DailyQuestList.Count; i++)
		{
			DailyQuestList[i].SetReward();
		}
		for (int i = 0; i < RepeatQuestList.Count; i++)
		{
			RepeatQuestList[i].SetReward();
		}
	}

	public void ReceiveMainQuestReward()
	{
		if (CurrentMainQuest == null)
		{
			return;
		}
		if (CurrentMainQuest.progressState != QuestProgressState.COMPLETE)
		{
			return;
		}

		CurrentMainQuest?.OnGetReward(true, true);

	}

	public static event Action<RuntimeData.QuestInfo> OnQuestCompleteEvent;

	public RuntimeData.QuestInfo GetNonMainQuest()
	{
		RuntimeData.QuestInfo dailyinfo = null;

		List<RuntimeData.QuestInfo> temp = new List<RuntimeData.QuestInfo>(DailyQuestList);
		temp.Sort((x, y) =>
		{

			int compare = y.progressState.CompareTo(x.progressState);

			if (compare == 0)
			{
				float yRatio = y.count / y.GoalCount;
				float xRatio = x.count / x.GoalCount;
				return yRatio.CompareTo(xRatio);
			}
			else
			{
				return compare;
			}
		});
		for (int i = 0; i < temp.Count; i++)
		{
			if (temp[i].progressState == QuestProgressState.END || temp[i].progressState == QuestProgressState.NONE)
			{
				continue;
			}
			dailyinfo = DailyQuestList.Find(x => x.Tid == temp[i].Tid);
			break;
		}


		RuntimeData.QuestInfo repeatinfo = null;
		temp = new List<RuntimeData.QuestInfo>(RepeatQuestList);
		temp.Sort((x, y) =>
		{

			int compare = y.progressState.CompareTo(x.progressState);

			if (compare == 0)
			{
				float yRatio = y.count / y.GoalCount;
				float xRatio = x.count / x.GoalCount;
				return yRatio.CompareTo(xRatio);
			}
			else
			{
				return compare;
			}
		});

		for (int i = 0; i < temp.Count; i++)
		{
			if (temp[i].progressState == QuestProgressState.END || temp[i].progressState == QuestProgressState.NONE)
			{
				continue;
			}
			repeatinfo = RepeatQuestList.Find(x => x.Tid == temp[i].Tid);
			break;
		}


		if (dailyinfo != null && dailyinfo.progressState == QuestProgressState.COMPLETE)
		{
			return dailyinfo;
		}
		if (repeatinfo != null && repeatinfo.progressState == QuestProgressState.COMPLETE)
		{
			return repeatinfo;
		}

		if (dailyinfo != null)
		{
			return dailyinfo;
		}

		return repeatinfo;
	}

	public void ReceiveQuestRewardAll(QuestType type)
	{
		switch (type)
		{
			case QuestType.MAIN:
				ReceiveMainQuestReward();

				break;
			case QuestType.DAILY:
				for (int i = 0; i < DailyQuestList.Count; i++)
				{
					DailyQuestList[i].OnGetReward(true, true);
				}
				break;
			case QuestType.REPEAT:
				for (int i = 0; i < RepeatQuestList.Count; i++)
				{
					RepeatQuestList[i].OnGetReward(true, true);
				}
				break;
		}
	}
}
