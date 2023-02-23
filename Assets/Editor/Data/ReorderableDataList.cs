using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.DemiEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class ReorderableDataList
{
	private SerializedProperty property;
	public LinkedTypeContainer linkedTypeContainer
	{
		get;
		private set;
	}


	public DataContainer dataContainer
	{
		get;
		private set;
	}

	public DataTableEditorSettings settings
	{ get; private set; }
	private Malee.List.ReorderableList reorderableList;
	private SerializedObject serializedObject;

	public static ReorderableDataList Init(DataTableEditorSettings _settings, SerializedObject _object, SerializedProperty _property)
	{
		ReorderableDataList gen = new ReorderableDataList();
		gen.serializedObject = _object;
		gen.settings = _settings;
		gen.property = _property;
		return gen;
	}


	public ReorderableDataList SetLoadedData(DataContainer _dataContainer)
	{
		dataContainer = _dataContainer;

		return this;
	}


	public Malee.List.ReorderableList Build(int pageSize, bool paginate)
	{
		if (reorderableList != null)
		{
			reorderableList = null;
		}
		reorderableList = new Malee.List.ReorderableList(property);
		reorderableList.paginate = paginate;
		reorderableList.pageSize = pageSize;

		CreateMainList();
		return reorderableList;
	}



	public Dictionary<string, LinkedData> linkedDataDic = new Dictionary<string, LinkedData>();
	private void CreateMainList()
	{
		Type rawDataType = property.arrayElementType.GetAssemblyType();
		FieldInfo[] fields = rawDataType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		linkedTypeContainer = new LinkedTypeContainer();
		linkedDataDic = new Dictionary<string, LinkedData>();

		CreateLinkedTypeContainer(fields);
		CreateLinkedData();

		reorderableList.headerHeight = 50;
		reorderableList.drawHeaderCallback += (rect, guicontent) =>
		{
			rect.x = 34;
			Rect tempRect = new Rect(rect);
			Rect inputRect = new Rect(rect);
			SerializedProperty fieldSettings = serializedObject.FindProperty("fieldSettings");
			if (fieldSettings.arraySize != fields.Length)
			{
				fieldSettings.arraySize = fields.Length;
			}

			float width = 0;
			tempRect.height = EditorGUIUtility.singleLineHeight;

			var elementproperty = fieldSettings.GetArrayElementAtIndex(0);
			width = GetWidth(elementproperty);
			tempRect.x = rect.x;
			tempRect.width = width;
			inputRect = tempRect;

			SetFieldWidthUI(elementproperty, "tid", inputRect);
			EditorGUI.LabelField(tempRect, "tid", EditorStyles.boldLabel);

			elementproperty = fieldSettings.GetArrayElementAtIndex(1);
			tempRect.x += (width + settings.rowSpace);

			width = GetWidth(elementproperty);
			tempRect.width = width;

			inputRect = tempRect;

			SetFieldWidthUI(elementproperty, "description", inputRect);
			EditorGUI.LabelField(tempRect, "description", EditorStyles.boldLabel);


			for (int i = 0; i < fields.Length; i++)
			{
				//tempRect.width = settings.cellSize.x;

				if (fields[i].Attributes.HasFlag(FieldAttributes.NotSerialized))
				{
					continue;
				}

				if (fields[i].Name == "tid" || fields[i].Name == "description")
				{
					continue;
				}

				{
					elementproperty = fieldSettings.GetArrayElementAtIndex(i + 2);
					tempRect.x += width + settings.rowSpace;
					width = GetWidth(elementproperty);
					tempRect.width = width;
					inputRect = tempRect;

					SetFieldWidthUI(elementproperty, fields[i].Name, inputRect);
					EditorGUI.LabelField(tempRect, fields[i].Name, EditorStyles.boldLabel);
				}

				//tempRect.x += width + settings.rowSpace;
			}
		};

		float GetWidth(SerializedProperty property)
		{
			float value = property.FindPropertyRelative("width").floatValue;
			if (value == 0)
			{
				value = settings.cellSize.x;
			}
			return value;
		}

		void SetFieldWidthUI(SerializedProperty property, string fieldName, Rect rect)
		{
			property.FindPropertyRelative("fieldName").stringValue = fieldName;
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			var widthProperty = property.FindPropertyRelative("width");
			if (widthProperty.floatValue == 0)
			{
				widthProperty.floatValue = (int)settings.cellSize.x;
			}
			widthProperty.floatValue = EditorGUI.FloatField(rect, widthProperty.floatValue);
		}

		reorderableList.onAddCallback += (list) =>
		{
			serializedObject.Update();

			var proprety = serializedObject.FindProperty("dataSheet");

			var targetObj = serializedObject.targetObject;
			var obj = targetObj.GetType().GetField("dataSheet").GetValue(targetObj);
			System.Type stype = proprety.type.GetAssemblyType();
			MethodInfo methodInfo = stype.GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
			stype.GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(obj, null);


			serializedObject.ApplyModifiedProperties();
		};

		reorderableList.onSelectCallback += MaleeReorderableList_onSelectCallback;
		reorderableList.getElementHeightCallback += MaleeReorderableList_getElementHeightCallback;
		reorderableList.drawElementCallback += (rect, element, guiContent, isActive, isFocused) =>
		{
			float width = 0;
			Rect tempRect = new Rect(rect);
			SerializedProperty info = element;
			Type type = rawDataType;
			SerializedProperty fieldSettings = serializedObject.FindProperty("fieldSettings");


			SerializedProperty sf = info.FindPropertyRelative("tid");
			SerializedProperty elementproperty = fieldSettings.GetArrayElementAtIndex(0);
			width = GetWidth(elementproperty);
			tempRect.x = rect.x;
			tempRect.width = width;
			EditorGUI.PropertyField(tempRect, sf, GUIContent.none);


			sf = info.FindPropertyRelative("description");
			elementproperty = fieldSettings.GetArrayElementAtIndex(1);
			tempRect.x += (width + settings.rowSpace);
			width = GetWidth(elementproperty);
			tempRect.width = width;
			EditorGUI.PropertyField(tempRect, sf, GUIContent.none);


			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				tempRect.y = rect.y;
				tempRect.height = EditorGUIUtility.singleLineHeight;
				sf = info.FindPropertyRelative(field.Name);
				if (sf == null)
				{
					continue;
				}
				var tidField = info.FindPropertyRelative("tid");

				if (field.Name == "tid" || field.Name == "description")
				{
					continue;
				}

				else
				{
					elementproperty = fieldSettings.GetArrayElementAtIndex(i + 2);
					tempRect.x += width + settings.rowSpace;
					//Rect relativeRect = new Rect(tempRect.x + ((width + settings.rowSpace)), tempRect.y, width, tempRect.height);
					width = GetWidth(elementproperty);
					tempRect.width = width;
					if (sf.propertyType.Equals(SerializedPropertyType.Generic))

					{
						if (GUI.Button(tempRect, "Edit"))
						{
							CreateSubWindow(tempRect, sf);
						}
					}
					else
					{
						if (linkedTypeContainer.Find(field.Name) != null)
						{
							if (DrawLinkedProperty(tempRect, tempRect, sf, field.Name))
							{
								//tempRect.x += width + settings.rowSpace;
								continue;
							}
						}
						else
						{
							EditorGUI.PropertyField(tempRect, sf, GUIContent.none);
						}
					}
				}
			}
		};
	}

	private void CreateLinkedTypeContainer(FieldInfo[] fields)
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
				if (linkedTypeContainer.Find(typeName) != null)
				{
					continue;
				}


				linkedTypeContainer.Add(fields[i].Name, type);
			}
			if (fields[i].Name.Contains("DataList", StringComparison.Ordinal))
			{
				string[] split = fields[i].Name.Split("DataList");
				string typeName = $"{split[0]}DataSheet".FirstCharacterToUpper();

				string typeString = ConvertUtility.GetAssemblyName(typeName);
				System.Type type = System.Type.GetType(typeString);
				if (type == null)
				{
					continue;
				}
				if (linkedTypeContainer.Find(typeName) != null)
				{
					continue;
				}

				linkedTypeContainer.Add(fields[i].Name, type);
			}
		}
	}
	private void CreateLinkedData()
	{
		for (int i = 0; i < linkedTypeContainer.container.Count; i++)
		{
			string fieldName = linkedTypeContainer.container[i].fieldName;
			var linkType = linkedTypeContainer.Find(fieldName);

			var containedData = dataContainer.Find(linkType.type);
			List<long> tids = new List<long>();
			List<string> names = new List<string>();
			if (containedData != null)
			{
				FieldInfo obj = containedData.type.GetField("infos");
				object ff = obj.GetValue(containedData.data);
				tids.Add(0);
				names.Add("0 : Empty");
				if (typeof(IList).IsAssignableFrom(ff))
				{
					IList list = (IList)ff;
					Type itemType = typeof(BaseData);
					FieldInfo tidField = itemType.GetField("tid");
					FieldInfo descField = itemType.GetField("description");
					for (int ii = 0; ii < list.Count; ii++)
					{
						object item = list[ii];

						long id = (long)tidField.GetValue(item);
						tids.Add(id);
						names.Add($"{id} :{(string)descField.GetValue(item)}");

					}
				}
			}
			linkedDataDic.Add(fieldName, new LinkedData() { tidArray = tids.ToArray(), nameArray = names.ToArray() });
		}

	}


	private bool DrawLinkedProperty(Rect _relativeRect, Rect _rect, SerializedProperty _sf, string _fieldName)
	{
		var linkType = linkedTypeContainer.Find(_fieldName);
		if (linkType == null)
		{
			EditorGUI.PropertyField(_relativeRect, _sf, GUIContent.none);
			_rect.x += _rect.width + settings.rowSpace;
			return false;
		}

		//var containedData = dataContainer.Find(linkType.type);

		//if (containedData != null)
		{
			int selectedIndex = 0;

			for (int ii = 0; ii < linkedDataDic[_fieldName].tidArray.Length; ii++)
			{
				long item = linkedDataDic[_fieldName].tidArray[ii];

				if (_sf.longValue == item)
				{
					selectedIndex = ii;
				}
			}

			if (linkedDataDic[_fieldName].nameArray.Length > 0)
			{
				selectedIndex = EditorGUI.Popup(_relativeRect, selectedIndex, linkedDataDic[_fieldName].nameArray);
				_sf.longValue = linkedDataDic[_fieldName].tidArray[selectedIndex];
				_rect.x += _rect.width + settings.rowSpace;
				return true;
			}

		}
		return true;
	}

	private void CreateSubWindow(Rect rect, SerializedProperty property)
	{
		var activatorrect = rect;

		SubListDataWindow widnow = new SubListDataWindow();

		widnow.DrawChild(this, serializedObject, property);
		PopupWindow.Show(activatorrect, widnow);

	}

	private void MaleeReorderableList_onSelectCallback(Malee.List.ReorderableList list)
	{

	}

	private float MaleeReorderableList_getElementHeightCallback(SerializedProperty element)
	{


		return EditorGUIUtility.singleLineHeight;
	}

}



