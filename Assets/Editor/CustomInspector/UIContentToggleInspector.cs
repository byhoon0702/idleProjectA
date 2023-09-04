using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIContentToggle))]
public class UIContentToggleInspector : ToggleEditor
{
	UIContentToggle button = null;
	SerializedProperty contentTypeProperty = null;
	SerializedProperty redDotProperty = null;
	SerializedProperty ignoreStatusProperty = null;
	protected override void OnEnable()
	{
		base.OnEnable();
		button = (UIContentToggle)target;
		contentTypeProperty = serializedObject.FindProperty("contentType");
		redDotProperty = serializedObject.FindProperty("redDot");
		ignoreStatusProperty = serializedObject.FindProperty("ignoreStatus");
	}
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(contentTypeProperty);
		EditorGUILayout.PropertyField(redDotProperty);
		EditorGUILayout.PropertyField(ignoreStatusProperty);
		serializedObject.ApplyModifiedProperties();
		base.OnInspectorGUI();
		EditorUtility.SetDirty(button);

	}
}
