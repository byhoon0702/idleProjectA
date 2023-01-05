using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VLog 
{
	public static void Log(object _message, Object _context = null)
	{
		Debug.Log(_message, _context);
	}

	public static void LogWarning(object _message, Object _context = null)
	{
		Debug.LogWarning(_message, _context);
	}

	public static void LogError(object _message, Object _context = null)
	{
		Debug.LogError(_message, _context);
	}

	/// <summary>
	/// 스킬로그
	/// </summary>
	public static void SkillLog(object _message, Object _context = null)
	{
		Debug.Log(_message, _context);
	}
	public static void SkillLogError(object _message, Object _context = null)
	{
		Debug.LogError(_message, _context);
	}

	public static void ScheduleLog(object _message, Object _context = null)
	{
		Debug.Log(_message, _context);
	}

	public static void ScheduleLogError(object _message, Object _context = null)
	{
		Debug.LogError(_message, _context);
	}

	public static void AILog(object _message, Object _context = null)
	{
		Debug.Log(_message, _context);
	}

	public static void AILogWarning(object _message, Object _context = null)
	{
		Debug.LogWarning(_message, _context);
	}
}
