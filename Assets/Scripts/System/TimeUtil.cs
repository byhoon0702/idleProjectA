using System;

public static class TimeUtil
{
	public readonly static DateTime EPOCH;
	private static readonly DateTime m_MinTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	private static readonly DateTime m_MaxTime = new DateTime(9999, 12, 31, 0, 0, 0, DateTimeKind.Utc);

	public static DateTime MinTime { get { return m_MinTime; } }
	public static DateTime MaxTime { get { return m_MaxTime; } }

	public static double Second2Milisecond { get { return 1000; } }
	public static double Minute2Second { get { return 60; } }
	public static double Hour2Minute { get { return 60; } }
	public static double Day2Hour { get { return 24; } }
	public static double Week2Day { get { return 7; } }
	public static double Hour2Second { get { return Hour2Minute * Minute2Second; } }
	public static double Day2Second { get { return Day2Hour * Hour2Second; } }
	public static double Week2Second { get { return Week2Day * Day2Second; } }

	public readonly static string[] DateTimeFormat =
	{
			"yyyy-MM-dd"
			, "yyyy-MM-dd HH:mm"
			, "yyyy-MM-dd HH:mm:ss"
			, "yyyy-MM-dd HH:mm:ss.fff"
		};

	public static Int64 NowToTimeStamp()
	{
		return DateTimeToTimeStamp(TimeManager.Instance.UtcNow);
	}
	public static string NowToTimeStampString()
	{
		return TimeManager.Instance.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss");
	}

	public static string NowToString()
	{
		return DateTimeToString(TimeManager.Instance.UtcNow);
	}
	public static string YesterdayToString()
	{
		return DateTimeToString(TimeManager.Instance.UtcNow.AddDays(-1));
	}

	public static Int64 DateTimeToTimeStamp(DateTime utc)
	{
		TimeSpan elapsedTime = utc - EPOCH;
		return Convert.ToInt64(elapsedTime.TotalMilliseconds);
	}

	public static string MinTimeToString()
	{
		return m_MinTime.ToString("yyyy-MM-dd HH:mm:ss");
	}
	public static string MaxTimeToString()
	{
		return m_MaxTime.ToString("yyyy-MM-dd HH:mm:ss");
	}

	public static DateTime TimeStampToDateTime(Int64 timestamp)
	{
		return EPOCH.AddMilliseconds(timestamp);
	}

	public static string UtcNowString()
	{
		return TimeManager.Instance.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
	}
	public static string UtcDayKey()
	{
		return TimeManager.Instance.UtcNow.ToString("yyMMdd");
	}
	public static string DateDayKey(DateTime dt)
	{
		return dt.ToString("yyMMdd");
	}
	public static string DateDayKey(string dt)
	{
		return StringToDateTime(dt).ToString("yyMMdd");
	}

	public static string DateTimeToString(DateTime dt)
	{
		return dt.ToString("yyyy-MM-dd HH:mm:ss");
	}
	public static string DateTimeToStringMilisec(DateTime dt)
	{
		return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
	}

	public static string DateTimeToString(DateTime dt, String format)
	{
		return dt.ToString(format);
	}

	public static string ToStringFormat(String dt, String format)
	{
		return StringToDateTime(dt).ToString(format);
	}

	public static DateTime StringToDateTime(string datetime_string)
	{
		DateTime date_time = DateTime.MinValue;

		if (datetime_string != "" && datetime_string != "0" && datetime_string != null)
		{
			if (false == DateTime.TryParse(datetime_string, out date_time))
			{
				//gplat.Log.logger("util.time").ErrorFormat("fail string to datetime, format : {0}", datetime_string);
			}

		}

		return date_time;
	}


	public static long ToUtcTicks(string datetime_string)
	{
		DateTime date_time = DateTime.MinValue;

		if (datetime_string != "" && datetime_string != "0" && datetime_string != null)
		{
			if (false == DateTime.TryParse(datetime_string, out date_time))
			{
				//gplat.Log.logger("util.time").ErrorFormat("fail string to datetime, format : {0}", datetime_string);
			}
		}

		return date_time.ToUniversalTime().Ticks;
	}

