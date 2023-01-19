using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Malee.List;
using UnityEditor.PackageManager.UI;

public class SubListDataWindow : EditorWindow
{
	[SerializeField]
	SerializedProperty property;
	DataTableEditorSettings settings;
	[SerializeField]
	ReorderableList childReorderableList;

	public static SubListDataWindow CreateWindow()
	{
		var window = EditorWindow.GetWindow<SubListDataWindow>();

		window.Show();

		return window;
	}



	public void DrawChild(SerializedProperty _property, DataTableEditorSettings _settings)
	{
		if (childReorderableList == null)
		{
			property = _property;
			settings = _settings;
			CreateSubList();
		}

		childReorderableList.DoLayoutList();
	}

	public void CreateSubList()
	{
		Type rawDataType = System.Type.GetType($"{property.arrayElementType}, Assembly-CSharp");
		FieldInfo[] fields = rawDataType.GetFields();
		childReorderableList = new ReorderableList(property);

		childReorderableList.drawHeaderCallback += (rect, guicontent) =>
		{
			rect.x = 34;
			Rect tempRect = new Rect(rect);

			for (int i = 0; i < fields.Length; i++)
			{
				tempRect.width = settings.cellSize.x;
				tempRect.height = EditorGUIUtility.singleLineHeight;

				EditorGUI.LabelField(tempRect, fields[i].Name, EditorStyles.boldLabel);
				tempRect.x += tempRect.width + settings.rowSpace;
			}
		};

		childReorderableList.drawElementCallback += (rect, element, guiContent, isActive, isFocused) =>
		{

			Rect tempRect = new Rect(rect);
			SerializedProperty info = element;
			Type type = rawDataType;

			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				tempRect.y = rect.y;
				tempRect.width = settings.cellSize.x;
				tempRect.height = EditorGUIUtility.singleLineHeight;
				var sf = info.FindPropertyRelative(field.Name);

				EditorGUI.PropertyField(tempRect, sf, GUIContent.none);

				tempRect.x += tempRect.width + settings.rowSpace;

			}
		};
	}
}
