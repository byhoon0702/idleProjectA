using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class SubListDataWindow : PopupWindowContent
{
	[SerializeField]
	private SerializedProperty property;
	private SerializedObject so;
	DataTableEditorSettings settings;
	[SerializeField]
	UnityEditorInternal.ReorderableList childReorderableList;

	Vector2 size;
	Vector2 scrollPos;

	ReorderableDataList owner;
	public void DrawChild(ReorderableDataList _owner, SerializedObject _so, SerializedProperty _property)
	{
		if (so == null)
		{
			this.so = _so;
		}
		owner = _owner;
		property = _property;
		settings = owner.settings;

	}
	public override Vector2 GetWindowSize()
	{
		return new Vector2(size.x == 0 ? 400 : size.x, size.y == 0 ? 300 : size.y);
	}

	public override void OnGUI(Rect rect)
	{
		so.Update();
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.BeginVertical(GUILayout.Width(size.x));
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		if (property != null)
		{
			if (property.isArray)
			{
				if (childReorderableList == null)
				{
					CreateSubList();
				}
				if (childReorderableList != null)
				{
					childReorderableList.DoLayoutList();
				}
			}
			else
			{
				EditorGUILayout.PropertyField(property);
			}

		}
		EditorGUILayout.LabelField("", GUILayout.Width(size.x));
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		if (EditorGUI.EndChangeCheck())
		{
			so.ApplyModifiedProperties();
		}
	}

	private List<long> tidList = new List<long>();
	private string[] nameArray = new string[1];
	private int selectedIndex = 0;
	private bool isLinkedField = false;
	private long prefixID;

	private ContainedData containedData;

	public void CreateSubList()
	{
		Type rawDataType = ConvertUtility.ConvertStringToType(property.arrayElementType);

		System.Type baseType = rawDataType;
		System.Type check = rawDataType;
		while (baseType != null)
		{
			check = baseType;
			baseType = check.BaseType;
		}

		bool isObjectType = check.Equals(typeof(System.Object));

		FieldInfo[] fields = rawDataType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		FieldInfo tidField = null;
		FieldInfo descriptionField = null;
		childReorderableList = new UnityEditorInternal.ReorderableList(so, property);

		if (isObjectType)
		{
			tidField = rawDataType.GetField("tid");
			descriptionField = rawDataType.GetField("description");
			var linkedType = owner.linkedTypeContainer.Find(property.name);
			if (linkedType == null)
			{
				isLinkedField = false;
			}
			else
			{

				containedData = owner.dataContainer.Find(linkedType.type);
				if (containedData != null)
				{
					isLinkedField = true;
					System.Type type = containedData.data.GetType();
					var value = type.GetField("infos").GetValue(containedData.data);
					prefixID = (long)type.GetField("prefixID", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(containedData.data);
					IList datas = (IList)value;
					System.Type cellType = datas[0].GetType();
					nameArray = new string[datas.Count];
					tidList = new List<long>();
					for (int i = 0; i < datas.Count; i++)
					{
						var data = datas[i];
						long tid = (long)cellType.GetField("tid").GetValue(data);
						string desc = (string)cellType.GetField("description").GetValue(data);


						long replacedTid = EditorHelper.ReplacePrefixTid(tid, prefixID);
						nameArray[i] = $"{replacedTid}:{desc}";
						tidList.Add(replacedTid);
					}
				}
			}
		}

		childReorderableList.drawHeaderCallback = (rect) =>
		{
			rect.x = 24;
			Rect tempRect = new Rect(rect);
			tempRect.y = rect.y;
			tempRect.width = settings.cellSize.x;
			tempRect.height = EditorGUIUtility.singleLineHeight;
			if (isObjectType)
			{
				if (tidField != null)
				{
					GUI.Label(tempRect, "tid", EditorStyles.boldLabel);
					tempRect.x += tempRect.width + settings.rowSpace;
				}
				if (descriptionField != null)
				{
					GUI.Label(tempRect, "description", EditorStyles.boldLabel);
					tempRect.x += tempRect.width + settings.rowSpace;
				}

				for (int i = 0; i < fields.Length; i++)
				{
					if (fields[i].Name == "tid" || fields[i].Name == "description")
					{
						continue;
					}
					GUI.Label(tempRect, fields[i].Name, EditorStyles.boldLabel);
					tempRect.x += tempRect.width + settings.rowSpace;
				}
			}
			else
			{
				tempRect.width = settings.cellSize.x;
				tempRect.height = EditorGUIUtility.singleLineHeight;
				GUI.Label(tempRect, property.displayName, EditorStyles.boldLabel);
			}

			size.x = tempRect.x + tempRect.width + settings.rowSpace;
			size.y = 300;
		};

		childReorderableList.onAddCallback = (rect) =>
		{

			int newIndex = property.arraySize;
			property.arraySize++;
			long lastTid = 0;


			int lastIndex = newIndex - 1 < 0 ? 0 : newIndex - 1;
			var laseElement = property.GetArrayElementAtIndex(lastIndex);
			SerializedProperty sp = laseElement.FindPropertyRelative("tid");
			if (sp != null)
			{
				if (newIndex - 1 < 0)
				{
					lastTid = 0;
					if (containedData != null)
					{
						System.Type type = containedData.data.GetType();
						var value = type.GetField("infos").GetValue(containedData.data);
						IList list = (IList)value;
						if (list.Count > 0)
						{
							System.Type datatype = list[0].GetType();
							FieldInfo linkedTidField = datatype.GetField("tid");
							if (linkedTidField != null)
							{
								lastTid = (long)linkedTidField.GetValue(list[0]);
							}
						}
					}
				}
				else
				{
					lastTid = sp.longValue + 1;
				}
			}

			var newElement = property.GetArrayElementAtIndex(newIndex);

			EditorHelper.AddNewElement(newElement, lastTid);

		};

		childReorderableList.drawElementCallback += (rect, index, isActive, isFocused) =>
		{
			Rect tempRect = new Rect(rect);
			tempRect.y = rect.y;
			tempRect.width = settings.cellSize.x;
			tempRect.height = EditorGUIUtility.singleLineHeight;
			var info = childReorderableList.serializedProperty.GetArrayElementAtIndex(index);
			Type type = rawDataType;
			if (isObjectType)
			{
				if (tidField != null)
				{
					var sf = info.FindPropertyRelative(tidField.Name);
					if (isLinkedField)
					{

						for (int i = 0; i < tidList.Count; i++)
						{
							if (sf.longValue == tidList[i])
							{
								selectedIndex = i;
								break;
							}
						}

						selectedIndex = EditorGUI.Popup(tempRect, selectedIndex, nameArray);
						sf.longValue = tidList[selectedIndex];
					}
					else
					{
						EditorGUI.PropertyField(tempRect, sf, GUIContent.none);
					}

					tempRect.x += tempRect.width + settings.rowSpace;
				}
				if (descriptionField != null)
				{
					var sf = info.FindPropertyRelative(descriptionField.Name);
					EditorGUI.PropertyField(tempRect, sf, GUIContent.none);
					tempRect.x += tempRect.width + settings.rowSpace;
				}

				for (int i = 0; i < fields.Length; i++)
				{
					var field = fields[i];
					if (field.Name == "tid" || field.Name == "description")
					{
						continue;
					}

					var sf = info.FindPropertyRelative(field.Name);
					EditorGUI.PropertyField(tempRect, sf, GUIContent.none);

					tempRect.x += tempRect.width + settings.rowSpace;
				}
			}
			else
			{
				if (property.name.Contains("Tid", StringComparison.Ordinal))
				{
					string ss = property.name.Split("Tid")[0] + "DataSheet";
				}

				EditorGUI.PropertyField(tempRect, info, GUIContent.none);

				tempRect.x += tempRect.width + settings.rowSpace;
			}


		};
	}


}