	public static long ToTicks(string datetime_string)
	{
		DateTime date_time = DateTime.MinValue;

		if (datetime_string != "" && datetime_string != "0" && datetime_string != null)
		{
			if (false == DateTime.TryParse(datetime_string, out date_time))
			{
				//gplat.Log.logger("util.time").ErrorFormat("fail string to datetime, format : {0}", datetime_string);
			}
		}

		return date_time.Ticks;
	}

	public static string ToUtcTimeString(string datetime_string)
	{
		DateTime date_time = DateTime.MinValue;

		if (datetime_string != "" && datetime_string != "0" && datetime_string != null)
		{
			if (false == DateTime.TryParse(datetime_string, out date_time))
			{
				//gplat.Log.logger("util.time").ErrorFormat("fail string to datetime, format : {0}", datetime_string);
			}
		}

		return date_time.ToUniversalTime().ToString();
	}

	public static string UtcToLocalTimeString(string datetime_string)
	{
		return UtcTicksToLocalTimeString(ToTicks(datetime_string));
	}

	public static string UtcTicksToLocalTimeString(long utc_ticks)
	{
		var date = new DateTime(utc_ticks, DateTimeKind.Utc).ToLocalTime();
		return DateTimeToString(date);
	}

	public static string UtcTicksToTimeString(long utc_ticks)
	{
		var date = new DateTime(utc_ticks, DateTimeKind.Utc);
		return DateTimeToString(date);
	}


	public static string ChangeStringFormat(string time_string)
	{
		return DateTimeToString(DateTime.Parse(time_string));
	}

	public static bool IsBetweenTime(DateTime startUtc, DateTime endUtc)
	{
		DateTime utcNow = TimeManager.Instance.UtcNow;
		if (startUtc <= utcNow && endUtc >= utcNow)
		{
			return true;
		}

		return false;
	}

	public static bool IsBetweenTime(DateTime targetUtc, DateTime startUtc, DateTime endUtc)
	{
		if (startUtc <= targetUtc && endUtc >= targetUtc)
		{
			return true;
		}

		return false;
	}

	public static bool IsBetweenTime(Int64 start, Int64 end)
	{
		Int64 utcNow = DateTimeToTimeStamp(TimeManager.Instance.UtcNow);
		if (start <= utcNow && utcNow >= end)
		{
			return true;
		}

		return false;
	}

	public static bool IsBetweenTime(string start_at, string end_at)
	{
		DateTime utcNow = TimeManager.Instance.UtcNow;
		var start = StringToDateTime(start_at);
		var end = StringToDateTime(end_at);

		if (start <= utcNow && end >= utcNow)
		{
			return true;
		}

		return false;
	}

	// 메일의 만료일자를 가져온다.
	public static string GetMailExpiredAt(Int64 expiredDay = 7)
	{
		DateTime utcNow = TimeManager.Instance.UtcNow;
		utcNow = utcNow.AddDays(expiredDay);

		return DateTimeToString(utcNow);
	}

	public static DateTime getTodayDateAt()
	{
		return TodayResetUtcTime().Date;
	}

	public static DateTime getTomorrowDateAt()
	{
		DateTime utc_now = getTodayDateAt();
		utc_now = utc_now.AddDays(1);

		return utc_now;
	}

	public static Int32 getPastDays(DateTime at)
	{
		DateTime utc_now = getTodayDateAt();

		return (utc_now - at).Days;
	}

	public static bool isNullDate(DateTime at)
	{
		if (at == DateTime.MinValue)
		{
			return true;
		}

		return false;
	}

	public static Int64 getLeftMilliSec(DateTime at)
	{
		DateTime utc_now = TimeManager.Instance.UtcNow;

		return (Int64)(at - utc_now).TotalMilliseconds;
	}

	public static Int64 getDayLeftMillisec()
	{
		return getLeftMilliSec(getTomorrowDateAt());
	}

