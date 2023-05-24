using System;
using System.Collections.Generic;

using System.IO;
using System.Text;

using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections;
[Serializable]
public class LinkedData
{
	public string[] nameArray;
	public long[] tidArray;

}
[Serializable]
public class LinkedTypeContainer
{
	public List<LinkTypeInfo> container;
	public LinkedTypeContainer()
	{
		container = new List<LinkTypeInfo>();
	}

	~LinkedTypeContainer()
	{
		container.Clear();
		container = null;
	}

	public void Add(LinkTypeInfo info)
	{
		container.Add(info);
	}
	public void Add(string name, List<System.Type> type)
	{
		LinkTypeInfo info = new LinkTypeInfo();
		info.fieldName = name;
		info.type = type;
		container.Add(info);
	}
	public bool isExist(string name)
	{
		for (int i = 0; i < container.Count; i++)
		{
			var data = container[i];
			if (data.fieldName == name)
			{
				return true;
			}
		}
		return false;
	}
	public LinkTypeInfo Find(string name)
	{
		for (int i = 0; i < container.Count; i++)
		{
			var data = container[i];
			if (data.fieldName == name)
			{
				return data;
			}
		}
		return null;

	}

}
[Serializable]
public class LinkTypeInfo
{
	public string fieldName;

	public List<System.Type> type;
}

public partial class DataTableEditor
{
	private Vector2 editScrollPos;
	private void DrawPrefixButton()
	{
		if (prefixIDProperty.longValue < minTidPrefix)
		{
			prefixIDProperty.longValue = minTidPrefix;
		}
		EditorGUILayout.PropertyField(prefixIDProperty, new GUIContent("prefix ID"), GUILayout.Width(300));
		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Prefix 교체", GUILayout.Width(200)))
		{
			ChangeTid();
		}
		EditorGUILayout.LabelField(prefixChangeDescription);

		//EditorGUILayout.BeginHorizontal();
		//if (GUILayout.Button("Prefix 덧셈", GUILayout.Width(200)))
		//{
		//	PlusTid();
		//}

		//EditorGUILayout.LabelField(prefixAddDescription);
		//EditorGUILayout.EndHorizontal();
		//EditorGUILayout.BeginHorizontal();

		//if (GUILayout.Button("Prefix 뺄셈", GUILayout.Width(200)))
		//{
		//	MinusTid();
		//}
		//EditorGUILayout.LabelField(prefixMinusDescription);
		EditorGUILayout.EndHorizontal();

		System.Type t = serializedObject.targetObject.GetType();
		if (t.Equals(typeof(StatusDataSheetObject)))
		{
			if (GUILayout.Button("Enum 생성", GUILayout.Width(200)))
			{
				t.GetMethod("Call", BindingFlags.Public | BindingFlags.Instance).Invoke(serializedObject.targetObject, null);
			}
		}
	}

	private void ChangeTid()
	{
		System.Type type = infosProperty.arrayElementType.GetAssemblyType();
		for (int i = 0; i < infosProperty.arraySize; i++)
		{
			var property = infosProperty.GetArrayElementAtIndex(i).FindPropertyRelative("tid");
			property.longValue = EditorHelper.ReplacePrefixTid(property.longValue, prefixIDProperty.longValue);
		}
	}

	private void PlusTid()
	{
		System.Type type = infosProperty.arrayElementType.GetAssemblyType();
		for (int i = 0; i < infosProperty.arraySize; i++)
		{
			var property = infosProperty.GetArrayElementAtIndex(i).FindPropertyRelative("tid");
			property.longValue = property.longValue + prefixIDProperty.longValue;

		}
	}

	private void MinusTid()
	{
		System.Type type = infosProperty.arrayElementType.GetAssemblyType();
		for (int i = 0; i < infosProperty.arraySize; i++)
		{
			var property = infosProperty.GetArrayElementAtIndex(i).FindPropertyRelative("tid");
			property.longValue = property.longValue - prefixIDProperty.longValue;
			if (property.longValue < 0)
			{
				property.longValue = 1;
			}
		}
	}
	private void DrawVerifyLinktidButton()
	{
		//if (GUILayout.Button("Link Tid 검증 및 교체", GUILayout.Width(200)))
		//{
		//	VerifyLinkTID();
		//}
	}

	private void VerifyLinkTID()
	{
		//link
	}

