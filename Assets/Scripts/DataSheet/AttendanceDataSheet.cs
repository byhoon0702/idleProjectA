using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttendanceData : BaseData
{
	public List<Reward> rewardList;
	public TimeLimitType limitType;
}

[System.Serializable]
public class AttendanceDataSheet : DataSheetBase<AttendanceData>
{

}
