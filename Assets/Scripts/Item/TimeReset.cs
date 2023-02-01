using System;


public class TimeReset
{
	// 다음 리셋 시간
	public DateTime m_next_reset = TimeManager.it.server_utc;

	/// <summary>
	/// 초기화
	/// </summary>
	public void Initialize(string _date)
	{
		m_next_reset = TimeUtil.StringToDateTime(_date);
	}

	/// <summary>
	/// Update 해야하는지 확인
	/// </summary>
	/// <returns></returns>
	public bool ProcessUpdate()
	{
		if (TimeManager.it.server_utc < m_next_reset)
		{
			return false;
		}
		return true;
	}

	public TimePoint CalcNextReset(int _hasPoint, int _resetPoint, int _resetHour = -1, int _resetMin = -1)
	{
		DateTime nextReset;
		if (_resetHour < 0)
		{
			nextReset = TimeUtil.TodayResetUtcTime();       // UTC 20시 기준
		}
		else
		{
			nextReset = TimeUtil.NextDayResetUtcTime(_resetHour, _resetMin);
		}

		// 보유량을 확인해서 Reset 수치보다 크면 추가하지 않는다.
		int add_point = Math.Max(_resetPoint - _hasPoint, 0);

		return new TimePoint()
		{
			updateType = sc_define.point_update_type_e.TIME_RESET,
			point = add_point,
			nextDate = nextReset,
			prevDate = m_next_reset
		};
	}

	/// <summary>
	/// 데이터 갱신
	/// </summary>
	public void CommitData(DateTime _updateDate)
	{
		m_next_reset = _updateDate;
	}
}
