using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAttendancePageDaily : UIAttendancePage
{

	[SerializeField] private UIItemDailyReward[] uiDailyRewards;

	private RuntimeData.AttendanceInfo _attendanceInfo;
	public override void GetReward()
	{
		PlatformManager.UserDB.attendanceContainer.GetReward(_attendanceInfo);
		Refresh();
	}

	public override void SetData(UIPopupAttendance parent)
	{
		_parent = parent;

		gameObject.SetActive(true);
		Init();
	}

	public void Reset()
	{

	}


	public override void Refresh()
	{
		Init();
	}
	public void Init()
	{
		var list = PlatformManager.UserDB.attendanceContainer.AttendanceList.Find(x => x.RawData.limitType == TimeLimitType.DAILY);
		_attendanceInfo = list;
		for (int i = 0; i < uiDailyRewards.Length; i++)
		{
			uiDailyRewards[i].SetData(this, i + 1, list.DayRewardList[i]);
			uiDailyRewards[i].UpdateStatus(list);
		}
	}
}
