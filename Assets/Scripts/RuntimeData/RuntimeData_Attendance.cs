using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeData
{
	public enum DayStatus
	{
		DayClaimable,
		DayClaimed,
		DayUnClaimable,
	}

	[System.Serializable]
	public class DailyReward
	{
		public int day;
		public RuntimeData.RewardInfo reward;
		public DayStatus status;
		public DailyReward(int day, Reward reward, DayStatus status)
		{
			this.day = day;
			this.reward = new RewardInfo(reward);
			this.status = status;
		}

		public void UpdateStatus(int day)
		{
			if (this.day < day)
			{
				this.status = DayStatus.DayClaimed;
			}
			else if (this.day > day)
			{
				this.status = DayStatus.DayUnClaimable;
			}
			else
			{
				this.status = DayStatus.DayClaimable;
			}

		}

		public void GetReward()
		{
			if (status != DayStatus.DayClaimable)
			{
				return;
			}

			status = DayStatus.DayClaimed;
			PlatformManager.UserDB.AddRewards(new List<RewardInfo>() { reward }, true);
		}
	}

	[System.Serializable]
	public class AttendanceInfo : BaseInfo
	{
		[SerializeField] private int _attendanceDay;
		public int AttendanceDay => _attendanceDay;

		public List<DailyReward> DayRewardList { get; private set; } = new List<DailyReward>();
		public AttendanceData RawData { get; private set; }

		public bool canGetReward { get; private set; }

		public string rewardClaimTime;

		public System.DateTime RewardClaimDate { get; private set; }

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);

			AttendanceInfo temp = info as AttendanceInfo;
			_attendanceDay = temp._attendanceDay;

			rewardClaimTime = temp.rewardClaimTime;

			if (_attendanceDay == 0)
			{
				_attendanceDay = 1;
			}

			for (int i = 0; i < DayRewardList.Count; i++)
			{
				DayRewardList[i].UpdateStatus(_attendanceDay);
			}

			if (System.DateTime.TryParse(rewardClaimTime, out System.DateTime time))
			{
				RewardClaimDate = time;
			}
			else
			{
				canGetReward = true;
			}
		}

		public override void SetRawData<T>(T data)
		{
			RawData = data as AttendanceData;
			tid = RawData.tid;
			if (_attendanceDay == 0)
			{
				_attendanceDay = 1;
			}
			for (int i = 0; i < RawData.rewardList.Count; i++)
			{
				var reward = RawData.rewardList[i];

				DayRewardList.Add(new DailyReward(i + 1, reward, DayStatus.DayUnClaimable));
				DayRewardList[i].UpdateStatus(_attendanceDay);
			}
		}

		public override void UpdateData()
		{
			if ((RewardClaimDate - TimeManager.Instance.UtcNow).TotalSeconds <= 0)
			{
				Reset(RawData.limitType);
			}
		}

		public void GetReward()
		{
			if (canGetReward == false)
			{
				ToastUI.Instance.Enqueue("오늘은 더이상 받을 수 없어요");
				return;
			}

			for (int i = 0; i < DayRewardList.Count; i++)
			{
				if (DayRewardList[i].status == DayStatus.DayClaimable)
				{
					DayRewardList[i].GetReward();
					break;
				}
			}
			rewardClaimTime = TimeUtil.NextDayResetTimeToString();
			_attendanceDay++;
			canGetReward = false;
		}

		public DailyReward GetDaily(int day)
		{
			return DayRewardList.Find(x => x.day == day);
		}

		public bool AllRewardClaimed()
		{
			bool allClaimed = true;
			for (int i = 0; i < DayRewardList.Count; i++)
			{
				if (DayRewardList[i].status != DayStatus.DayClaimed)
				{
					allClaimed = false;
					break;
				}

			}
			return allClaimed;
		}

		public void Reset(TimeLimitType type)
		{
			canGetReward = true;
			if (RawData.limitType == TimeLimitType.USER)
			{
				if (AllRewardClaimed())
				{
					canGetReward = false;
				}

				return;
			}

			if (RawData.limitType == type)
			{
				if (AllRewardClaimed())
				{
					_attendanceDay = 1;
					for (int i = 0; i < DayRewardList.Count; i++)
					{
						DayRewardList[i].UpdateStatus(_attendanceDay);

					}
				}
			}
		}
	}
}


