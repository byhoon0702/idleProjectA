using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;



public class SkillDataEditor : EditorWindow
{
	/// <summary>
	/// 게임 내 사용중인 데이터
	/// </summary>
	public SerializedObject instanceSO;

	/// <summary>
	/// 메타에서 불러온 데이터
	/// </summary>
	public SerializedObject metaSO;

	public string skillKey;
	public Int64 tid;

	public string skillFileFullPath => SkillMeta.filePath + skillKey + ".json";


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
		window.instanceSO = new SerializedObject(_skillData);

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

		if (instanceSO == null)
		{
			Debug.LogError("Data Invalid..");
			Close();
			return;
		}
		if (metaSO == null)
		{
			ReloadMetaData();
		}

		if (GUILayout.Button($"파일 열기({skillKey})"))
		{
			Application.OpenURL(skillFileFullPath);
		}


		GUILayout.Label(new GUIContent($"{tid} - {skillKey}"), "PreToolbar");
		ShowButtonMenu();
		GUILayout.Space(5);


		instanceSO.Update();
		metaSO.Update();
		var fields = instanceSO.targetObject.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		foreach (var field in fields)
		{
			foreach (var attribute in field.CustomAttributes)
			{
				if (attribute.AttributeType == typeof(TooltipAttribute))
				{
					string tooltipText = attribute.ConstructorArguments[0].Value.ToString();
					string propertyText = field.Name;

					GUILayout.Label($"{tooltipText}", labelStyle);

					SerializedProperty prop = instanceSO.FindProperty(propertyText);

					float originWidth = EditorGUIUtility.labelWidth;
					EditorGUIUtility.labelWidth = 300;


					GUILayout.BeginHorizontal();
					EditorGUILayout.PropertyField(prop);

					bool compareJson = false;

					var instanceValue = field.GetValue(instanceSO.targetObject);
					var metaObject = field.GetValue(metaSO.targetObject);
					if(metaObject.GetType() == typeof(string) ||
						metaObject.GetType() == typeof(int) ||
						metaObject.GetType() == typeof(float) ||
						metaObject.GetType() == typeof(long) ||
						metaObject.GetType() == typeof(double))
					{
						compareJson = false;
					}
					else
					{
						compareJson = true;
					}

					string finalMetaText = "";

					if(compareJson)
					{
						string instanceToJson = JsonUtility.ToJson(instanceValue);
						string metaToJson = JsonUtility.ToJson(metaObject);

						if (instanceToJson == metaToJson)
						{
							finalMetaText = $"  Meta: {metaToJson}";
						}
						else
						{
							finalMetaText = $"  <color=yellow>Meta: {metaToJson}</color>";
						}
					}
					else
					{
						if (instanceValue.ToString() == metaObject.ToString())
						{
							finalMetaText = $"  Meta: {metaObject}";
						}
						else
						{
							finalMetaText = $"  <color=yellow>Meta: {metaObject}</color>";
						}
					}
					GUILayout.Label(finalMetaText, labelStyle);
					GUILayout.EndHorizontal();


					EditorGUIUtility.labelWidth = originWidth;
					GUILayout.Space(10);
				}
			}
		}
		instanceSO.ApplyModifiedProperties();

		GUILayout.Label("Json Data", "PreToolbar");
		EditorGUI.BeginDisabledGroup(true);
		GUILayout.TextArea(JsonUtility.ToJson(instanceSO.targetObject, true));
		EditorGUI.EndDisabledGroup();
	}

	private void ShowButtonMenu()
	{
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Save"))
		{
			var json = JsonUtility.ToJson(instanceSO.targetObject, true);
			File.WriteAllText(skillFileFullPath, json);
			AssetDatabase.Refresh();

			ReloadMetaData();
		}
		if (GUILayout.Button("Load"))
		{
			var json = File.ReadAllText(skillFileFullPath);
			JsonUtility.FromJsonOverwrite(json, instanceSO.targetObject);

			ReloadMetaData();
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(10);
	}

	private void ReloadMetaData()
	{
		metaSO = new SerializedObject(CreateInstance($"{skillKey}"));

		if(File.Exists(skillFileFullPath))
		{
			var json = File.ReadAllText(skillFileFullPath);
			JsonUtility.FromJsonOverwrite(json, metaSO.targetObject);
		}
	}
}
