using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Malee.List;
using Unity.VisualScripting;
using NUnit.Framework.Constraints;

public class ReorderableListGenerator
{
	private SerializedProperty property;
	private Dictionary<string, Type> linkedTypeList;
	private Dictionary<string, List<bool>> foldOutList;
	private Dictionary<Type, object> jsonContainer;
	private DataTableEditorSettings settings;
	private ReorderableList reorderableList;
	private SerializedObject serializedObject;
	private Dictionary<string, SubEditorWindow> childListWindows = new Dictionary<string, SubEditorWindow>();


	public static ReorderableListGenerator Init(DataTableEditorSettings _settings, SerializedObject _object, SerializedProperty _property)
	{
		ReorderableListGenerator gen = new ReorderableListGenerator();
		gen.serializedObject = _object;
		gen.settings = _settings;
		gen.property = _property;
		return gen;
	}
	public ReorderableListGenerator SetLoadedData(Dictionary<Type, object> _jsonContainer)
	{
		jsonContainer = _jsonContainer;
		return this;
	}


	public Malee.List.ReorderableList Build(int pageSize, bool paginate)
	{
		if (reorderableList != null)
		{
			reorderableList = null;
		}
		reorderableList = new ReorderableList(property);
		reorderableList.paginate = paginate;
		reorderableList.pageSize = pageSize;
		CreateMainList();
		return reorderableList;
	}
	public string TypeString(string name, string assembly = "Assembly-CSharp")
	{
		return $"{name}, {assembly}";
	}
	private void CreateMainList()
	{
		Type rawDataType = System.Type.GetType($"{property.arrayElementType}, Assembly-CSharp");
		FieldInfo[] fields = rawDataType.GetFields();

		linkedTypeList = null;
		linkedTypeList = new Dictionary<string, Type>();

		childListWindows = null;
		childListWindows = new Dictionary<string, SubEditorWindow>();
		for (int i = 0; i < fields.Length; i++)
		{
			if (fields[i].Name.Contains("Tid", StringComparison.Ordinal))
			{
				string typeName = fields[i].Name.Replace("Tid", "DataSheet").FirstCharacterToUpper();

				if (linkedTypeList.ContainsKey(typeName))
				{
					continue;
				}
				linkedTypeList.Add(fields[i].Name, System.Type.GetType(TypeString(typeName)));
			}
		}

		foldOutList = new Dictionary<string, List<bool>>();
		reorderableList.drawHeaderCallback += (rect, guicontent) =>
		{
			rect.x = 34;
			Rect tempRect = new Rect(rect);

			for (int i = 0; i < fields.Length; i++)
			{
				tempRect.width = settings.cellSize.x;
				tempRect.height = EditorGUIUtility.singleLineHeight;

				if (fields[i].Name == "tid")
				{
					tempRect.x = rect.x;
					EditorGUI.LabelField(tempRect, fields[i].Name, EditorStyles.boldLabel);
				}
				else if (fields[i].Name == "description")
				{
					tempRect.x = rect.x + (tempRect.width + settings.rowSpace);
					EditorGUI.LabelField(tempRect, fields[i].Name, EditorStyles.boldLabel);
				}
				else
				{
					EditorGUI.LabelField(new(tempRect.x + ((tempRect.width + settings.rowSpace) * 2), tempRect.y, tempRect.width, tempRect.height), fields[i].Name, EditorStyles.boldLabel);
				}
				tempRect.x += tempRect.width + settings.rowSpace;
			}
		};
		reorderableList.onSelectCallback += MaleeReorderableList_onSelectCallback;
		reorderableList.getElementHeightCallback += MaleeReorderableList_getElementHeightCallback;
		reorderableList.drawElementCallback += (rect, element, guiContent, isActive, isFocused) =>
		{

			Rect tempRect = new Rect(rect);
			SerializedProperty info = element;
			Type type = rawDataType;
			long tid = 0;
			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				tempRect.y = rect.y;
				tempRect.width = settings.cellSize.x;
				tempRect.height = EditorGUIUtility.singleLineHeight;
				var sf = info.FindPropertyRelative(field.Name);

				var tidField = info.FindPropertyRelative("tid");


				if (field.Name == "tid")
				{
					tempRect.x = rect.x;
					EditorGUI.PropertyField(tempRect, sf, GUIContent.none);
					tid = sf.longValue;
				}
				else if (field.Name == "description")
				{
					tempRect.x = rect.x + (tempRect.width + settings.rowSpace);
					EditorGUI.PropertyField(tempRect, sf, GUIContent.none);
				}
				else
				{
					Rect relativeRect = new Rect(tempRect.x + ((tempRect.width + settings.rowSpace) * 2), tempRect.y, tempRect.width, tempRect.height);

					if (linkedTypeList.ContainsKey(field.Name))
					{
						Type linkType = linkedTypeList[field.Name];
						if (linkType == null)
						{
							EditorGUI.PropertyField(relativeRect, sf, GUIContent.none);
							tempRect.x += tempRect.width + settings.rowSpace;
							continue;
						}
						if (jsonContainer.ContainsKey(linkType))
						{
							FieldInfo obj = jsonContainer[linkType].GetType().GetField("infos");
							object ff = obj.GetValue(jsonContainer[linkType]);


							if (typeof(IList).IsAssignableFrom(ff))
							{
								IList list = (IList)ff;
								GUIContent[] nameList = new GUIContent[list.Count];
								int[] idList = new int[list.Count];

								for (int ii = 0; ii < list.Count; ii++)
								{
									object item = list[ii];
									Type itemType = item.GetType();
									long id = (long)itemType.GetField("tid").GetValue(item);
									idList[ii] = (int)id;
									nameList[ii] = new GUIContent($"{id} :{(string)itemType.GetField("description").GetValue(item)}");
								}
								if (nameList.Length > 0)
								{
									EditorGUI.IntPopup(relativeRect, sf, nameList, idList, GUIContent.none);
									tempRect.x += tempRect.width + settings.rowSpace;
									continue;
								}
							}
						}
					}

					if (sf.propertyType.Equals(SerializedPropertyType.Generic))
					{
						if (GUI.Button(relativeRect, "Edit"))
						{
							CreateSubWindow(tidField.longValue, sf, settings);
						}
					}
					else
					{
						EditorGUI.PropertyField(relativeRect, sf, GUIContent.none);
					}

					tempRect.x += tempRect.width + settings.rowSpace;
				}
			}
		};
	}

	private void CreateSubWindow(long tid, SerializedProperty property, DataTableEditorSettings settings)
	{
		string windowName = $"{property.type}:{property.name}:{tid}";
		if (childListWindows.ContainsKey(windowName))
		{
			childListWindows[windowName].Build(serializedObject, property, settings, OnRemoveSubWindow);
			return;
		}

		var window = SubEditorWindow.Create(windowName).Build(serializedObject, property, settings, OnRemoveSubWindow);
		childListWindows.Add(windowName, window);
	}
	public void OnRemoveSubWindow(string name)
	{
		childListWindows.Remove(name);
	}

	private void MaleeReorderableList_onSelectCallback(Malee.List.ReorderableList list)
	{
		Debug.Log(list.canAdd);
	}

	private float MaleeReorderableList_getElementHeightCallback(SerializedProperty element)
	{
		int index = reorderableList.IndexOf(element);
		for (int i = 0; i < reorderableList.Selected.Length; i++)
		{
			if (index == reorderableList.Selected[i])
			{
				return EditorGUIUtility.singleLineHeight * 2;
			}
		}
		return EditorGUIUtility.singleLineHeight;
	}

}

