using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI
	;

[CustomEditor(typeof(UIEconomyButton))]
public class UIEconomyButtonInspector : SelectableEditor
{
	private UIEconomyButton _button;

	SerializedProperty _labelProperty;
	SerializedProperty _iconProperty;
	SerializedProperty _valueProperty;
	protected override void OnEnable()
	{
		base.OnEnable();
		_button = (UIEconomyButton)target;
		_labelProperty = serializedObject.FindProperty("_textmeshLabel");
		_iconProperty = serializedObject.FindProperty("_imageEconomy");
		_valueProperty = serializedObject.FindProperty("_textmeshEconomy");

	}
	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		serializedObject.Update();

		EditorGUILayout.PropertyField(_labelProperty);
		EditorGUILayout.PropertyField(_iconProperty);
		EditorGUILayout.PropertyField(_valueProperty);
		serializedObject.ApplyModifiedProperties();
		base.OnInspectorGUI();

		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(_button);
		}

	}
}
