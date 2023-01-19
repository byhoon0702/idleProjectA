using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections;

using UnityEngine.Purchasing.MiniJSON;
using JetBrains.Annotations;
using static UnityEngine.Rendering.DebugUI;
using System.Security.Cryptography;

public class DataTableEditor : EditorWindow
{

	public DataTableEditorSettings settings;
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

	//private ReorderableList reorderableList;
	private UnityEditorInternal.ReorderableList reorderableList;
	private Malee.List.ReorderableList maleeReorderableList;
	private string label;
	private string searchString;
	private Type instanceType;
	private string currentJsonFileName;
	private string currentJsonFilePath;
	[SerializeField]
	private Dictionary<Type, object> jsonContainer;
	[SerializeField]
	private Dictionary<string, Type> linkedTypeList;

	[SerializeField]
	private Dictionary<string, List<bool>> foldOutList;

	[SerializeField]
	public SerializedProperty dataSheetProperty;


	[SerializeField]
	public Dictionary<string, EditorWindow> subWindows = new Dictionary<string, EditorWindow>();



	private int addArraySize = 0;
	private int pageSize = 10;

	public void CreateSubWindow()
	{
		subWindows.Add("", null);
	}

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
		maleeReorderableList = null;
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




	public static T TryParse<T>(string value)
	{
		TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
		return (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, value);
	}

	public void LoadSettings()
	{
		if (settings != null)
		{
			return;
		}
		string[] guid = AssetDatabase.FindAssets($"t:ScriptableObject");
		for (int i = 0; i < guid.Length; i++)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid[i]);
			string filename = Path.GetFileNameWithoutExtension(path);

