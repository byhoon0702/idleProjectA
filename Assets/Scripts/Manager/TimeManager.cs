using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	public static TimeManager Instance;
	public Int64 syncRelative;

	long currTicks = 0;
	public DateTime UtcNow
	{
		get
		{
			return DateTime.UtcNow.AddTicks(syncRelative);
		}
	}

	public DateTime Now
	{
		get
		{
			return DateTime.Now.AddTicks(syncRelative);
		}
	}

	public DateTime LocalServerTime;
	public DateTime UtcServerTime;


	public long prevPlayTicks;
	public string LastLoginTimeForOfflineReward;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			if (Instance.gameObject != null)
			{
				if (Instance.gameObject != gameObject)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				Instance = null;
				Instance = this;
			}
		}

		syncRelative = 0;

		prevPlayTicks = UtcNow.Ticks;
	}


	public void SetServerTime(DateTime in_server_utc)
	{
		UtcServerTime = in_server_utc;
		LocalServerTime = in_server_utc.ToLocalTime();
		syncRelative = in_server_utc.Ticks - DateTime.UtcNow.Ticks;
	}
}