	private void DrawHeader(float width)
	{
		EditorGUILayout.BeginHorizontal();
		addArraySize = EditorGUILayout.IntField("리스트 개수 ", addArraySize, GUILayout.Width(300));
		if (GUILayout.Button("리스트 추가", GUILayout.Width(130)))
		{

			for (int i = 0; i < addArraySize; i++)
			{
				int newIndex = infosProperty.arraySize;
				infosProperty.arraySize++;
				long lastTid = 0;
				if (newIndex - 1 >= 0)
				{
					var laseElement = infosProperty.GetArrayElementAtIndex(newIndex - 1);
					lastTid = laseElement.FindPropertyRelative("tid").longValue;
				}

				var newElement = infosProperty.GetArrayElementAtIndex(newIndex);
				var proprety = serializedObject.FindProperty("dataSheet");
				var prefixId = proprety.FindPropertyRelative("prefixID");


				long replacedID = EditorHelper.ReplacePrefixTid(lastTid + 1, prefixId.longValue);
				EditorHelper.AddNewElement(newElement, replacedID);
			}

			//infosProperty.arraySize += addArraySize;


		}
		if (GUILayout.Button("리스트 삭제", GUILayout.Width(130)))
		{
			if (infosProperty.arraySize < addArraySize)
			{
				infosProperty.arraySize = 0;
			}
			else
			{
				infosProperty.arraySize -= addArraySize;
			}
		}
		EditorGUILayout.EndHorizontal();
		pageSize = EditorGUILayout.IntField("페이지당 데이터 개수 ", pageSize, GUILayout.Width(300));

	}

	private void DrawDataEdit()
	{
		label = EditorGUILayout.TextField("데이터 테이블 이름", label);
		if (label.IsNullOrEmpty())
		{
			if (GUILayout.Button("Load Json"))
			{
				FromJson();
			}
			if (GUILayout.Button("Load CSV"))
			{
				FromCSV();
			}
			EditorGUILayout.HelpBox("테이블 이름을 입력하거나 Json 파일을 불러오세요", MessageType.Warning);
		}
		else
		{
			if (serializedObject == null)
			{
				if (GetScriptableObject(label) == false)
				{
					if (TypeExist())
					{
						if (GUILayout.Button("Create Scriptable Object"))
						{
							scriptableObject = CreateScriptableAsset(label);
							serializedObject = new SerializedObject(scriptableObject);
						}
					}
					else
					{
						if (GUILayout.Button("Create CSharp File"))
						{
							CreateCSharpFile();
						}
					}
					if (GUILayout.Button("Load Json"))
					{
						FromJson();
					}
				}
			}
		}
		if (serializedObject != null)
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			if (GUILayout.Button("Save Json", EditorStyles.toolbarButton))
			{
				ToJson();
				LoadAllDataPath("Json");
			}
			if (GUILayout.Button("Load Json", EditorStyles.toolbarButton))
			{
				FromJson();
			}
			if (GUILayout.Button("Load All Json", EditorStyles.toolbarButton))
			{
				LoadAllJson();
			}

			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			if (GUILayout.Button("Save CSV", EditorStyles.toolbarButton))
			{
				ToCSV();
				LoadAllDataPath("Csv");
			}
			if (GUILayout.Button("Load CSV", EditorStyles.toolbarButton))
			{
				FromCSV();
			}
			if (GUILayout.Button("Convert Json CSV", EditorStyles.toolbarButton))
			{
				ConvertJsonToCSV();
			}

			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal(EditorStyles.toolbar);

			if (GUILayout.Button("Unload Data", EditorStyles.toolbarButton))
			{
				label = "";
				maleeReorderableList = null;
				serializedObject = null;
				scriptableObject = null;
			}

			if (GUILayout.Button("Verify Data Tables", EditorStyles.toolbarButton))
			{
				VerifyTidDuplicate();
			}

			GUILayout.EndHorizontal();

		}
		if (dataContainer == null || dataContainer.dataContainer.Count == 0)
		{
			LoadAllJson();
		}

