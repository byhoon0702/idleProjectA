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

	public string preset;
	public Int64 tid;

	private Vector2 scrollPos;
	public string skillFileFullPath => SkillMeta.jsonFilePath + tid + ".json";


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
		SkillDataEditor window = CreateInstance<SkillDataEditor>();
		window.titleContent = new GUIContent(window.ToString());
		window.instanceSO = new SerializedObject(_skillData);

		window.preset = _skillData.skillPreset;
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

		if (GUILayout.Button($"파일 열기({tid}({preset}))"))
		{
			Application.OpenURL(skillFileFullPath);
		}


		GUILayout.Label(new GUIContent($"{tid} - {preset}"), "PreToolbar");
		ShowButtonMenu();
		GUILayout.Space(5);


		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

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
					if(metaObject == null)
					{
						compareJson = false;
					}
					else if(metaObject.GetType() == typeof(string) ||
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

					if(metaObject == null) // 새로 추가된 데이터고 json에 없으면 여기로 들어온다
					{
						finalMetaText = "Meta: NULL";
					}
					else if(compareJson)
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

				if(attribute.AttributeType == typeof(FourArithmeticAttribute))
				{
					if(GUILayout.Button("사칙연산 테스트", GUILayout.MaxWidth(200)))
					{
						var instanceValue = field.GetValue(instanceSO.targetObject);
						FourArithmeticTester.ShowEditor(instanceValue.ToString());
					}
					GUILayout.Space(20);
				}
			}
		}
		instanceSO.ApplyModifiedProperties();

		GUILayout.Label("Json Data", "PreToolbar");
		EditorGUI.BeginDisabledGroup(true);
		GUILayout.TextArea(JsonUtility.ToJson(instanceSO.targetObject, true));
		EditorGUI.EndDisabledGroup();

		EditorGUILayout.EndScrollView();
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
		metaSO = new SerializedObject(CreateInstance($"{preset}"));

		if(File.Exists(skillFileFullPath))
		{
			var json = File.ReadAllText(skillFileFullPath);
			JsonUtility.FromJsonOverwrite(json, metaSO.targetObject);
		}
	}
}
