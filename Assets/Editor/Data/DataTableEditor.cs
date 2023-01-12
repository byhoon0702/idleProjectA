using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Reflection;
using System.Collections;

using UnityEngine.Purchasing.MiniJSON;
using System.Linq;

public class DataTableEditor : EditorWindow
{
	[SerializeField]
	public string className;

	private bool foldOut = false;

	[SerializeField]
	public SerializedObject serializedObject;
	[SerializeField]
	public SerializedObject serializedWindowObject;
	[SerializeField]
	public ScriptableObject scriptableObject;

	public Vector2 scrollPos;

	private ReorderableList reorderableList;
	private string label;
	private string searchString;
	private Type instanceType;
	private string currentJsonFileName;
	private string currentJsonFilePath;
	[SerializeField]
	private Dictionary<string, object> jsonContainer;
	[SerializeField]
	private Dictionary<string, Type> linkedTypeList;
	[SerializeField]
	public SerializedProperty dataSheetProperty;

	[MenuItem("Custom Menu/DataEditor/DataTableEditor", false, 5000)]
	public static void Init()
	{
		var window = EditorWindow.CreateWindow<DataTableEditor>(new Type[] { typeof(DataTableEditor), });
		window.minSize = new Vector2(330f, 360f);

		window.Show();
		window.InitData();
	}

	public void InitData()
	{
		LoadAllJson();
	}
	private void OnDestroy()
	{

	}

	private static readonly Type[] types =
	{
		typeof(DataTableEditor),
	};


	public override IEnumerable<Type> GetExtraPaneTypes()
	{

		return types;
	}

	private string _instanceName;
	public string instanceName
	{
		set
		{
			_instanceName = value;
		}
		get
		{
			return $"{_instanceName}, Assembly-CSharp";
		}
	}


	public string TypeString(string name, string assembly = "Assembly-CSharp")
	{
		return $"{name}, {assembly}";
	}

	public static T TryParse<T>(string value)
	{
		TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
		return (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, value);
	}

	public void LoadAllJson()
	{
		jsonContainer = new Dictionary<string, object>();
		string[] files = Directory.GetFiles(Application.dataPath + "/AssetFolder/Resources/Json");

		foreach (string file in files)
		{
			if (file.Contains(".meta"))
			{
				continue;
			}
			if (file.Contains("DataSheet") == false)
			{
				continue;
			}
			using (FileStream fs = File.OpenRead(file))
			{
				using (BinaryReader br = new BinaryReader(fs))
				{
					string name = System.IO.Path.GetFileNameWithoutExtension(file);

					System.Type t = System.Type.GetType($"{name}, Assembly-CSharp");

					string json = br.ReadString();

					try
					{
						var jsonData = JsonUtility.FromJson(json, t);
						jsonContainer.Add(t.Name, jsonData);
					}
					catch (Exception e)
					{

					}

				}
			}
		}
	}


