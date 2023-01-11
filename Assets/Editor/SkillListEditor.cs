using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;



public class SkillListEditor : EditorWindow
{
	/// <summary>
	/// 게임내 사용중인 config
	/// </summary>
	public SerializedObject skillMetaScriptableObject;
	public bool raw;
	private Vector2 scrollPos;
	private string filter;

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


	[MenuItem("Custom Menu/Skill List")]
	public static void ShowEditor()
	{
		SkillListEditor window = ScriptableObject.CreateInstance<SkillListEditor>();
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 600, 300);
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}

	private void OnGUI()
	{
		if (GUILayout.Button($"파일 위치({SkillMeta.filePath})"))
		{
			Application.OpenURL(SkillMeta.filePath);
		}

		if (Application.isPlaying == false)
		{
			GUILayout.Label("플레이중이 아님");
			return;
		}

		// 초기화가 안된경우, 초기화 해줌
		if (skillMetaScriptableObject == null && GameManager.it.skillMeta != null)
		{
			skillMetaScriptableObject = new SerializedObject(GameManager.it.skillMeta);
		}

		raw = GUILayout.Toggle(raw, "Raw");
		filter = EditorGUILayout.TextField("Filter", filter);


		GUILayout.Space(10);
		GUILayout.Label("Skill List", "PreToolbar");

		scrollPos = GUILayout.BeginScrollView(scrollPos);
		foreach (var skill in SkillTidDictionary.dic)
		{
			var skillData = SkillMeta.it.dic[skill.Value];
			string titleText = $"[{skill.Key} - {skillData.skillName}({skill.Value})] : {skillData.description}";

			if (string.IsNullOrEmpty(filter) == false)
			{
				if (titleText.ToLower().Contains(filter.ToLower()) == false)
				{
					continue;
				}
			}

			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Edit", GUILayout.MaxWidth(40), GUILayout.MinWidth(40)))
			{
				SkillDataEditor.ShowEditor(skillData);
			}
			GUILayout.Label(titleText);
			GUILayout.EndHorizontal();

			if(raw)
			{
				GUILayout.Label(JsonUtility.ToJson(skillData), labelStyle);
			}
			GUILayout.Space(5);
		}
		GUILayout.EndScrollView();
	}
}
