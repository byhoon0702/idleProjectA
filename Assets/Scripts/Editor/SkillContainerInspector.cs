using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;
using System.IO;
using Unity.VisualScripting;

[CustomEditor(typeof(SkillContainer))]
public class SkillContainerInspector : Editor
{
	SkillContainer skillContainer;
	private void OnEnable()
	{
		skillContainer = target as SkillContainer;
	}
	public override void OnInspectorGUI()
	{
		//bas
		//e.OnInspectorGUI();
		serializedObject.Update();

		GUI.enabled = false;
		EditorGUILayout.ObjectField("Script:", MonoScript.FromScriptableObject(skillContainer), typeof(SkillContainer), false);
		GUI.enabled = true;


		EditorGUILayout.Space(5);
		if (GUILayout.Button("Load Skill"))
		{
			UpdateSkillList("Assets/Resources/RuntimeDatas/Skills/Items");
			AssetDatabase.Refresh();
		}
		EditorGUILayout.LabelField("Skill");

		EditorGUILayout.PropertyField(serializedObject.FindProperty("skillList"));

		serializedObject.ApplyModifiedProperties();

	}

	private void UpdateSkillList(string path)
	{
		List<RuntimeData.SkillInfo> infoList = new List<RuntimeData.SkillInfo>();
		var guids = AssetDatabase.FindAssets("t:scriptableobject", new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = AssetDatabase.GUIDToAssetPath(guids[i]);

			RuntimeData.SkillInfo info = new RuntimeData.SkillInfo();

			NewSkill skillitemObject = (NewSkill)AssetDatabase.LoadAssetAtPath(assetpath, typeof(NewSkill));
			info.level = 0;
			info.count = 0;
			info.tid = skillitemObject.Tid;


			infoList.Add(info);
		}

		infoList.Sort((x, y) => { return x.tid.CompareTo(y.tid); });
		skillContainer.SetList(infoList);
		EditorUtility.SetDirty(target);
		AssetDatabase.SaveAssetIfDirty(target);

	}
}
