using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TimeEditor : EditorWindow
{
	int second;

	[MenuItem("Custom Menu/Time")]
	public static void ShowEditor()
	{
		TimeEditor window = GetWindow<TimeEditor>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}


	private void OnGUI()
	{
		if (Application.isPlaying == false)
		{
			return;
		}

		TimeScale();
		if (TimeManager.Instance == null)
		{
			return;
		}

		GUILayout.Label("Sync", "PreToolbar");
		second = EditorGUILayout.IntField("Minute", second);
		TimeManager.Instance.syncRelative = second * TimeSpan.TicksPerMinute;
		GUILayout.Label("UTC", "PreToolbar");
		GUILayout.Label(TimeManager.Instance.UtcNow.ToString());
		GUILayout.Label("Now", "PreToolbar");
		GUILayout.Label(TimeManager.Instance.Now.ToString());
		GUILayout.Label("Play Time", "PreToolbar");
		//GUILayout.Label(UserInfo.PlayTicksToString);
	}

	private void Update()
	{
		Repaint();
	}


	private void TimeScale()
	{
		EditorGUILayout.BeginHorizontal();
		Time.timeScale = EditorGUILayout.FloatField("Time Scale", Time.timeScale);
		EditorGUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("x0.1"))
		{
			Time.timeScale = 0.1f;
		}
		if (GUILayout.Button("x0.5"))
		{
			Time.timeScale = 0.5f;
		}
		if (GUILayout.Button("x1"))
		{
			Time.timeScale = 1;
		}
		if (GUILayout.Button("x2"))
		{
			Time.timeScale = 2;
		}
		if (GUILayout.Button("x5"))
		{
			Time.timeScale = 5;
		}
		if (GUILayout.Button("x100"))
		{
			Time.timeScale = 100;
		}
		GUILayout.EndHorizontal();
	}
}
