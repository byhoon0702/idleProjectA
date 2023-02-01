using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(Companion), true)]
public class CompanionEditor : Editor
{
	private Companion unit;


	private GUIStyle labelStyle
	{
		get
		{
			GUIStyle style = new GUIStyle("Label");
			style.richText = true;
			style.wordWrap = true;

			return style;
		}
	}
	private void OnEnable()
	{
		unit = target as Companion;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (Application.isPlaying == false)
		{
			return;
		}

		EditorGUILayout.Space();
		if (GUILayout.Button("Extension"))
		{
			UnitEditorExtension.ShowEditor(unit);
		}

		try
		{
			EditorGUILayout.Space();
			UnitEditorUtility.ShowCharacterInfo(unit.info, new IdleNumber(), new IdleNumber());
		}
		catch (Exception e)
		{
			GUILayout.Label("<color=red>Invalid target</color>", labelStyle);
			GUILayout.Label($"<color=red>{e.Message}</color>", labelStyle);
		}
	}
}
