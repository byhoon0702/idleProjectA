using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


public class SceneChangeWindow : EditorWindow
{
	public string loadUserData;

	[MenuItem("Custom Menu/SceneChanger %w")]
	public static void ShowWindow()
	{
		SceneChangeWindow window = EditorWindow.GetWindow<SceneChangeWindow>();
		window.titleContent = new GUIContent("Scene Changer");
		window.Show();
		window.Init();
	}

	public void Init()
	{
	}

	private void OnGUI()
	{
		if(GUILayout.Button($"{UserInfo.UserDataFilePath}"))
		{
			Application.OpenURL(UserInfo.UserDataFilePath);
		}

		GUILayout.BeginHorizontal();
		loadUserData = EditorGUILayout.TextField("Load Data(File Name)", loadUserData);
		if(GUILayout.Button("Apply", GUILayout.MaxWidth(60)))
		{
			PlayerPrefs.SetString("LoadUserInfoFileName", loadUserData);
		}
		GUILayout.EndHorizontal();

		string prefsSave = PlayerPrefs.GetString("LoadUserInfoFileName");

		GUILayout.Space(20);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button($"Play SampleScene({prefsSave})", GUILayout.MinHeight(40), GUILayout.ExpandWidth(true)))
		{
			Play("SampleScene");
		}
		if (GUILayout.Button($"Play Oldman({prefsSave})", GUILayout.MinHeight(40), GUILayout.ExpandWidth(true)))
		{
			Play("Oldman");
		}
		EditorGUILayout.EndHorizontal();
	}

	public void Play(string _sceneName)
	{
		if (EditorApplication.isPlaying)
		{
			return;
		}

		if (EditorSceneManager.GetActiveScene().isDirty)
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		}


		EditorSceneManager.OpenScene("Assets/Scenes/Intro.unity");
		EditorApplication.isPlaying = true;

		PlayerPrefs.SetString("GameSceneName", _sceneName);
	}
}