	/// <summary>
	/// 입력 받은시간의 day_of_week을 구함, 시간은 정각
	/// </summary>
	/// <param name="date_time"></param>
	/// <param name="day_of_week"></param>
	/// <returns></returns>
	public static DateTime thisDayOfWeek(DateTime date_time, DayOfWeek day_of_week)
	{
		return date_time.AddDays(Convert.ToInt32(day_of_week) - Convert.ToInt32(date_time.DayOfWeek)).Date;
	}

	public static DateTime nextDayOfWeek(DateTime start_time, DayOfWeek need_day_of_week)
	{
		const Int32 ONE_WEEK = 7;

		var add_day = (need_day_of_week - start_time.DayOfWeek + ONE_WEEK) % 7;

		if (add_day == 0)
		{
			add_day = ONE_WEEK;
		}

		return start_time.AddDays(add_day);
	}

	public static DateTime nextDayOfWeek(DayOfWeek need_day_of_week)
	{
		return nextDayOfWeek(TimeManager.Instance.UtcNow, need_day_of_week);
	}

	//public static gplat_define.day_of_week_e getDayOfWeek(DateTime in_time)
	//{
	//	switch (in_time.DayOfWeek)
	//	{
	//		case System.DayOfWeek.Sunday:
	//			return gplat_define.day_of_week_e.Sunday;
	//		case System.DayOfWeek.Monday:
	//			return gplat_define.day_of_week_e.Monday;
	//		case System.DayOfWeek.Tuesday:
	//			return gplat_define.day_of_week_e.Tuesday;
	//		case System.DayOfWeek.Wednesday:
	//			return gplat_define.day_of_week_e.Wednesday;
	//		case System.DayOfWeek.Thursday:
	//			return gplat_define.day_of_week_e.Thursday;
	//		case System.DayOfWeek.Friday:
	//			return gplat_define.day_of_week_e.Friday;
	//		case System.DayOfWeek.Saturday:
	//			return gplat_define.day_of_week_e.Saturday;
	//	}
	//	return gplat_define.day_of_week_e._NONE;
	//}

	public static bool isToday(DateTime utcTime)
	{
		if (IsBetweenTime(utcTime, TodayResetUtcTime(), NextDayResetUtcTime()))
		{
			return true;
		}

		return false;
	}

	public static bool isToday(string utcTime)
	{
		DateTime dateTime = StringToDateTime(utcTime);
		return isToday(dateTime);
	}


	// Reset시간을 기준으로 날짜를 비교해서 다른 날짜인지 체크한다.
	public static bool isOverDay(string utcTime, int resetHour, int resetMin)
	{
		var day_reset = NextDayResetUtcTime(resetHour, resetMin);
		int day_reset_key = day_reset.Year * 10000 + day_reset.Month * 100 + day_reset.Day;

		int target_date_key = 0;
		DateTime target_time = StringToDateTime(utcTime);
		if (target_time.Hour >= resetHour)
		{
			target_time.AddDays(1);
		}
		target_date_key = target_time.Year * 10000 + target_time.Month * 100 + target_time.Day;

		return (target_date_key != day_reset_key);
	}
	// Reset시간을 기준으로 날짜를 비교해서 다른 날짜인지 체크한다.
	public static bool isOverDay(DateTime utcTime, int resetHour, int resetMin)
	{
		var day_reset = NextDayResetUtcTime(resetHour, resetMin);
		int day_reset_key = day_reset.Year * 10000 + day_reset.Month * 100 + day_reset.Day;

		int target_date_key = 0;
		DateTime target_time = utcTime;
		if (target_time.Hour >= resetHour)
		{
			target_time.AddDays(1);
		}
		target_date_key = target_time.Year * 10000 + target_time.Month * 100 + target_time.Day;

		return (target_date_key != day_reset_key);
	}


	public static string StringToUtcTimeString(string format)
	{
		var local_time = StringToDateTime(format);
		return DateTimeToString(local_time.ToUniversalTime());
	}

