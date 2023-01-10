using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LogOptionEditor : EditorWindow
{
	public bool showConditionLog = false;
	public bool showSkillLog = false;
	public bool showScheduleLog = false;
	public bool showAILog = false;
	public bool showBattleLog = false;



	[MenuItem("Custom Menu/Log Option")]
	private static void InitEditor()
	{
		LogOptionEditor window = ScriptableObject.CreateInstance<LogOptionEditor>();
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 600, 300);
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}

	private void OnGUI()
	{
		showConditionLog = GUILayout.Toggle(showConditionLog, "상태로그");
		showSkillLog = GUILayout.Toggle(showSkillLog, "스킬로그");
		showScheduleLog = GUILayout.Toggle(showScheduleLog, "스케쥴로그");
		showAILog = GUILayout.Toggle(showAILog, "AI로그");
		showBattleLog = GUILayout.Toggle(showBattleLog, "전투로그");



		if (Application.isPlaying == false)
		{
			return;
		}

		VLog.showConditionLog = showConditionLog;
		VLog.showSkillLog = showSkillLog;
		VLog.showScheduleLog = showScheduleLog;
		VLog.showAILog = showAILog;
		VLog.showBattleLog = showBattleLog;
	}
}
