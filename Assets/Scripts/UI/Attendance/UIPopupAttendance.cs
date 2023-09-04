using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupAttendance : UIBase
{
	[SerializeField] private Toggle _toggleDaily;
	[SerializeField] private Toggle _toggleNewUser;
	[SerializeField] private UIAttendancePageDaily uiAttendancePageDaily;
	[SerializeField] private UIAttendancePageNewUser uiAttendancePageNewUser;

	public void Open()
	{
		if (Activate() == false)
		{
			return;
		}
		gameObject.SetActive(true);
		_toggleDaily.SetIsOnWithoutNotify(true);
		_toggleNewUser.SetIsOnWithoutNotify(false);
		OnDailyToggleValueChanged(true);
		OnNewUserToggleValueChanged(false);

	}

	public void OnDailyToggleValueChanged(bool isOn)
	{
		if (isOn)
		{
			uiAttendancePageDaily.SetData(this);
		}
		else
		{
			uiAttendancePageDaily.gameObject.SetActive(false);
		}
	}
	public void OnNewUserToggleValueChanged(bool isOn)
	{
		if (isOn)
		{
			uiAttendancePageNewUser.SetData(this);
		}
		else
		{
			uiAttendancePageNewUser.gameObject.SetActive(false);
		}

	}
}
