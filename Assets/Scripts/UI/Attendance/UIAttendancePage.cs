using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIAttendancePage : MonoBehaviour
{
	protected UIPopupAttendance _parent;

	public abstract void SetData(UIPopupAttendance parent);

	public abstract void GetReward();

	public abstract void Refresh();


}
