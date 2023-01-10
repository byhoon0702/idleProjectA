using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;



public class SkillDataEditor : EditorWindow
{
	/// <summary>
	/// 게임 내 사용중인 데이터
	/// </summary>
	public SerializedObject instanceSkillData;

	/// <summary>
	/// 메타에서 불러온 데이터
	/// </summary>
	public SerializedObject metaSkillData;

	public string skillKey;
	public Int64 tid;



	private GUIStyle labelStyle
	{
		get
		{
			GUIStyle style = new GUIStyle("Label");
			style.richText = true;

			return style;
		}
	}

	public static void ShowEditor(SkillBaseData _skillData)
	{
		SkillDataEditor window = ScriptableObject.CreateInstance<SkillDataEditor>();
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 600, 300);
		window.titleContent = new GUIContent(window.ToString());
		window.instanceSkillData = new SerializedObject(_skillData);

		window.skillKey = _skillData.key;
		window.tid = _skillData.tid;

		window.Show();
	}

	private void OnGUI()
	{
		if (Application.isPlaying == false)
		{
			Close();
			return;
		}
		if(instanceSkillData == null)
		{
			Debug.LogError("Data Invalid..");
			Close();
			return;
		}

		GUILayout.Label(new GUIContent($"{tid} - {skillKey}"), "PreToolbar");
		GUILayout.Space(5);


		instanceSkillData.Update();
		var fields = instanceSkillData.targetObject.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		foreach (var field in fields)
		{
			foreach (var attribute in field.CustomAttributes)
			{
				if (attribute.AttributeType == typeof(TooltipAttribute))
				{
					string tooltipText = attribute.ConstructorArguments[0].Value.ToString();
					string propertyText = field.Name;

					GUILayout.Label($"{tooltipText}", labelStyle);

					SerializedProperty prop = instanceSkillData.FindProperty(propertyText);

					float originWidth = EditorGUIUtility.labelWidth;
					EditorGUIUtility.labelWidth = 300;

					EditorGUILayout.PropertyField(prop);

					EditorGUIUtility.labelWidth = originWidth;
					GUILayout.Space(10);
				}
			}
		}
		instanceSkillData.ApplyModifiedProperties();

		GUILayout.Label("Json Data", "PreToolbar");
		EditorGUI.BeginDisabledGroup(true);
		GUILayout.TextArea(JsonUtility.ToJson(instanceSkillData.targetObject, true));
		EditorGUI.EndDisabledGroup();
	}
}
