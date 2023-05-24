using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(Pet), true)]
public class PetEditor : Editor
{
	private Pet pet;


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
		pet = target as Pet;
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
			//UnitInfoEditorExtension.ShowEditor(pet);
		}

		try
		{
			EditorGUILayout.Space();
			//UnitEditorUtility.ShowUnitInfo(pet.info, new IdleNumber(), new IdleNumber());
		}
		catch (Exception e)
		{
			GUILayout.Label("<color=red>Invalid target</color>", labelStyle);
			GUILayout.Label($"<color=red>{e.Message}</color>", labelStyle);
		}
	}
}
