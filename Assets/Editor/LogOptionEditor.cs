using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LogOptionEditor : EditorWindow
{
	public bool showConditionLog = false;
	public bool showSkillLog = false;
	public bool showPetLog = false;
	public bool showItemLog = true;
	public bool showScheduleLog = false;
	public bool showAILog = false;
	public bool showBattleLog = false;
	public bool showSoundLog = false;



	[MenuItem("Custom Menu/Log Option")]
	private static void InitEditor()
	{
		LogOptionEditor window = GetWindow<LogOptionEditor>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}

	private void OnGUI()
	{
		showConditionLog = GUILayout.Toggle(showConditionLog, "상태로그");
		showSkillLog = GUILayout.Toggle(showSkillLog, "스킬로그");
		showPetLog = GUILayout.Toggle(showPetLog, "펫로그");
		showItemLog = GUILayout.Toggle(showItemLog, "아이템로그");
		showScheduleLog = GUILayout.Toggle(showScheduleLog, "스케쥴로그");
		showAILog = GUILayout.Toggle(showAILog, "AI로그");
		showBattleLog = GUILayout.Toggle(showBattleLog, "전투로그");
		showSoundLog = GUILayout.Toggle(showSoundLog, "사운드로그");



		if (Application.isPlaying == false)
		{
			return;
		}

		VLog.showConditionLog = showConditionLog;
		VLog.showSkillLog = showSkillLog;
		VLog.showPetLog = showPetLog;
		VLog.showItemLog = showItemLog;
		VLog.showScheduleLog = showScheduleLog;
		VLog.showAILog = showAILog;
		VLog.showBattleLog = showBattleLog;
		VLog.showSoundLog = showSoundLog;
	}
}
