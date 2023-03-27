using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CostumeSkin))]
public class CostumeSkinInspector : Editor
{
	CostumeSkin costumeSkin;
	private void OnEnable()
	{
		costumeSkin = target as CostumeSkin;
	}
	public override void OnInspectorGUI()
	{
		//serializedObject.Update();
		EditorGUI.BeginChangeCheck();
		base.OnInspectorGUI();

		if (GUILayout.Button("Set"))
		{
			costumeSkin.Init();
		}
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(costumeSkin);
			AssetDatabase.SaveAssetIfDirty(costumeSkin);
		}
		//serializedObject.ApplyModifiedProperties();
	}
}
