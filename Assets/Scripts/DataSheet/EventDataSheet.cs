using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventData : BaseData
{
	public string season;
	public long eventBattleTid;
	public List<long> eventShopTid;
	public long currencyTid;
	public string startDate;
	public string endDate;
}

[System.Serializable]
public class EventDataSheet : DataSheetBase<EventData>
{

	public EventData GetBySeason(string season)
	{
		return infos.Find(e => e.season == season);
	}
}