			var obj = AssetDatabase.LoadAssetAtPath(path, typeof(DataTableEditorSettings));
			if (obj is DataTableEditorSettings)
			{
				settings = obj as DataTableEditorSettings;
				return;
			}
		}
	}
	public void LoadAllJson()
	{
		jsonContainer = new Dictionary<Type, object>();
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
			if (LoadFromBinary(file) == false)
			{
				LoadFromNonBinary(file);
			}

		}
	}

	private bool LoadFromBinary(string file)
	{
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
					jsonContainer.Add(t, jsonData);
				}
				catch (Exception e)
				{
					return false;
				}
			}
		}
		return true;
	}

	private bool LoadFromNonBinary(string file)
	{

		string json = File.ReadAllText(file);

		string name = System.IO.Path.GetFileNameWithoutExtension(file);

		System.Type t = System.Type.GetType($"{name}, Assembly-CSharp");


		try
		{
			var jsonData = JsonUtility.FromJson(json, t);
			jsonContainer.Add(t, jsonData);
		}
		catch (Exception e)
		{
			return false;
		}
		return true;
	}

	void OnGUI()
	{
		LoadSettings();
		EditorGUILayout.Space(10);
		label = EditorGUILayout.TextField("데이터 테이블 이름", label);
		if (label.IsNullOrEmpty())
		{
			EditorGUILayout.Space(3);
			if (GUILayout.Button("Load Binary Json"))
			{
				FromJson(true);
			}
			if (GUILayout.Button("Load Non Binary Json"))
			{
				FromJson(false);
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
				FromJson(true);
			}
		}
		else
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			if (GUILayout.Button("Save Json", EditorStyles.toolbarButton))
			{
				ToJson(true);
			}
			if (GUILayout.Button("Load Json", EditorStyles.toolbarButton))
			{
				FromJson(true);
			}
			if (GUILayout.Button("Load All Json", EditorStyles.toolbarButton))
			{
				LoadAllJson();
			}

			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			if (GUILayout.Button("Save Non Binary Json", EditorStyles.toolbarButton))
			{
				ToJson(false);
			}
			if (GUILayout.Button("Load Non Binary Json", EditorStyles.toolbarButton))
			{
				FromJson(false);
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			if (GUILayout.Button("Verify Data Tables", EditorStyles.toolbarButton))
			{
				VerifyTidDuplicate();
			}

			GUILayout.EndHorizontal();


			if (GUILayout.Button("Convert To CSV"))
			{
				ConvertToCSV();
			}
			if (GUILayout.Button("Convert From Json To CSV"))
			{
				ConvertJsonToCSV();

			}
			if (GUILayout.Button("Load From Csv"))
			{
				ConvertFromCsvToData();
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
	SerializedProperty infosProperty;
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
			Type tableType = targetObje.GetType().GetField("dataSheet").FieldType;

			Type rawDataType = System.Type.GetType($"{infosProperty.arrayElementType}, Assembly-CSharp");
			FieldInfo[] fields = rawDataType.GetFields();

			float width = fields.Length * (settings.cellSize.x + settings.rowSpace) + 34;

			scrollPos = GUILayout.BeginScrollView(scrollPos);

			dataSheetProperty.FindPropertyRelative("typeName").stringValue = tableType.Name;

			if (tableType.Name.Contains("ResultCode", StringComparison.Ordinal))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField($"데이터 타입: {tableType.Name}", GUILayout.Width(width / 2));
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
			}

			DrawHeader(width);

			maleeReorderableList.DoLayoutList();//DoList(new Rect(0, EditorGUIUtility.singleLineHeight + 10, width, height + 40), GUIContent.none);

			GUILayout.EndScrollView();
		}

		EditorGUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();

		EditorUtility.SetDirty(scriptableObject);
	}

	private void DrawHeader(float width)
	{
		EditorGUILayout.BeginHorizontal();
		addArraySize = EditorGUILayout.IntField("리스트 개수 ", addArraySize, GUILayout.Width(width - 300));
		if (GUILayout.Button("리스트 추가", GUILayout.Width(130)))
		{
			infosProperty.arraySize += addArraySize;
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
		pageSize = EditorGUILayout.IntField("한 페이지에 표시할 데이터 개수 ", pageSize, GUILayout.Width(width - 300));

	}
	private void DrawToolTip()
	{
		GUIStyle boxStyle = new GUIStyle(GUI.skin.GetStyle("HelpBox"));
		boxStyle.richText = true;
		boxStyle.fontSize = 12;
		StringBuilder sb = new StringBuilder();
		sb.Append("알림");
		EditorGUILayout.TextArea("", boxStyle);
	}

	private void CreateReorderableList()
	{
		if (serializedObject == null)
		{
			return;
		}
		dataSheetProperty = serializedObject.FindProperty("dataSheet");
		infosProperty = dataSheetProperty.FindPropertyRelative("infos");


		maleeReorderableList = ReorderableListGenerator.Init(settings, serializedObject, infosProperty).SetLoadedData(jsonContainer).Build(10, true);
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

		AssetDatabase.CreateAsset(newDataObject, $"Assets/DataSheetObject/{label}Object.asset");
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
			"Assets/DataSheetObject",
			"Assets/Resources"
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
		maleeReorderableList = null;
		serializedObject = null;
		scriptableObject = null;
		return false;
	}


	void ToJson(bool isBinary)
	{
		var targetObje = serializedObject.FindProperty("dataSheet").serializedObject.targetObject;

		System.Reflection.FieldInfo info = targetObje.GetType().GetField("dataSheet");
		var sd = info.GetValue(targetObje);

		if (VerifyTidDuplicate(sd) == false)
		{

		}
		if (currentJsonFilePath.IsNullOrEmpty())
		{
			currentJsonFilePath = $"{Application.dataPath}/AssetFolder/Resources/Json";
		}
		if (currentJsonFileName.IsNullOrEmpty())
		{
			currentJsonFileName = "";
		}
		string rootPath = Path.GetDirectoryName(currentJsonFilePath);
		string path = EditorUtility.SaveFilePanel("", rootPath, currentJsonFileName, "json");

		if (path.IsNullOrEmpty())
		{
			return;
		}

		string json = JsonUtility.ToJson(sd);

		if (isBinary)
		{
			using (FileStream fs = File.Create(path))
			{
				using (BinaryWriter sw = new BinaryWriter(fs))
				{
					sw.Write(json);
				}
			}
		}
		else
		{
			File.WriteAllText(path, json);
		}

		currentJsonFileName = Path.GetFileName(path);
		currentJsonFilePath = path;
		AssetDatabase.Refresh();
	}

	void FromJson(bool isBinary)
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

		string fileName = Path.GetFileNameWithoutExtension(path);
		string typeName = fileName;

		object json = JsonConverter.ToData(isBinary, path);
		if (json != null)
		{
			label = typeName;

			if (GetScriptableObject(typeName))
			{
				if (TypeExist())
				{

					instanceType.GetField("dataSheet").SetValue(scriptableObject, json);
					serializedObject = new SerializedObject(scriptableObject);
				}
			}
		}


		CreateReorderableList();
		EditorGUI.FocusTextInControl(null);
		currentJsonFileName = Path.GetFileName(path);
		currentJsonFilePath = path;
	}

	private void CollectDataList(object currentData, Dictionary<long, List<string>> duplicatedDatas)
	{
		string typename = "";
		long tid = 0;
		foreach (var table in jsonContainer)
		{

			if (currentData != null && currentData.GetType() == table.Key)
			{
				continue;
			}

			//현재 편집 중인 데이터이면 따로 추가 한다.

			IList list = (IList)table.Key.GetField("infos").GetValue(table.Value);

			foreach (var item in list)
			{
				tid = (long)item.GetType().GetField("tid").GetValue(item);
				typename = item.GetType().Name;

				if (duplicatedDatas.ContainsKey(tid) == false)
				{
					duplicatedDatas.Add(tid, new List<string>());
				}
				duplicatedDatas[tid].Add(typename);
			}
		}
	}

	private bool VerifyTidDuplicate(object currentData = null)
	{
		bool verified = true;
		Dictionary<long, List<string>> duplicatedDatas = new Dictionary<long, List<string>>();

		CollectDataList(currentData, duplicatedDatas);

		StringBuilder sb = new StringBuilder();

		foreach (var item in duplicatedDatas)
		{
			if (item.Value.Count > 1)
			{
				sb.Append($"Tid : {item.Key}");
				sb.Append($"\nTable : \n");
				for (int i = 0; i < item.Value.Count; i++)
				{
					sb.Append($"{item.Value[i]}");
					sb.Append("\n");
				}
				sb.Append("\n");
				verified = false;
			}
		}

		//EditorUtility.DisplayDialog("중복 데이터", sb.ToString(), "확인");
		return verified;
	}

	void ConvertToCSV()
	{
		var targetObje = serializedObject.FindProperty("dataSheet").serializedObject.targetObject;

		string csv = CsvConverter.FromData(targetObje);

		StreamWriter streamWriter = File.CreateText($"{Application.dataPath}/AssetFolder/test.csv");
		streamWriter.Write(csv);
		streamWriter.Close();
		AssetDatabase.Refresh();
	}

	void ConvertJsonToCSV()
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

		object json = JsonConverter.ToData(false, path);
		if (json != null)
		{
			string csv = CsvConverter.FromJson(json);
			StreamWriter streamWriter = File.CreateText($"{Application.dataPath}/AssetFolder/test.csv");
			streamWriter.Write(csv);
			streamWriter.Close();
		}
		EditorGUI.FocusTextInControl(null);
		AssetDatabase.Refresh();
	}

	void ConvertFromCsvToData()
	{
		string rootPath = Path.GetDirectoryName($"{Application.dataPath}/AssetFolder/");
		string path = EditorUtility.OpenFilePanel("", rootPath, "csv");

		if (path.IsNullOrEmpty())
		{
			return;
		}
		var data = CsvConverter.ToData(path);


		if (data != null)
		{
			string typeName = data.GetType().GetField("typeName").GetValue(data).ToString();
			label = typeName;

			if (GetScriptableObject(typeName))
			{
				if (TypeExist())
				{

					instanceType.GetField("dataSheet").SetValue(scriptableObject, data);
					serializedObject = new SerializedObject(scriptableObject);
				}
			}
		}

		CreateReorderableList();
		EditorGUI.FocusTextInControl(null);
		currentJsonFileName = Path.GetFileName(path);
		currentJsonFilePath = path;
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
				sb.AppendLine("\t[SerializeField]");
				sb.AppendLine($"\tpublic {label} dataSheet;");
				sb.AppendLine("}");

				writer.Write(sb.ToString());
			}
		}

		AssetDatabase.Refresh();
	}
}
