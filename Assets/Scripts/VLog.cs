using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VLog
{
	public static bool showConditionLog = false;
	public static bool showSkillLog = false;
	public static bool showPetLog = false;
	public static bool showItemLog = true;
	public static bool showScheduleLog = false;
	public static bool showAILog = false;
	public static bool showBattleLog = false;
	public static bool showSoundLog = false;


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

	public static void ConditionLog(object _message, Object _context = null)
	{
		if (showConditionLog)
		{
			Debug.Log($"[Condition] {_message}", _context);
		}
	}

	public static void SkillLog(object _message, Object _context = null)
	{
		if (showSkillLog)
		{
			Debug.Log($"[Skill] {_message}", _context);
		}
	}

	public static void SkillLogWarning(object _message, Object _context = null)
	{
		Debug.LogWarning($"[Skill] {_message}", _context);
	}

	public static void SkillLogError(object _message, Object _context = null)
	{
		Debug.LogError($"[Skill] {_message}", _context);
	}

	public static void ScheduleLog(object _message, Object _context = null)
	{
		if (showScheduleLog)
		{
			Debug.Log($"[Schedule] {_message}", _context);
		}
	}

	public static void ScheduleLogError(object _message, Object _context = null)
	{
		Debug.LogError($"[Schedule] {_message}", _context);
	}

	public static void AILog(object _message, Object _context = null)
	{
		if (showAILog)
		{
			Debug.Log($"[AI] {_message}", _context);
		}
	}

	public static void AILogWarning(object _message, Object _context = null)
	{
		Debug.LogWarning($"[AI] {_message}", _context);
	}

	public static void BattleRecordLog(object _message, Object _context = null)
	{
		if (showBattleLog)
		{
			Debug.Log($"[Record] {_message}", _context);
		}
	}
	public static void ItemLog(object _message, Object _context = null)
	{
		if (showItemLog)
		{
			Debug.Log($"[Item] {_message}", _context);
		}
	}

	public static void ItemLogError(object _message, Object _context = null)
	{
		Debug.LogError($"[Item] {_message}", _context);
	}

	public static void SoundLog(object _message, Object _context = null)
	{
		if (showSoundLog)
		{
			Debug.Log($"[Sound] {_message}", _context);
		}
	}

	public static void SoundLogError(object _message, Object _context = null)
	{
		Debug.LogError($"[Sound] {_message}", _context);
	}

	public static void PetLog(object _message, Object _context = null)
	{
		if (showPetLog)
		{
			Debug.Log($"[Pet] {_message}", _context);
		}
	}

	public static void PetLogWarning(object _message, Object _context = null)
	{
		Debug.LogWarning($"[Pet] {_message}", _context);
	}

	public static void PetLogError(object _message, Object _context = null)
	{
		Debug.LogError($"[Pet] {_message}", _context);
	}

}