	public static DateTime StringToUtcDateTime(string format)
	{
		var local_time = StringToDateTime(format);
		return local_time.ToUniversalTime();
	}

	public static string DateTimeToUtcTimeString(DateTime datetime)
	{
		var local_time = datetime.ToUniversalTime();
		return DateTimeToString(local_time);
	}

	public static DateTime TodayResetTime()
	{
		return TodayResetUtcTime().ToLocalTime();
	}

	public static DateTime TodayResetUtcTime()
	{
		var Now = TimeManager.Instance.UtcNow;

		//서버 시간에 관계없이 UTC 기준 20시로 맞춘다(한국시간으론 오전 5시)
		//20시를 기점으로 '오늘' 기준의 리셋타임을 가져온다
		if (Now.Hour < 20)
		{
			return new DateTime(Now.Year, Now.Month, Now.Day, 20, 0, 0, DateTimeKind.Utc).AddDays(-1);
		}
		else
		{
			return new DateTime(Now.Year, Now.Month, Now.Day, 20, 0, 0, DateTimeKind.Utc);
		}
	}


	// 20201230 serment7
	// 현재 시간을 기준으로 임의로 정한 다음 리셋시간을 구한다.
	public static DateTime NextDayResetUtcTime(int hour, int minute)
	{
		var Now = TimeManager.Instance.UtcNow;
		var target = new DateTime(Now.Year, Now.Month, Now.Day, hour, minute, 0, DateTimeKind.Utc);

		if (target < Now)
		{
			return target.AddDays(1);
		}
		else
		{
			return target;
		}
	}
	public static DateTime NextDayResetUtcTime(DateTime target_date, int hour, int minute)
	{
		var target = new DateTime(target_date.Year, target_date.Month, target_date.Day, hour, minute, 0, DateTimeKind.Utc);
		return target;
	}

	// UTC 기준, 해당 날짜 이후의 리셋시간을 구한다.
	public static DateTime GetResetUtcTime(int day_after, int hour, int minute)
	{
		var Now = TimeManager.Instance.UtcNow;
		var target = new DateTime(Now.Year, Now.Month, Now.Day, hour, minute, 0, DateTimeKind.Utc);

		if (target < Now)
		{
			return target.AddDays(1 + day_after);
		}
		else
		{
			return target.AddDays(day_after);
		}
	}

	// 날짜에 해당하는 Key를 생성한다.
	// YYYYMMDD
	public static int GetResetKey(DateTime in_date)
	{
		int day_key = in_date.Year * 10000 + in_date.Month * 100 + in_date.Day;
		return day_key;
	}
	public static int GetResetKey(string in_date)
	{
		return GetResetKey(StringToDateTime(in_date));
	}
	public static int GetResetKey(int day_after, int hour, int minute)
	{
		return GetResetKey(GetResetUtcTime(day_after, hour, minute));
	}


	public static string NextDayResetUtcTimeToString(int hour, int minute)
	{
		return DateTimeToString(NextDayResetUtcTime(hour, minute));
	}

	public static DateTime NextDayResetTime()
	{
		var utcResetTime = TodayResetUtcTime();
		utcResetTime = utcResetTime.AddDays(1);
		return utcResetTime;
	}

	public static string NextDayResetTimeToString()
	{
		return DateTimeToString(NextDayResetTime());
	}

	public static string NextDayResetUtcTimeToString()
	{
		return DateTimeToString(NextDayResetUtcTime());
	}

	public static DateTime NextDayResetUtcTime()
	{
		var utcResetTime = TodayResetUtcTime();
		utcResetTime = utcResetTime.AddDays(1);
		return utcResetTime;
	}

	public static string NextWeeklyResetUtcTimeToString()
	{
		return DateTimeToString(weeklyResetTime());
	}
	public static string NextMonthlyResetUtcTimeToString()
	{
		return DateTimeToString(monthlyNextResetTime());
	}

	public static string dateTimeToUtcTime(DateTime format)
	{
		return DateTimeToString(format.ToUniversalTime());
	}

