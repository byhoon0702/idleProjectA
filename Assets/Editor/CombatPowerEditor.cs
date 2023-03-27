using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CombatPowerEditor : EditorWindow
{
	private GUIStyle textAreaStyle
	{
		get
		{
			GUIStyle style = new GUIStyle("TextArea");
			style.richText = true;
			style.wordWrap = true;

			return style;
		}
	}
	public Vector2 scrollPos;
	public string saveFileName;
	public string highlight;


	[MenuItem("Custom Menu/Combat Power Info")]
	static void ShowEditor()
	{
		CombatPowerEditor window = EditorWindow.GetWindow<CombatPowerEditor>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}

	private void OnGUI()
	{
		if (Application.isPlaying == false)
		{
			return;
		}

		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Oldman")
		{
			return;
		}

		scrollPos = GUILayout.BeginScrollView(scrollPos);
		GUILayout.Label($"전투력 총합: {UserInfo.totalCombatPower.ToString()}");
		GUILayout.Space(10);
		GUILayout.Label($"플레이어: {UserInfo.info.TotalCombatPower().ToString()}");
		GUILayout.Label($"진급능력: {UserInfo.coreAbil.TotalCombatPower().ToString()}");
		GUILayout.Label($"아이템: {Inventory.it.TotalCombatPower().ToString()}");
		foreach(ItemType type in Enum.GetValues(typeof(ItemType)))
		{
			GUILayout.Label($"   {type.ToString()} : {Inventory.it.abilityCalculator.GetCalculator(type).TotalCombatBower().ToString()}");
		}
		GUILayout.EndScrollView();
	}
}
