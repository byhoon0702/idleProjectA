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


		GUILayout.BeginHorizontal();
		loadUserData = EditorGUILayout.TextField("Load Data(File Name)", loadUserData);
		if (GUILayout.Button("Apply", GUILayout.MaxWidth(60)))
		{
			PlayerPrefs.SetString("LoadUserInfoFileName", loadUserData);
		}
		GUILayout.EndHorizontal();

		string prefsSave = PlayerPrefs.GetString("LoadUserInfoFileName");

		GUILayout.Space(20);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button($"Play({prefsSave})", GUILayout.MinHeight(40), GUILayout.ExpandWidth(true)))
		{
			Play();
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(20);
		GUILayout.Label("Load Scene", "PreToolbar");
		if (GUILayout.Button($"Intro", GUILayout.MinHeight(40), GUILayout.ExpandWidth(true)))
		{
			EditorSceneManager.OpenScene("Assets/Scenes/Intro.unity");
		}
		if (GUILayout.Button($"Oldman", GUILayout.MinHeight(40), GUILayout.ExpandWidth(true)))
		{
			EditorSceneManager.OpenScene("Assets/Scenes/Oldman.unity");
		}
		if (GUILayout.Button($"UnitEditor", GUILayout.MinHeight(40), GUILayout.ExpandWidth(true)))
		{
			EditorSceneManager.OpenScene("Assets/Scenes/UnitEditor.unity");
		}
	}

	public void Play()
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
	}
}