		DrawProperty();
	}

	void DrawProperty()
	{
		if (scriptableObject == null)
		{
			return;
		}

		if (serializedObject == null)
		{
			serializedObject = new SerializedObject(scriptableObject);
		}

		serializedObject.Update();

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.Space(10);

		if (maleeReorderableList == null)
		{
			CreateReorderableList();
		}

		EditorGUILayout.BeginVertical();

		GUIStyle boxStyle = new GUIStyle(GUI.skin.GetStyle("HelpBox"));
		boxStyle.richText = true;
		boxStyle.fontSize = 15;

		EditorGUILayout.TextArea($"현재 Json 파일 : {currentJsonFileName}\n현재 파일 경로 : {currentJsonFilePath}", boxStyle);

		DrawToolTip();

		if (dataSheetProperty != null)
		{

			var targetObje = dataSheetProperty.serializedObject.targetObject;
			FieldInfo dataSheetField = targetObje.GetType().GetField("dataSheet");
			Type tableType = dataSheetField.FieldType;

			prefixIDProperty = dataSheetProperty.FindPropertyRelative("prefixID");
			Type rawDataType = infosProperty.arrayElementType.GetAssemblyType();
			FieldInfo[] fields = EditorHelper.GetSerializedField(rawDataType);

			float width = fields.Length * (settings.cellSize.x + settings.rowSpace) + 34;

			dataSheetProperty.FindPropertyRelative("typeName").stringValue = tableType.Name;

			if (tableType.Name.Contains("ResultCode", StringComparison.Ordinal))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField($"데이터 타입: {tableType.Name}", GUILayout.Width(width / 2));


				DrawPrefixButton();

				if (GUILayout.Button("리절트 코드 생성", GUILayout.Width(width / 2)))
				{
					string methodName = targetObje.name.Replace("DataSheetObject", "");
					var methodinfo = targetObje.GetType().GetMethod($"Call");
					methodinfo.Invoke(targetObje, null);
				}
				EditorGUILayout.EndHorizontal();

			}
			else
			{
				EditorGUILayout.LabelField($"데이터 타입: {tableType.Name}", GUILayout.Width(width));

				DrawPrefixButton();

				var methodinfo = targetObje.GetType().GetMethod($"Call");
				if (methodinfo != null)
				{
					if (GUILayout.Button("Call"))
					{
						methodinfo.Invoke(targetObje, null);
					}
				}

			}
			DrawVerifyLinktidButton();
			DrawHeader(width);

			editScrollPos = GUILayout.BeginScrollView(editScrollPos);

			maleeReorderableList.DoLayoutList();

			SerializedProperty fieldSettings = serializedObject.FindProperty("fieldSettings");
			if (fieldSettings.arraySize != fields.Length)
			{
				fieldSettings.arraySize = fields.Length;
			}
			float totalWidth = settings.cellSize.x + 34;
			for (int i = 0; i < fieldSettings.arraySize; i++)
			{
				totalWidth += fieldSettings.GetArrayElementAtIndex(i).FindPropertyRelative("width").floatValue;
				totalWidth += settings.columeSpace * 2;
			}

			EditorGUILayout.LabelField("", GUILayout.Width(totalWidth - 140));
			GUILayout.EndScrollView();
		}

		EditorGUILayout.EndVertical();
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(scriptableObject);
			serializedObject.ApplyModifiedProperties();
		}
	}

	Vector2 tooltipScroll;

	private void DrawToolTip()
	{
		tooltipScroll = EditorGUILayout.BeginScrollView(tooltipScroll, GUILayout.Height(80));

		string tooltip = $"알림\n각 DataSheetObject 의 tooltip 변수를 설정 하세요.";
		var tooltipProperty = serializedObject.FindProperty("tooltip");
		if (tooltipProperty != null && (tooltipProperty.stringValue.IsNullOrEmpty() == false))
		{
			tooltip = $"알림\n{tooltipProperty.stringValue}";
		}
		GUIStyle style = GUI.skin.box;
		GUIContent guiContent = new GUIContent(tooltip);
		style.alignment = TextAnchor.UpperLeft;
		Vector2 size = style.CalcSize(guiContent);
		EditorGUILayout.LabelField(tooltip, style, GUILayout.Height(size.y), GUILayout.MinHeight(80), GUILayout.Width(position.width), GUILayout.ExpandHeight(true));
		//GUI.enabled = true;
		EditorGUILayout.EndScrollView();
	}

	private void CreateReorderableList()
	{
		if (serializedObject == null)
		{
			return;
		}



		dataSheetProperty = serializedObject.FindProperty("dataSheet");
		infosProperty = dataSheetProperty.FindPropertyRelative("infos");



		maleeReorderableList = ReorderableDataList.Init(settings, serializedObject, infosProperty).SetLoadedData(dataContainer).Build(pageSize, true);
	}

}
