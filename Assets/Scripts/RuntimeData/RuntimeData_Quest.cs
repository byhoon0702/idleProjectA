using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeData
{
	[System.Serializable]
	public class QuestInfo : BaseInfo
	{
		[SerializeField][ReadOnly(false)] private string progressCount;
		private IdleNumber goalCount;
		public IdleNumber GoalCount => goalCount;

		[ReadOnly(false)] public QuestProgressState progressState;

		public QuestType Type => rawData.questType;
		public QuestGoalType GoalType => rawData.goalType;
		public long GoalTid => rawData.goalTid;
		public IdleNumber count { get; private set; }

		public QuestData rawData { get; private set; }

		public List<RewardInfo> rewardInfos { get; private set; } = new List<RewardInfo>();

		public void Reset()
		{
			progressCount = "0";
			if (Type != QuestType.MAIN)
			{
				progressState = QuestProgressState.ACTIVE;
			}
			else
			{
				progressState = QuestProgressState.NONE;
			}
			count = (IdleNumber)0;
		}

		public override void SetRawData<T>(T data) where T : class
		{
			rawData = data as QuestData;
			tid = rawData.tid;

			IdleNumber.TryConvert(rawData.goalValue, out goalCount);

			if (goalCount == 0)
			{
				goalCount = (IdleNumber)1;
			}

			if (Type != QuestType.MAIN)
			{
				progressState = QuestProgressState.ACTIVE;
			}
			else
			{
				progressState = QuestProgressState.NONE;
			}
		}
		public void SetReward()
		{
			SetReward((IdleNumber)0);
		}
		public void SetReward(IdleNumber step)
		{
			rewardInfos = new List<RewardInfo>();
			for (int i = 0; i < rawData.rewards.Count; i++)
			{
				RewardInfo rewardInfo = new RewardInfo(rawData.rewards[i]);
				rewardInfo.UpdateCount(step);
				rewardInfos.Add(rewardInfo);
			}
		}


		public void UpdateRewardCount(IdleNumber step)
		{

			for (int i = 0; i < rewardInfos.Count; i++)
			{
				RewardInfo rewardInfo = rewardInfos[i];
				rewardInfo.UpdateCount(step);
			}
		}

		public override void Load<T>(T _info)
		{
			if (_info == null)
			{
				return;
			}
			QuestInfo info = _info as QuestInfo;

			progressState = info.progressState;
			progressCount = info.progressCount;
			//if (progressState == QuestProgressState.COMPLETE || progressState == QuestProgressState.END)
			//{
			//	progressCount = goalCount.ToString();
			//}

			count = (IdleNumber)progressCount;

		}

		public void ActivateQuest()
		{
			if (progressState != QuestProgressState.NONE)
			{
				return;
			}
			progressState = QuestProgressState.ONPROGRESS;
		}

		public void OnGetReward(bool displayReward, bool showToast = false)
		{
			if (progressState != QuestProgressState.COMPLETE)
			{
				return;
			}

			if (Type == QuestType.REPEAT)
			{
				count = count % GoalCount;
				count.Turncate();
				progressCount = count.ToString();
				progressState = QuestProgressState.ONPROGRESS;
			}

			else
			{
				progressState = QuestProgressState.END;
			}

			PlatformManager.UserDB.AddRewards(rewardInfos, displayReward, showToast);
			PlatformManager.UserDB.questContainer.OnQuestUpdate();
		}


		public void UpdateAccumulateQuest()
		{

			switch (GoalType)
			{
				case QuestGoalType.ACTIVATE_BUFF:
					{
						var item = PlatformManager.UserDB.inventory.GetPersistent(InventoryContainer.AdFreeTid);
						bool free = item.unlock;
						if (free)
						{
							OnChange(GoalType, GoalTid, (IdleNumber)1, true);
						}
					}
					break;
				case QuestGoalType.ABILITY_LEVEL:
					{
						var item = PlatformManager.UserDB.training.Find(GoalTid);
						OnChange(GoalType, GoalTid, (IdleNumber)item.Level, true);
					}
					break;
				case QuestGoalType.USER_LEVEL:
					{
						var item = PlatformManager.UserDB.userInfoContainer.userInfo;
						OnChange(GoalType, GoalTid, (IdleNumber)item.UserLevel, true);
					}
					break;
				case QuestGoalType.TOTAL_PLAY_TIME:
					{
						var item = PlatformManager.UserDB.userInfoContainer.userInfo;
						OnChange(GoalType, GoalTid, (IdleNumber)(item.PlayTicks / 1000 / 60), true);
					}
					break;
				case QuestGoalType.PLAY_TIME:
					{
						var item = PlatformManager.UserDB.userInfoContainer.userInfo;
						OnChange(GoalType, GoalTid, (IdleNumber)(item.DailyPlayTicks / 1000 / 60), true);
					}
					break;
				case QuestGoalType.STAGE_CLEAR:
					{
						var item = PlatformManager.UserDB.stageContainer.LastPlayedNormalStage();
						OnChange(GoalType, GoalTid, (IdleNumber)item.StageNumber, true);
					}
					break;
				case QuestGoalType.LEVELUP_AWAKENING:
					{
						var item = PlatformManager.UserDB.awakeningContainer.InfoList;
						int index = 0;
						for (int i = 0; i < item.Count; i++)
						{
							if (item[i].IsAwaken == false)
							{
								break;
							}
							index = i + 1;
						}
						OnChange(GoalType, GoalTid, (IdleNumber)index, true);
					}
					break;

			}
		}
		public void OnChange(QuestGoalType _goalType, long _goalTid, IdleNumber _progress, bool overwrite = false)
		{

			switch (rawData.questType)
			{
				case QuestType.REPEAT:
					if (progressState != QuestProgressState.ACTIVE && progressState != QuestProgressState.ONPROGRESS && progressState != QuestProgressState.COMPLETE)
					{
						return;
					}
					break;
				default:
					if (progressState != QuestProgressState.ACTIVE && progressState != QuestProgressState.ONPROGRESS)
					{
						return;
					}
					if (count >= goalCount)
					{
						progressState = QuestProgressState.COMPLETE;
						return;
					}

					break;
			}

			if (rawData.goalType != _goalType)
			{
				return;
			}

			if (GoalTid > 0)
			{
				if (GoalTid != _goalTid)
				{
					return;
				}
			}

			if (overwrite)
			{
				count = _progress;
			}
			else
			{
				count += _progress;
			}

			count.Turncate();
			progressCount = count.ToString();
			if (count >= goalCount)
			{
				count.Turncate();
				progressCount = count.ToString();
				progressState = QuestProgressState.COMPLETE;
			}
			else
			{
				progressState = QuestProgressState.ONPROGRESS;
			}

			if (rawData.questType == QuestType.REPEAT)
			{
				int step = (int)Mathf.Max(0, (count / GoalCount) - 1);
				UpdateRewardCount((IdleNumber)step);
			}

		}
	}
}
