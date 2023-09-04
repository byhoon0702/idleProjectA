using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(TutorialManager))]
public class TutorialManagerInspector : Editor
{

	TutorialManager manager;
	SerializedProperty scriptableObje;
	SerializedObject sso;
	private void OnEnable()
	{
		manager = target as TutorialManager;
		//scriptableObje = serializedObject.FindProperty("obj");
		//sso = new SerializedObject(scriptableObje.objectReferenceValue);
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		//var protper = sso.FindProperty("go");
		//if (protper != null)
		//{
		//	EditorGUILayout.PropertyField(protper);
		//}
	}
}
