using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 일정 시간마다 충전되는 포인트 계산
/// </summary>
public class TimeRefill
{
	// refill interval minute (1 hour = 60, 1 day = 1440)
	private int intervalMin = 0;
	// refill limit
	private int limit = 0;
	// increase count per interval
	private int perPoint = 0;
	// 마지막 갱신시간
	private DateTime nextRefillDate = TimeManager.it.server_utc;

	// 계산 시점의 시간 (설정하면 계산시점을 정할수 있고, 하지 않으면 UTCNow로 계산된다.)
	private DateTime computeAt = TimeUtil.MinTime;





	#region getter_setter
	public int Interval { get { return intervalMin; } }
	public int Limit { get { return limit; } }
	public DateTime NextRefillDate { get { return nextRefillDate; } }
	public DateTime ComputeAt
	{
		get { return (computeAt == TimeUtil.MinTime) ? TimeManager.it.server_utc : computeAt; }
		set { computeAt = value; }
	}
	#endregion getter_setter



	/// <summary>
	/// 초기화
	/// </summary>
	public void Initialize(string _date, int _intervalMin, int _perPoint = 1, int _limit = 1)
	{
		nextRefillDate = TimeUtil.StringToDateTime(_date);
		intervalMin = _intervalMin;
		perPoint = _perPoint;
		limit = _limit;
	}

	/// <summary>
	/// refill 포인트가 있는지 확인한다.
	/// </summary>
	public bool ProcessUpdate()
	{
		if (ComputeAt < nextRefillDate)
		{
			return false;
		}
		return true;
	}

	/// <summary>
	/// 지금 시간부터 다음 Refill시간을 계산한다.
	/// </summary>
	public DateTime RefillTimeFromNow()
	{
		return TimeManager.it.server_utc.AddMinutes(intervalMin);
	}

	public TimePoint CalcRefillPoint(int _adjustPoint)
	{
		// 외부 요인으로 포인트가 변경되었을 경우 변경후 계산
		int currentPoint = _adjustPoint;
		var timePoint = new TimePoint();

		timePoint.updateType = sc_define.point_update_type_e.TIME_REFILL;
		// Next refill 시점을 기반으로 계산 및 저장
		int elapsed_minutes = (Int32)(ComputeAt - nextRefillDate).TotalMinutes + intervalMin;
		int calc_point = elapsed_minutes / intervalMin;
		timePoint.prevDate = nextRefillDate;
		timePoint.nextDate = nextRefillDate.AddMinutes(calc_point * intervalMin);
		int gain_total_point = Math.Min(currentPoint + calc_point * perPoint, limit);     // limit 값을 넘을 수 없다.
		timePoint.point = Math.Max(gain_total_point - currentPoint, 0);       // 음수가 되지 않는다.

		return timePoint;
	}


	/// <summary>
	/// 데이터 갱신 - Calc 이후 Commit Update
	/// </summary>
	public void CommitData(DateTime _updateDate)
	{
		nextRefillDate = _updateDate;
	}

}
public class TimePoint
{
	public sc_define.point_update_type_e updateType;
	public DateTime prevDate;
	public DateTime nextDate;
	public int point;

	public override string ToString()
	{
		return string.Format($"{updateType}, {point}, Next:{nextDate} - Prev:{prevDate}");
	}
}

public enum RefillType
{
	_NONE,
	Refill,
	Reset,
	Both,
	_END
}

public class RefillResult
{
	public RefillType type = RefillType.Refill;
	public long itemTid;
	public int addCount;
	public string nextRefill;
	public string nextReset;

	public RefillResult(long _itemTid, int _addCount, DateTime _nextUpdate)
	{
		itemTid = _itemTid;
		addCount = _addCount;
		nextRefill = TimeUtil.DateTimeToString(_nextUpdate);
	}
	public RefillResult(RefillType _type, long _itemTid, int _addCount, DateTime _nextReset)
	{
		type = _type;
		itemTid = _itemTid;
		addCount = _addCount;
		nextReset = TimeUtil.DateTimeToString(_nextReset);
	}
	public RefillResult(RefillType _type, long _itemTid, int _addCount, string _nextRefill, DateTime _nextReset)
	{
		type = _type;
		itemTid = _itemTid;
		addCount = _addCount;
		nextRefill = _nextRefill;
		nextReset = TimeUtil.DateTimeToString(_nextReset);
	}
}
