using System.Collections;
using System.Collections.Generic;
using DG.DemiEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class SkillObjectEditorWindow : EditorWindow
{
	private SkillObject selected;
	private ParticleSystem[] particles;

	private GameObject particleToCopy;
	private int copyCount;


	#region Property
	private string skillName;

	#endregion

	bool isPrefab;
	SerializedObject serializedObject;


	[MenuItem("Tools/SkillObjectEditorWindow")]
	public static void CreateWindow()
	{
		var window = EditorWindow.GetWindow<SkillObjectEditorWindow>();
		window.Show();
	}
	Vector2 scrollpos;
	void Clear()
	{

		selected = null;
		particles = null;
		particleToCopy = null;
		copyCount = 0;
		serializedObject = null;
		isPrefab = false;
		//scriptable = null;
	}

	private void OnGUI()
	{
		if (Selection.activeGameObject != null)
		{
			selected = Selection.activeGameObject.GetComponent<SkillObject>();
		}
		else
		{
			Clear();
		}

		if (selected == null)
		{
			EditorGUILayout.HelpBox("SkillObject 가 아닙니다.", MessageType.Warning);
			return;
		}

		isPrefab = IsPrefabInstance(selected.gameObject);




		GUI.enabled = false;
		EditorGUILayout.Toggle("프리팹 여부", isPrefab);

		GUI.enabled = true;

		if (isPrefab == false)
		{
			skillName = EditorGUILayout.TextField("이름", skillName);
			if (GUILayout.Button("스킬 프리팹 생성"))
			{
				CreateNewPrefab();
			}

			return;
		}
		else
		{
			if (GUILayout.Button("프리팹 편집"))
			{
				OpenPrefab();
			}
		}
		if (serializedObject == null)
		{
			serializedObject = new SerializedObject(selected);
		}
		serializedObject.Update();
		EditorGUILayout.LabelField("오브젝트 이름", selected.name);
		DrawParticleProperty();
		DrawSkillObjectProperty();
		serializedObject.ApplyModifiedProperties();
		if (GUILayout.Button("스킬 프리팹 저장"))
		{
			//CreateNewPrefab();
		}
	}

	void DrawParticleProperty()
	{
		EditorGUILayout.Space(5);
		GUIStyle style = new GUIStyle();
		style.fontStyle = FontStyle.Bold;
		style.fontSize = 20;
		style.normal.textColor = Color.white;
		EditorGUILayout.LabelField("파티클 프로퍼티", style);
		EditorGUILayout.Space(2);
		particleToCopy = (GameObject)EditorGUILayout.ObjectField("파티클 이펙트", particleToCopy, typeof(GameObject), false);
		if (particleToCopy != null)
		{
			copyCount = EditorGUILayout.IntField("복사할 파티클수", copyCount);
			if (GUILayout.Button("복사"))
			{

			}
		}

		var particles = selected.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < particles.Length; i++)
		{

		}


	}



	void DrawSkillObjectProperty()
	{
		EditorGUILayout.Space(5);
		GUIStyle style = new GUIStyle();
		style.fontStyle = FontStyle.Bold;
		style.fontSize = 20;
		style.normal.textColor = Color.white;
		EditorGUILayout.LabelField("SkillObject 프로퍼티", style);
		EditorGUILayout.Space(2);
		serializedObject.Update();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("movement"));

		SerializedProperty skillAbility = serializedObject.FindProperty("skillAbility");
		EditorGUILayout.PropertyField(skillAbility);
		if (skillAbility != null)
		{
			var scriptable = new SerializedObject(skillAbility.objectReferenceValue);

			var property = scriptable.GetIterator();
			EditorGUILayout.BeginVertical("window");
			scrollpos = EditorGUILayout.BeginScrollView(scrollpos);
			EditorGUI.indentLevel++;
			if (property.NextVisible(true))
			{
				do
				{
					if (property.name == "m_Script")
					{
						continue;
					}
					EditorGUILayout.PropertyField(property);
				}
				while (property.NextVisible(false));
			}

			scriptable.Dispose();
			EditorGUI.indentLevel--;
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		serializedObject.ApplyModifiedProperties();
	}

	//SerializedObject scriptable;
	private void Update()
	{
		Repaint();
	}
	void CreateNewPrefab()
	{
		GameObject newObject = new GameObject(skillName);
		newObject.AddComponent<SkillObject>();

		string path = "SkillObjects".Resources().Assets();
		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder(path.Parent(), path.FileOrDirectoryName());
		}
		string prefabPath = $"{path}/{skillName}.prefab";

		PrefabUtility.SaveAsPrefabAssetAndConnect(newObject, prefabPath, InteractionMode.AutomatedAction);

		UnityEditor.SceneManagement.PrefabStageUtility.OpenPrefab(prefabPath);

		DestroyImmediate(newObject);
	}
	void OpenPrefab()
	{
		string path = AssetDatabase.GetAssetPath(selected);

		UnityEditor.SceneManagement.PrefabStageUtility.OpenPrefab(path);
	}

	bool IsPrefabInstance(GameObject instance)
	{
		GameObject original = PrefabUtility.GetCorrespondingObjectFromOriginalSource(instance);


		if (original == null)
		{
			return false;
		}

		return true;
	}
}
