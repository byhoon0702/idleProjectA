using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttendanceContainer : BaseContainer
{

	[SerializeField] private List<RuntimeData.AttendanceInfo> _attendanceList = new List<RuntimeData.AttendanceInfo>();
	public List<RuntimeData.AttendanceInfo> AttendanceList => _attendanceList;
	public override void Dispose()
	{

	}

	public override void FromJson(string json)
	{
		AttendanceContainer temp = CreateInstance<AttendanceContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);
		LoadListTidMatch(ref _attendanceList, temp._attendanceList);
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();
		SetListRawData(ref _attendanceList, DataManager.Get<AttendanceDataSheet>().GetInfosClone());
	}

	public override void LoadScriptableObject()
	{

	}

	public override void DailyResetData()
	{
		//for (int i = 0; i < _attendanceList.Count; i++)
		//{
		//	_attendanceList[i].Reset(TimeLimitType.DAILY);
		//}

	}

	public override string Save()
	{
		string json = JsonUtility.ToJson(this, true);
		return json;
	}

	public override void UpdateData()
	{
		for (int i = 0; i < _attendanceList.Count; i++)
		{
			_attendanceList[i].UpdateData();
		}
	}

	public void GetReward(RuntimeData.AttendanceInfo info)
	{
		info.GetReward();
	}
}
