using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveDataCopyWindow : EditorWindow
{
	[MenuItem("Custom Menu/Save Data Copy")]
	public static void ShowWindow()
	{
		SaveDataCopyWindow window = EditorWindow.GetWindow<SaveDataCopyWindow>();
		window.titleContent = new GUIContent("Save Data Copy");
		window.Show();
	}


	public TextAsset original;
	public TextAsset copy;

	string userJson;
	string myJson;
	bool canChange;
	Vector2 scrollPos;
	Vector2 myScrollPos;
	private void OnEnable()
	{

	}

	private void OnGUI()
	{
		original = EditorGUILayout.ObjectField("복사할 유저 데이터", original, typeof(TextAsset), false) as TextAsset;
		copy = EditorGUILayout.ObjectField("내 데이터", copy, typeof(TextAsset), false) as TextAsset;

		if (GUILayout.Button("Json 변환"))
		{
			if (original != null)
			{
				Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
				jsonSerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
				userJson = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(original.text, jsonSerializerSettings);
				userJson = userJson.Replace("\n", "\\n");
				userJson = userJson.Replace("\\n", "\n");
				userJson = userJson.Replace("\\", "");
			}

			if (copy != null)
			{
				Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
				jsonSerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
				myJson = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(copy.text, jsonSerializerSettings);
				myJson = myJson.Replace("\n", "\\n");
				myJson = myJson.Replace("\\n", "\n");
				myJson = myJson.Replace("\\", "");
			}
			Repaint();
		}

		if (GUILayout.Button("Json 복사"))
		{
			UserDBSave save = new UserDBSave();
			UserDB userdb = new UserDB();

			//JsonUtility.ToJson
		}

		EditorGUILayout.Space();

		GUIStyle style = new GUIStyle(EditorStyles.textArea);
		style.richText = true;
		style.wordWrap = true;

		EditorGUILayout.LabelField("유저 데이터 Json", EditorStyles.toolbar);
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MaxHeight(300));
		userJson = EditorGUILayout.TextArea(userJson, style);
		EditorGUILayout.EndScrollView();

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("내 데이터 Json", EditorStyles.toolbar);
		myScrollPos = EditorGUILayout.BeginScrollView(myScrollPos, GUILayout.MaxHeight(300));
		myJson = EditorGUILayout.TextArea(myJson, style);
		EditorGUILayout.EndScrollView();
		//EditorGUILayout.LabelField("")


	}
}
