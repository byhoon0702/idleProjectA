using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIItemDailyReward : MonoBehaviour
{

	[SerializeField] private GameObject objClaim;
	[SerializeField] private UIItemReward uiItemReward;
	[SerializeField] private TextMeshProUGUI textDate;
	[SerializeField] private GameObject objCheck;

	public System.Action onClick;
	private int _dayIndex = 0;

	RuntimeData.DailyReward _info;
	UIAttendancePage _page;
	public void SetData(UIAttendancePage page, int day, RuntimeData.DailyReward info)
	{
		_page = page;
		_info = info;

		_dayIndex = day;
		textDate.text = $"{day} 일차";
		uiItemReward.Set(new AddItemInfo(_info.reward));
	}

	public void UpdateStatus(RuntimeData.AttendanceInfo attendance)
	{
		objCheck.SetActive(false);
		objClaim.SetActive(false);

		var dayStatus = attendance.GetDaily(_dayIndex);
		if (dayStatus.status == RuntimeData.DayStatus.DayClaimable)
		{
			objClaim.SetActive(true && attendance.canGetReward);
		}
		if (dayStatus.status == RuntimeData.DayStatus.DayClaimed)
		{

			objCheck.SetActive(true);
		}
	}

	public void OnClick()
	{
		_page.GetReward();
		_page.Refresh();
	}
}
