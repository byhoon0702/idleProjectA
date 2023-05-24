using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Unity.VisualScripting;

using UnityEditor;
using UnityEngine;
using DG.DemiEditor;

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

		FieldInfo[] fieldInfos = EditorHelper.GetSerializedField(rawDataType);

		linkedTypeContainer = new LinkedTypeContainer();
		linkedDataDic = new Dictionary<string, LinkedData>();

		CreateLinkedTypeContainer(fieldInfos);
		CreateLinkedData();

		reorderableList.headerHeight = 50;
		reorderableList.drawHeaderCallback += (rect, guicontent) =>
		{
			rect.x = 34;
			Rect tempRect = new Rect(rect);
			Rect inputRect = new Rect(rect);
			SerializedProperty fieldSettings = serializedObject.FindProperty("fieldSettings");
			if (fieldSettings.arraySize != fieldInfos.Length)
			{
				fieldSettings.arraySize = fieldInfos.Length;
			}

			float width = 0;
			tempRect.height = EditorGUIUtility.singleLineHeight;


			for (int i = 0; i < fieldInfos.Length; i++)
			{
				var field = fieldInfos[i];
				if (field.Attributes.HasFlag(FieldAttributes.NotSerialized))
				{
					continue;
				}

				var elementproperty = fieldSettings.GetArrayElementAtIndex(i);
				tempRect.x += width + settings.rowSpace;
				width = GetWidth(elementproperty);
				tempRect.width = width;
				inputRect = tempRect;

				SetFieldWidthUI(elementproperty, field.Name, inputRect);
				EditorGUI.LabelField(tempRect, field.Name, EditorStyles.boldLabel);
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

			for (int i = 0; i < fieldInfos.Length; i++)
			{
				var field = fieldInfos[i];
				tempRect.y = rect.y;
				tempRect.height = EditorGUIUtility.singleLineHeight;
				SerializedProperty sf = info.FindPropertyRelative(field.Name);
				if (sf == null)
				{
					continue;
				}

				var elementproperty = fieldSettings.GetArrayElementAtIndex(i);
				tempRect.x += width + settings.rowSpace;

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
							continue;
						}
					}
					else
					{
						EditorGUI.PropertyField(tempRect, sf, GUIContent.none);
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

				linkedTypeContainer.Add(fields[i].Name, new List<System.Type>() { type });
			}

			else if (fields[i].Name.Contains("ItemRewards", StringComparison.Ordinal))
			{
				List<System.Type> typelist = new List<Type>();
				for (int ii = 0; ii < dataContainer.dataContainer.Count; ii++)
				{
					if (dataContainer.dataContainer[ii].type.Name.Contains("ItemData"))
					{
						if (typelist.Find(x => x.Equals(dataContainer.dataContainer[ii].type)) == null)
						{
							typelist.Add(dataContainer.dataContainer[ii].type);
						}
					}
				}
				linkedTypeContainer.Add(fields[i].Name, typelist);
			}
			else if (fields[i].Name.Contains("DataList", StringComparison.Ordinal))
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

				linkedTypeContainer.Add(fields[i].Name, new List<System.Type>() { type });
			}
		}
	}
	private void CreateLinkedData()
	{

		for (int i = 0; i < linkedTypeContainer.container.Count; i++)
		{
			string fieldName = linkedTypeContainer.container[i].fieldName;
			var linkType = linkedTypeContainer.Find(fieldName);

			List<long> tids = new List<long>();
			List<string> names = new List<string>();

			for (int ii = 0; ii < linkType.type.Count; ii++)
			{
				var containedDatas = dataContainer.FindAll(linkType.type[ii]);
				tids.Add(0);
				names.Add("0 : Empty");
				for (int iii = 0; iii < containedDatas.Count; iii++)
				{
					var containedData = containedDatas[iii];
					FieldInfo obj = containedData.type.GetField("infos");
					object ff = obj.GetValue(containedData.data);

					if (typeof(IList).IsAssignableFrom(ff))
					{
						IList list = (IList)ff;
						Type itemType = typeof(BaseData);
						FieldInfo tidField = itemType.GetField("tid");
						FieldInfo descField = itemType.GetField("description");
						for (int iiii = 0; iiii < list.Count; iiii++)
						{
							object item = list[iiii];

							long id = (long)tidField.GetValue(item);
							tids.Add(id);
							names.Add($"{id} :{(string)descField.GetValue(item)}");

						}
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