	public static DateTime weeklyResetTime()
	{
		var nextMonday = TodayResetUtcTime();
		nextMonday = nextMonday.AddDays(Convert.ToInt32(DayOfWeek.Sunday) - Convert.ToInt32(nextMonday.DayOfWeek));

		return nextMonday.AddDays(7).ToUniversalTime();
	}
	//public DateTime weeklyResetTime(DayOfWeek in_day_of_week, int hour, int minute)
	//{
	//	var nextMonday = NextDayResetUtcTime(hour, minute);
	//	nextMonday = nextMonday.AddDays(Convert.ToInt32(in_day_of_week) - Convert.ToInt32(nextMonday.DayOfWeek));

	//	return nextMonday.AddDays(7).ToUniversalTime();
	//}
	public static DateTime weeklyResetTime(DayOfWeek in_day_of_week, int week_count, int hour, int minute)
	{
		var nextMonday = NextDayResetUtcTime(hour, minute);
		nextMonday = nextMonday.AddDays(Convert.ToInt32(in_day_of_week) - Convert.ToInt32(nextMonday.DayOfWeek));
		int add_day = week_count * 7;
		return nextMonday.AddDays(add_day).ToUniversalTime();
	}

	// 30일 단위 리셋
	public static DateTime monthlyResetTime()
	{
		var nextMonday = NextDayResetUtcTime();
		nextMonday = nextMonday.AddDays(-nextMonday.Day);

		return nextMonday;
	}

	// 30일 단위 리셋
	public static DateTime monthlyNextResetTime()
	{
		var nextMonday = NextDayResetUtcTime();
		nextMonday = nextMonday.AddMonths(1);
		nextMonday = nextMonday.AddDays(-nextMonday.Day);

		return nextMonday;
	}
	public static DateTime monthlyNextResetTime(int hour, int minute)
	{
		var nextMonday = NextDayResetUtcTime(hour, minute);
		nextMonday = nextMonday.AddMonths(1);
		nextMonday = nextMonday.AddDays(-nextMonday.Day);

		return nextMonday;
	}

	// 특정시간이 지났는지 검사한다.
	// n초 일찍 요청온 것은 통과시켜준다.
	public static bool isPassedTime(DateTime limitTime)
	{
		// 5초 일찍 요청온건 통과
		if ((limitTime - TimeManager.Instance.UtcNow).TotalSeconds < 6)
		{
			return true;
		}

		return false;
	}

	public static long PassedTimeSeconds(DateTime in_time)
	{
		return Convert.ToInt64((TimeManager.Instance.UtcNow - in_time).TotalSeconds);
	}
	public static long PassedTimeSeconds(DateTime from_time, DateTime to_time)
	{
		return Convert.ToInt64((to_time - from_time).TotalSeconds);
	}

	public static bool isBetween(string in_start_time, string in_end_time)
	{
		DateTime start_time = DateTime.MinValue;
		DateTime end_time = DateTime.MaxValue;
		if (string.IsNullOrEmpty(in_start_time) == false)
		{
			if (in_start_time != "0")
			{
				start_time = TimeUtil.StringToDateTime(in_start_time);
			}
		}
		if (string.IsNullOrEmpty(in_end_time) == false)
		{
			if (in_end_time != "0")
			{
				end_time = TimeUtil.StringToDateTime(in_end_time);
			}
		}

		return IsBetweenTime(start_time, end_time);
	}

	public static bool IsValidDate(string in_datetime_string)
	{
		if (string.IsNullOrEmpty(in_datetime_string))
		{
			return true;
		}

		DateTime parseDateTime = DateTime.MinValue;
		if (false == DateTime.TryParseExact(in_datetime_string, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parseDateTime))
		{
			return false;
		}
		return true;
	}

	public static DateTime GetDateTimeByMilliseconds(long date)
	{
		DateTime resultTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double)date);
		return resultTime;
	}

	public static string GetDateTimeByMillisecondsToString(long date)
	{
		return DateTimeToString(GetDateTimeByMilliseconds(date));
	}
}
