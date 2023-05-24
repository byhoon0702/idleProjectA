using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageMapObject))]
public class StageMapObjectInspector : Editor
{

	StageMapObject stageMap;
	private void OnEnable()
	{
		stageMap = target as StageMapObject;
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Save"))
		{
			Save();
		}
		if (GUILayout.Button("Load"))
		{
			Load();
		}
	}


	private void Save()
	{

	}

	private void Load()
	{

	}
}