	void OnGUI()
	{
		EditorGUILayout.Space(10);
		label = EditorGUILayout.TextField("데이터 테이블 이름", label);
		if (label.IsNullOrEmpty())
		{
			EditorGUILayout.Space(3);
			if (GUILayout.Button("Load Json"))
			{
				FromJson();
			}
			EditorGUILayout.Space(3);
			EditorGUILayout.HelpBox("테이블 이름을 입력하거나 Json 파일을 불러오세요", MessageType.Warning);
			return;
		}
		EditorGUILayout.Space(3);

		if (jsonContainer == null)
		{
			LoadAllJson();
		}
		if (GetScriptableObject(label) == false)
		{
			if (TypeExist())
			{
				if (GUILayout.Button("Create Scriptable Object"))
				{
					CreateScriptableAsset(label);
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
		else
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			if (GUILayout.Button("Save Json", EditorStyles.toolbarButton))
			{
				ToJson();
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
			if (TypeExist() == false)
			{
				if (GUILayout.Button("Create CSharp File"))
				{
					CreateCSharpFile();
				}
			}


			DrawProperty();
		}

		EditorUtility.SetDirty(this);
	}
	string[] GetClassNames()
	{
		Directory.GetFiles(Application.dataPath + "/Scripts/");


		return new string[] { };
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

		EditorGUILayout.Space(10);

		if (reorderableList == null)
		{
			CreateReorderableList();
		}

		EditorGUILayout.BeginVertical();

		GUIStyle boxStyle = new GUIStyle(GUI.skin.GetStyle("HelpBox"));
		boxStyle.richText = true;
		boxStyle.fontSize = 15;
		EditorGUILayout.TextArea($"현재 Json 파일 : {currentJsonFileName}\n현재 파일 경로 : {currentJsonFilePath}", boxStyle);

		scrollPos = GUILayout.BeginScrollView(scrollPos, true, false, GUILayout.Width(1000));
		GUILayout.Box("", GUILayout.Width(EditorGUIUtility.currentViewWidth));

		if (dataSheetProperty != null)
		{
			var targetObje = dataSheetProperty.serializedObject.targetObject;
			Type tableType = targetObje.GetType().GetField("dataSheet").FieldType;
			dataSheetProperty.FindPropertyRelative("typeName").stringValue = tableType.Name;
			EditorGUILayout.LabelField(dataSheetProperty.FindPropertyRelative("typeName").stringValue);
		}

		reorderableList.DoLayoutList();

		GUILayout.EndScrollView();
		EditorGUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();

		EditorUtility.SetDirty(scriptableObject);
	}

	private void CreateReorderableList()
	{
		if (serializedObject == null)
		{
			return;
		}
		dataSheetProperty = serializedObject.FindProperty("dataSheet");
		var property = dataSheetProperty.FindPropertyRelative("infos");
		CreateReorderableList(dataSheetProperty.serializedObject, property);
		return;
	}

	void CreateReorderableList(SerializedObject _serializedObjet, SerializedProperty _serializeProperty)
	{
		if (_serializedObjet == null || _serializeProperty == null)
		{
			return;
		}
		Type rawDataType = System.Type.GetType($"{_serializeProperty.arrayElementType}, Assembly-CSharp");
		FieldInfo[] fields = rawDataType.GetFields();
		Type[] nested = rawDataType.GetNestedTypes();

		linkedTypeList = new Dictionary<string, Type>();

		float width = (EditorGUIUtility.currentViewWidth / fields.Length) - 10;
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

		reorderableList = new ReorderableList(_serializedObjet, _serializeProperty);

		reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 2;
		reorderableList.drawHeaderCallback += rect =>
		{
			Rect tempRect = new Rect(rect);
			tempRect.x = 20;
			for (int i = 0; i < fields.Length; i++)
			{
				tempRect.width = width;
				tempRect.height = EditorGUIUtility.singleLineHeight;

				EditorGUI.LabelField(tempRect, fields[i].Name, EditorStyles.boldLabel);
				tempRect.x += tempRect.width + 5;
			}
		};

		reorderableList.drawElementCallback += (rect, index, isActive, isFocused) =>
		{
			Rect tempRect = new Rect(rect);
			SerializedProperty info = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			Type type = rawDataType;

			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				tempRect.y = rect.y;
				tempRect.width = width;
				tempRect.height = EditorGUIUtility.singleLineHeight;
				var sf = info.FindPropertyRelative(field.Name);

				if (typeof(IList).IsAssignableFrom(sf))
				{

				}
				if (linkedTypeList.ContainsKey(field.Name))
				{
					Type linkType = linkedTypeList[field.Name];
					if (linkType == null)
					{
						EditorGUI.PropertyField(tempRect, sf, GUIContent.none);
						tempRect.x += tempRect.width + 5;
						continue;
					}
					if (jsonContainer.ContainsKey(linkType.Name))
					{
						var obj = jsonContainer[linkType.Name].GetType().GetField("infos");
						var ff = obj.GetValue(jsonContainer[linkType.Name]);

						List<GUIContent> nameList = new List<GUIContent>();
						List<int> idList = new List<int>();

						if (typeof(IList).IsAssignableFrom(ff))
						{
							foreach (var item in ff as IList)
							{
								Type itemType = item.GetType();
								long id = (long)itemType.GetField("tid").GetValue(item);
								idList.Add((int)id);
								nameList.Add(new GUIContent($"{id} :{(string)itemType.GetField("description").GetValue(item)}"));
							}
						}

						if (nameList.Count > 0)
						{
							EditorGUI.IntPopup(tempRect, sf, nameList.ToArray(), idList.ToArray(), GUIContent.none);
							tempRect.x += tempRect.width + 5;
							continue;
						}
					}
				}

				if (sf.type == "IdleNumber")
				{
					Rect idlenumberRect = new Rect(tempRect);

					idlenumberRect.width = tempRect.width / 2f - 2.5f;

					EditorGUI.PropertyField(idlenumberRect, sf.FindPropertyRelative("Value"), GUIContent.none);
					idlenumberRect.x += idlenumberRect.width + 5;
					EditorGUI.PropertyField(idlenumberRect, sf.FindPropertyRelative("Exp"), GUIContent.none);
				}
				else
				{
					EditorGUI.PropertyField(tempRect, sf, GUIContent.none);
				}

				tempRect.x += tempRect.width + 5;
			}
		};

		//	reorderableList.onSelectCallback += index => { };
	}

	bool TypeExist()
	{

		instanceName = searchString;
		instanceType = System.Type.GetType(instanceName);
		return instanceType != null;
	}

	/// <summary>
	/// 인스펙터 편집용으로 제작되는 ScriptableObject
	/// 절대로 런타임에서 데이터를 넣는 용도로 사용하면 안됨
	/// 런타임에서는 무조건 Json 데이터를 사용
	/// </summary>
	ScriptableObject CreateScriptableAsset(string name)
	{
		System.Type type = System.Type.GetType($"{name}Object, Assembly-CSharp");
		ScriptableObject newDataObject = (ScriptableObject)Activator.CreateInstance(type);

		AssetDatabase.CreateAsset(newDataObject, $"Assets/Resources/{label}Object.asset");
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow();

		return newDataObject;
	}

	bool GetScriptableObject(string name)
	{
		if (name.IsNullOrEmpty())
		{
			return false;
		}

		string[] folders = new string[]
		{
			"Assets/Resources",
		};
		string[] guid = AssetDatabase.FindAssets($"t:ScriptableObject", folders);
		for (int i = 0; i < guid.Length; i++)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid[i]);
			string filename = Path.GetFileNameWithoutExtension(path);
			if (filename.ToLower() == $"{name}Object".ToLower())
			{
				scriptableObject = (ScriptableObject)AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject));
				searchString = filename;
				return true;
			}
		}
		searchString = $"{name}Object";
		reorderableList = null;
		serializedObject = null;
		scriptableObject = null;
		return false;
	}

	void ToJson()
	{
		var targetObje = serializedObject.FindProperty("dataSheet").serializedObject.targetObject;

		System.Reflection.FieldInfo info = targetObje.GetType().GetField("dataSheet");
		var sd = info.GetValue(targetObje);
		string rootPath = Path.GetDirectoryName(currentJsonFilePath);
		string path = EditorUtility.SaveFilePanel("", rootPath, currentJsonFileName, "json");

		if (path.IsNullOrEmpty())
		{
			return;
		}

		string json = JsonUtility.ToJson(sd);

		using (FileStream fs = File.Create(path))
		{
			using (BinaryWriter sw = new BinaryWriter(fs))
			{
				sw.Write(json);
			}
		}

		currentJsonFileName = Path.GetFileName(path);
		currentJsonFilePath = path;
		AssetDatabase.Refresh();
	}

	void FromJson()
	{

		if (currentJsonFilePath.IsNullOrEmpty())
		{
			currentJsonFilePath = $"{Application.dataPath}/AssetFolder/Resources/Json";
		}
		string rootPath = Path.GetDirectoryName(currentJsonFilePath);
		string path = EditorUtility.OpenFilePanel("", rootPath, "json");

		if (path.IsNullOrEmpty())
		{
			return;
		}

		using (FileStream fs = File.OpenRead(path))
		{
			string fileName = Path.GetFileNameWithoutExtension(path);
			string typeName = fileName;

			using (BinaryReader sr = new BinaryReader(fs))
			{
				string jsonstring = sr.ReadString();
				Dictionary<string, object> jb = (Dictionary<string, object>)Json.Deserialize(jsonstring);

				if (jb.ContainsKey("typeName"))
				{
					typeName = (string)jb["typeName"];
				}

				Type type = System.Type.GetType($"{typeName}, Assembly-CSharp");
				if (type == null)
				{
					sr.Close();
				}

				label = typeName;

				if (GetScriptableObject(typeName))
				{
					if (TypeExist())
					{
						var json = JsonUtility.FromJson(jsonstring, type);
						instanceType.GetField("dataSheet").SetValue(scriptableObject, json);
						serializedObject = new SerializedObject(scriptableObject);
					}
				}
				else
				{

				}

				CreateReorderableList();
				EditorGUI.FocusTextInControl(null);
				currentJsonFileName = Path.GetFileName(path);
				currentJsonFilePath = path;
			}
		}
	}

	void CreateCSharpFile()
	{
		string path = string.Concat(Application.dataPath, Path.DirectorySeparatorChar, $"Scripts/DataSheet/{label}Object.cs");
		using (FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
		{
			using (StreamWriter writer = new StreamWriter(fs))
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendLine($"//========AUTO GENERATED CODE======//");
				sb.AppendLine("using UnityEngine;");
				sb.AppendLine("using System;");
				sb.AppendLine("[Serializable]");
				sb.AppendLine($"public class {label}Object : ScriptableObject ");
				sb.AppendLine("{");
				sb.AppendLine("[SerializeField]");
				sb.AppendLine($"public {label} dataSheet;");
				sb.AppendLine("}");

				writer.Write(sb.ToString());
			}
		}

		AssetDatabase.Refresh();
	}
}
