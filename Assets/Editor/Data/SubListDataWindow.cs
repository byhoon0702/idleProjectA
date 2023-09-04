using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

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

	int fieldCount;
	public void DrawChild(ReorderableDataList _owner, SerializedObject _so, SerializedProperty _property)
	{
		if (so == null)
		{
			this.so = _so;
		}
		owner = _owner;
		property = _property;
		settings = owner.settings;
		System.Type type = ConvertUtility.ConvertStringToType(property.type);
		if (type == null)
		{
			fieldCount = 1;
		}
		else
		{
			fieldCount = type.GetFields().Length;
		}
	}

	public override Vector2 GetWindowSize()
	{
		//return new Vector2(size.x == 0 ? 400 : size.x, size.y == 0 ? 300 : size.y);
		return new Vector2(650, 300);
	}

	public override void OnGUI(Rect rect)
	{
		so.Update();
		EditorGUI.BeginChangeCheck();
		Vector2 size = GetWindowSize();
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
		EditorGUILayout.LabelField("", GUILayout.Width((settings.cellSize.x + settings.columeSpace) * fieldCount));
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		if (EditorGUI.EndChangeCheck())
		{
			so.ApplyModifiedProperties();
		}
	}

	private List<long> tidList = new List<long>();
	private List<string> nameList = new List<string>();
	private int selectedIndex = 0;
	private bool isLinkedField = false;
	private long prefixID;

	private List<ContainedData> containedData;

	public void CreateSubList()
	{
		Type rawDataType = ConvertUtility.ConvertStringToType(property.arrayElementType);

		System.Type baseType = rawDataType;
		System.Type check = rawDataType;

		bool isObjectType = check.IsClass || check.IsStruct();

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
				tidList = new List<long>();
				nameList = new List<string>();

				for (int i = 0; i < linkedType.type.Count; i++)
				{
					containedData = owner.dataContainer.FindAll(linkedType.type[i]);

					for (int ii = 0; ii < containedData.Count; ii++)
					{
						isLinkedField = true;
						System.Type type = containedData[ii].data.GetType();
						var value = type.GetField("infos").GetValue(containedData[ii].data);
						prefixID = (long)type.GetField("prefixID", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(containedData[ii].data);
						IList datas = (IList)value;
						System.Type cellType = datas[0].GetType();


						for (int iii = 0; iii < datas.Count; iii++)
						{
							var data = datas[iii];
							long tid = (long)cellType.GetField("tid").GetValue(data);
							string desc = (string)cellType.GetField("description").GetValue(data);

							long replacedTid = EditorHelper.ReplacePrefixTid(tid, prefixID);
							nameList.Add($"{replacedTid}:{desc}");
							tidList.Add(replacedTid);
						}
					}
				}
			}
		}
		else
		{
			LinkTypeInfo linkedType = null;
			if (property.name.Contains("Tid", StringComparison.Ordinal))
			{
				string[] split = property.name.Split("Tid");
				string typeName = $"{split[0]}DataSheet".FirstCharacterToUpper();
				linkedType = owner.linkedTypeContainer.Find(property.name);
			}
			else
			{
				for (int i = 0; i < fields.Length; i++)
				{
					if (fields[i].Name.Contains("Tid", StringComparison.Ordinal))
					{
						string[] split = fields[i].Name.Split("Tid");
						string typeName = $"{split[0]}DataSheet".FirstCharacterToUpper();

						string typeString = ConvertUtility.GetAssemblyName(typeName);
						System.Type type = System.Type.GetType(typeString);
						if (type == null)
						{
							continue;
						}
						if (owner.linkedTypeContainer.Find(typeName) != null)
						{
							continue;
						}
						linkedType = owner.linkedTypeContainer.Find(property.name);
					}
				}
			}

			if (linkedType != null)
			{
				tidList = new List<long>();
				nameList = new List<string>();
				for (int i = 0; i < linkedType.type.Count; i++)
				{
					containedData = owner.dataContainer.FindAll(linkedType.type[i]);

					for (int ii = 0; ii < containedData.Count; ii++)
					{
						isLinkedField = true;
						System.Type type = containedData[ii].data.GetType();
						var value = type.GetField("infos").GetValue(containedData[ii].data);
						prefixID = (long)type.GetField("prefixID", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(containedData[ii].data);
						IList datas = (IList)value;
						System.Type cellType = datas[0].GetType();


						for (int iii = 0; iii < datas.Count; iii++)
						{
							var data = datas[iii];
							long tid = (long)cellType.GetField("tid").GetValue(data);
							string desc = (string)cellType.GetField("description").GetValue(data);

							long replacedTid = EditorHelper.ReplacePrefixTid(tid, prefixID);
							nameList.Add($"{replacedTid}:{desc}");
							tidList.Add(replacedTid);
						}
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
					if (isLinkedField)
					{
						for (int i = 0; i < containedData.Count; i++)
						{
							System.Type type = containedData[i].data.GetType();
							var value = type.GetField("infos").GetValue(containedData[i].data);
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
						lastTid = 1;
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

						selectedIndex = EditorGUI.Popup(tempRect, selectedIndex, nameList.ToArray());
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
					if (sf == null)
					{
						continue;
					}
					if (sf.isArray && sf.propertyType == SerializedPropertyType.Generic)
					{
						if (GUI.Button(tempRect, "Edit"))
						{
							SubListDataWindow2 widnow = new SubListDataWindow2();
							widnow.DrawChild(owner, so, sf);
							PopupWindow.Show(tempRect, widnow);
						}
					}
					else if (sf.propertyType == SerializedPropertyType.Generic)
					{
						if (GUI.Button(tempRect, "Edit"))
						{
							SubListDataWindow2 widnow = new SubListDataWindow2();
							widnow.DrawChild(owner, so, sf);
							PopupWindow.Show(tempRect, widnow);
						}
					}
					else
					{
						EditorGUI.PropertyField(tempRect, sf, GUIContent.none);
					}

					tempRect.x += tempRect.width + settings.rowSpace;
				}
			}
			else
			{
				//tidList
				if (property.name.Contains("Tid", StringComparison.Ordinal))
				{
					string ss = property.name.Split("Tid")[0] + "DataSheet";
					ss = ss.FirstCharacterToUpper();

					var sf = info;//.FindPropertyRelative(tidField.Name);
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

						selectedIndex = EditorGUI.Popup(tempRect, selectedIndex, nameList.ToArray());
						sf.longValue = tidList[selectedIndex];
					}
					else
					{
						EditorGUI.PropertyField(tempRect, sf, GUIContent.none);
					}


				}
				else
				{
					EditorGUI.PropertyField(tempRect, info, GUIContent.none);
				}

				tempRect.x += tempRect.width + settings.rowSpace;
			}
		};
	}
}

public class SubListDataWindow2 : PopupWindowContent
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
	private List<string> nameList = new List<string>();
	private int selectedIndex = 0;
	private bool isLinkedField = false;
	private long prefixID;

	private ContainedData containedData;

	public void CreateSubList()
	{
		Type rawDataType = ConvertUtility.ConvertStringToType(property.arrayElementType);

		System.Type baseType = rawDataType;
		System.Type check = rawDataType;


		bool isObjectType = check.IsClass || check.IsStruct();

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
				for (int i = 0; i < linkedType.type.Count; i++)
				{
					containedData = owner.dataContainer.Find(linkedType.type[i]);
					if (containedData != null)
					{
						isLinkedField = true;
						System.Type type = containedData.data.GetType();
						var value = type.GetField("infos").GetValue(containedData.data);
						prefixID = (long)type.GetField("prefixID", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(containedData.data);
						IList datas = (IList)value;
						System.Type cellType = datas[0].GetType();

						for (int ii = 0; ii < datas.Count; ii++)
						{
							var data = datas[ii];
							long tid = (long)cellType.GetField("tid").GetValue(data);
							string desc = (string)cellType.GetField("description").GetValue(data);


							long replacedTid = EditorHelper.ReplacePrefixTid(tid, prefixID);
							nameList.Add($"{replacedTid}:{desc}");
							tidList.Add(replacedTid);
						}
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

						selectedIndex = EditorGUI.Popup(tempRect, selectedIndex, nameList.ToArray());
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

					//if (sf.isArray && sf.propertyType == SerializedPropertyType.Generic)
					//{
					//	if (GUI.Button(tempRect, "Edit"))
					//	{
					//		var activator = GUILayoutUtility.GetLastRect();
					//		SubListDataWindow widnow = new SubListDataWindow();

					//		//widnow.DrawChild(owner, so, property);
					//		PopupWindow.Show(activator, widnow);
					//	}
					//}
					//else
					{
						EditorGUI.PropertyField(tempRect, sf, GUIContent.none);
					}

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
