using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UserInfoEditor : EditorWindow
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


	[MenuItem("Custom Menu/User Info")]
	static void InitOptionEditor()
	{
		UserInfoEditor window = EditorWindow.GetWindow<UserInfoEditor>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}

	private void OnGUI()
	{
		if (Application.isPlaying == false)
		{
			return;
		}

		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Intro")
		{
			return;
		}


		GUILayout.BeginHorizontal();
		saveFileName = EditorGUILayout.TextField("FileName", saveFileName);
		if(GUILayout.Button("Save Current", GUILayout.MaxWidth(200)))
		{
			if (saveFileName.HasStringValue() == false)
			{
				EditorUtility.DisplayDialog("Info", "파일명 없음", "확인");
				return;
			}

			try
			{
				string json = JsonUtility.ToJson(UserInfo.userData, true);
				System.IO.File.WriteAllText($"{UserInfo.UserDataFilePath}/{saveFileName}.json", json);
			}
			catch(Exception e)
			{
				EditorUtility.DisplayDialog("Error", "저장실패. 콘솔 로그 확인", "확인");
				Debug.LogError("Save Error\n" + e);
			}


			EditorUtility.DisplayDialog("Info", "저장성공", "확인");
		}
		EditorGUILayout.EndHorizontal();

		highlight = EditorGUILayout.TextField("Highlight", highlight);

		string jsonText = JsonUtility.ToJson(UserInfo.userData, true);
		if (highlight.HasStringValue())
		{
			jsonText = jsonText.Replace(highlight, $"<color=yellow>{highlight}</color>");
		}

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		GUILayout.TextArea(jsonText, textAreaStyle);
		EditorGUILayout.EndScrollView();
	}
}
