using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	private static TimeManager instance;
	public static TimeManager it => instance;
	public Int64 syncRelative;


	public DateTime server_utc
	{
		get
		{
			return DateTime.UtcNow.AddTicks(syncRelative);
		}
	}
	public DateTime m_now
	{
		get
		{
			return DateTime.Now.AddTicks(syncRelative);
		}
	}


	private void Awake()
	{
		instance = this;
		syncRelative = 0;
	}

	public void SetServerUtc(DateTime in_server_utc)
	{
		syncRelative = in_server_utc.Ticks - DateTime.UtcNow.Ticks;
	}
}
