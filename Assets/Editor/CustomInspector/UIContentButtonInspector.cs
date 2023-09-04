using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(UIContentButton))]
public class UIContentButtonInspector : ButtonEditor
{
	UIContentButton button = null;
	SerializedProperty contentTypeProperty = null;
	SerializedProperty redDotProperty = null;
	protected override void OnEnable()
	{
		base.OnEnable();
		button = (UIContentButton)target;
		contentTypeProperty = serializedObject.FindProperty("contentType");
		redDotProperty = serializedObject.FindProperty("redDot");
	}
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(contentTypeProperty);
		EditorGUILayout.PropertyField(redDotProperty);
		serializedObject.ApplyModifiedProperties();
		base.OnInspectorGUI();

		EditorUtility.SetDirty(button);



	}
}
