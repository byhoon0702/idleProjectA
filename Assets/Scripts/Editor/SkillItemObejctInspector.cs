using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Purchasing.MiniJSON;

[CustomEditor(typeof(SkillItemObject))]
public class SkillItemObjectInspector : Editor
{
	private SkillDataSheet dataSheet;
	SkillItemObject skill;
	private void OnEnable()
	{
		skill = target as SkillItemObject;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		var property = serializedObject.GetIterator();
		if (property.NextVisible(true))
		{
			do
			{
				if (property.name == "m_Script")
				{
					GUI.enabled = false;
					EditorGUILayout.PropertyField(property);
					GUI.enabled = true;
					continue;
				}

				//if (property.name == "skill")
				//{

				//	EditorGUILayout.BeginHorizontal();
				//	EditorGUILayout.LabelField("Skill", GUILayout.Width(100));
				//	//EditorGUI.BeginDisabledGroup(true);

				//	property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(SkillObject), false);
				//	//EditorGUI.EndDisabledGroup();
				//	//if (GUILayout.Button("click", GUILayout.MinWidth(100)))
				//	//{
				//	//	EditorGUIUtility.ShowObjectPicker<SkillObject>(property.objectReferenceValue, false, "t:SkillObject", EditorGUIUtility.GetControlID(FocusType.Passive) + 100);
				//	//}
				//	//if (Event.current.commandName == "ObjectSelectorClose")
				//	//{
				//	//	currentObj = (SkillObject)EditorGUIUtility.GetObjectPickerObject();
				//	//}

				//	EditorGUILayout.EndHorizontal();
				//	continue;
				//}
				EditorGUILayout.PropertyField(property);
			}
			while (property.NextVisible(false));
		}

		if (GUILayout.Button("Save"))
		{
			SaveData();
		}
		if (GUILayout.Button("Load"))
		{
			LoadData();
		}

		serializedObject.ApplyModifiedProperties();
	}

	private bool LoadDataSheet()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("Data/Json/SkillItemDataSheet");

		string name = textAsset.name.Split('_')[0];

		System.Type t = name.GetAssemblyType();

		if (t == null)
		{
			return false;
		}
		var json = JsonUtility.FromJson(textAsset.text, t);
		dataSheet = (SkillDataSheet)json;
		return true;
	}
	private void LoadData()
	{
		if (LoadDataSheet() == false)
		{
			return;
		}

		var data = dataSheet.Get(skill.Tid);
		if (data != null)
		{
			if (data.detailData != null)
			{
				skill.SetBasicData(data);
				skill.SetUseAbility(data.useValue);

				EditorUtility.SetDirty(skill);
			}
		}
	}

	private void SaveData()
	{
		if (dataSheet == null)
		{
			if (LoadDataSheet() == false)
			{
				return;
			}
		}

		//for (int i = 0; i < dataSheet.infos.Count; i++)
		//{
		//	if (dataSheet.infos[i].tid == skill.Tid)
		//	{
		//		SkillData newData = new SkillData(skill);

		//		dataSheet.infos[i] = newData;
		//		break;
		//	}
		//}

		//var jsonFile = JsonUtility.ToJson(dataSheet);

		//string path = $"{Application.dataPath}/AssetFolder/Resources/Data/Json/SkillItemDataSheet.json";
		//System.IO.File.WriteAllText(path, jsonFile);
		//AssetDatabase.Refresh();
	}
}
