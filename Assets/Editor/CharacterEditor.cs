using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(Character), true)]
public class CharacterEditor : Editor
{
	private Character character;


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
		character = target as Character;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (Application.isPlaying == false)
		{
			return;
		}

		EditorGUILayout.Space();
		if (GUILayout.Button("Death"))
		{
			character.Hit(character, character.info.rawHp * 10, "Editor", Color.white, 1);
		}
		if (GUILayout.Button("Extension"))
		{
			CharacterEditorExtension.ShowEditor(character);
		}

		try
		{
			EditorGUILayout.Space();
			CharacterEditorExtension.ShowCharInfo(character);
			EditorGUILayout.Space();
			CharacterEditorExtension.ShowRecordData(character);
			EditorGUILayout.Space();
			CharacterEditorExtension.ShowConditionList(character);
		}
		catch (Exception e)
		{
			GUILayout.Label("<color=red>Invalid target</color>", labelStyle);
			GUILayout.Label($"<color=red>{e.Message}</color>", labelStyle);
		}
	}
}