public static class EditorHelper
{
	public static void AddNewElement(SerializedProperty property, long tid)
	{
		System.Type type = ConvertUtility.ConvertStringToType(property.type);
		System.Type baseType = type;
		System.Type check = type;
		while (baseType != null)
		{
			check = baseType;
			baseType = check.BaseType;
		}

		bool isObject = check.Equals(typeof(System.Object));

		if (isObject)
		{
			var newData = Activator.CreateInstance(type);
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (var field in fields)
			{
				var relativeProperty = property.FindPropertyRelative(field.Name);

				if (relativeProperty == null)
				{
					continue;
				}
				object value = field.GetValue(newData);
				if (field.Name == "tid")
				{
					value = tid;
				}

				try
				{
					EditorHelper.SetSerializedPropertyValue(relativeProperty, value);

				}
				catch (Exception e)
				{
					Debug.LogError($"{relativeProperty.propertyType} , {field.Name}");
				}
			}
		}
	}
	public static void SetSerializedPropertyValue(SerializedProperty p, object value)
	{
		System.Type type = ConvertUtility.ConvertStringToType(p.type);
		if (type == null)
		{
			p.SetValue(value);
			return;
		}
		if (type.Equals(typeof(int)))
		{
			p.intValue = (int)value;
		}
		else if (type.Equals(typeof(long)))
		{
			p.longValue = (long)value;
		}
		else if (type.Equals(typeof(string)))
		{
			if (value == null)
			{
				p.stringValue = "";
			}
			else
			{
				p.stringValue = (string)value;
			}
		}
		else
		{
			p.SetValue(value);
		}
	}
	public static long ReplacePrefixTid(long originID, long prefix)
	{
		int count = 0;

		long origin = prefix;

		while (origin % 10 == 0)
		{
			origin /= 10;
			count++;
		}

		if (count > 0)
		{
			originID = originID % (int)Mathf.Pow(10, count) + prefix;
		}
		return originID;
	}
}