public class SubEditorWindow : EditorWindow
{

	[SerializeField] SerializedObject serializedObject;
	[SerializeField] SerializedProperty property;
	[SerializeField] DataTableEditorSettings settings;
	[SerializeField] ReorderableList childReorderableList;
	private Action<string> onDestroy;

	public static SubEditorWindow Create(string windowName)
	{
		var window = EditorWindow.CreateWindow<SubEditorWindow>();
		window.titleContent = new GUIContent(windowName);
		return window;
	}
	public SubEditorWindow Build(SerializedObject _serializedObject, SerializedProperty _property, DataTableEditorSettings _settings, Action<string> _onDestroy)
	{
		onDestroy = _onDestroy;
		serializedObject = _serializedObject;
		property = _property;
		settings = _settings;
		childReorderableList = null;
		Show();

		return this;
	}

	private void OnDestroy()
	{
		onDestroy?.Invoke(titleContent.text);
	}
	void OnGUI()
	{
		if (serializedObject == null)
		{
			return;
		}
		serializedObject.Update();

		if (childReorderableList == null)
		{
			CreateSubList();
		}

		childReorderableList.DoLayoutList();
		EditorUtility.SetDirty(this);
		serializedObject.ApplyModifiedProperties();
	}

	private void CreateSubList()
	{
		bool isObjectType = false;
		string convertTypeName = ConvertUtility.ConvertStringToType(property.arrayElementType);
		Type rawDataType = System.Type.GetType($"{convertTypeName}, Assembly-CSharp");
		if (rawDataType == null)
		{
			rawDataType = System.Type.GetType($"{convertTypeName}");
		}


		if (rawDataType.BaseType.Equals(typeof(object)))
		{
			isObjectType = true;
		}


		FieldInfo[] fields = rawDataType.GetFields();

		childReorderableList = new ReorderableList(property);

		childReorderableList.drawHeaderCallback += (rect, guicontent) =>
		{
			rect.x = 34;
			Rect tempRect = new Rect(rect);
			if (isObjectType == false)
			{
				tempRect.width = settings.cellSize.x;
				tempRect.height = EditorGUIUtility.singleLineHeight;

				EditorGUI.LabelField(tempRect, property.displayName, EditorStyles.boldLabel);
				tempRect.x += tempRect.width + settings.rowSpace;
			}
			else
			{
				for (int i = 0; i < fields.Length; i++)
				{
					tempRect.width = settings.cellSize.x;
					tempRect.height = EditorGUIUtility.singleLineHeight;

					EditorGUI.LabelField(tempRect, fields[i].Name, EditorStyles.boldLabel);
					tempRect.x += tempRect.width + settings.rowSpace;
				}
			}
		};

		childReorderableList.drawElementCallback += (rect, element, guiContent, isActive, isFocused) =>
			{

				Rect tempRect = new Rect(rect);
				SerializedProperty info = element;
				Type type = rawDataType;

				if (isObjectType == false)
				{

					tempRect.y = rect.y;
					tempRect.width = settings.cellSize.x;
					tempRect.height = EditorGUIUtility.singleLineHeight;


					EditorGUI.PropertyField(tempRect, info, GUIContent.none);

					tempRect.x += tempRect.width + settings.rowSpace;

				}
				else
				{
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
				}


			};
	}


}
