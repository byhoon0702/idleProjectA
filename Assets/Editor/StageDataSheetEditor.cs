using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.Data;

[CustomEditor(typeof(StageDataSheet))]
public class StageDataSheetEditor : Editor
{
	StageDataSheet dataSheet;
	SerializedProperty stageInfos;


	private void OnEnable()
	{
		dataSheet = target as StageDataSheet;

		stageInfos = serializedObject.FindProperty("stageInfos");
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField("데이터");
		serializedObject.Update();
		//base.OnInspectorGUI();

		EditorGUILayout.PropertyField(stageInfos);

		serializedObject.ApplyModifiedProperties();
	}

}
