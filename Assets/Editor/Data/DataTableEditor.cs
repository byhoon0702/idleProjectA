using System;
using System.Collections.Generic;

using System.IO;
using System.Text;

using UnityEditor;
using UnityEngine;

using System.Collections;
using Unity.VisualScripting;

[Serializable]
public class ContainedData
{
	[SerializeField]
	public System.Type type;
	[SerializeField]
	public object data;
}
[Serializable]
public class DataContainer
{
	public List<ContainedData> dataContainer;

	public DataContainer()
	{

		dataContainer = new List<ContainedData>();
	}

	~DataContainer()
	{
		dataContainer.Clear();
		dataContainer = null;
	}

	public void Add(ContainedData data)
	{
		dataContainer.Add(data);
	}
	public ContainedData Find(System.Type type)
	{
		for (int i = 0; i < dataContainer.Count; i++)
		{
			var data = dataContainer[i];
			if (data.type == type)
			{
				return data;
			}
		}
		return null;
	}

	public List<ContainedData> FindAll(System.Type type)
	{
		List<ContainedData> datas = new List<ContainedData>();
		for (int i = 0; i < dataContainer.Count; i++)
		{
			var data = dataContainer[i];
			if (data.type == type)
			{
				datas.Add(data);
			}
		}
		return datas;
	}

}


public partial class DataTableEditor : EditorWindow
{
	public DataTableEditorSettings settings;
	[SerializeField]
	public string className;

	[SerializeField]
	public SerializedObject serializedObject;

	[SerializeField]
	public ScriptableObject scriptableObject;

	[SerializeField] private Malee.List.ReorderableList maleeReorderableList;
	private string label;
	private string searchString;
	private Type instanceType;
	private string currentJsonFileName;
	private string currentJsonFilePath;

	private DataContainer dataContainer;

	[SerializeField]
	public SerializedProperty dataSheetProperty;

	private int addArraySize = 0;
	private int pageSize = 10;

	SerializedProperty infosProperty;

	private int pageIndex = 0;
	private string[] pageNames = new string[]
{
		"데이터 리스트",
		"데이터 편집",
		"TID 리스트"
};


	private SerializedProperty prefixIDProperty;

	private const string prefixChangeDescription = "해당 프리픽스의 0인 자릿수 만큼을 교체. ex) prefix: 1001000 , tid :  1012003 , result : 1001003 , 단 일의 자리수가 0이 아닌경우 교체 안됨";
	private const string prefixAddDescription = "해당 프리픽스 만큼 현재 TID 에서 더함";
	private const string prefixMinusDescription = "해당 프리픽스 만큼 현재 TID 에서 뺌";
	public const long minTidPrefix = 1000000;


	public event Action OnSave;
	[MenuItem("Custom Menu/DataEditor/DataTableEditor", false, 0)]
	public static void Init()
	{
		var window = EditorWindow.CreateWindow<DataTableEditor>(new Type[] { typeof(DataTableEditor), });
		window.minSize = new Vector2(330f, 360f);

		window.Show();
		window.SetEvent();

	}

	public void SetEvent()
	{
		OnSave += () =>
		{
			ToJson();
			LoadAllDataPath("Json");
			Repaint();
		};
	}


	private void OnDestroy()
	{
		OnSave = null;
		if (datalistforDataListPage != null)
		{
			datalistforDataListPage.Clear();
			datalistforDataListPage = null;
		}


		if (filePaths != null)
		{
			filePaths.Clear();
			filePaths = null;
		}


		if (dataContainer != null)
		{
			dataContainer = null;
		}

		maleeReorderableList = null;

		Resources.UnloadAsset(scriptableObject);
		Resources.UnloadAsset(settings);

		Resources.UnloadUnusedAssets();
		System.GC.Collect();
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

	public void LoadAllDataPath(string path)
	{

		datalistforDataListPage = new Dictionary<string, DataListInfo>();
		filePaths = new List<string>();
		string[] files = Directory.GetFiles(Application.dataPath + $"/AssetFolder/Resources/Data/{path}");

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

			filePaths.Add(file);
			string fileName = Path.GetFileNameWithoutExtension(file);
			if (path == "Json")
			{
				datalistforDataListPage.Add(fileName, new DataListInfo() { path = file, data = JsonConverter.ToData(file) });
			}
			else
			{
				datalistforDataListPage.Add(fileName, new DataListInfo() { path = file, data = CsvConverter.ToData(file) });
			}
		}

	}


	public void LoadAllData(string path)
	{
		dataContainer = new DataContainer();
		for (int i = 0; i < filePaths.Count; i++)
		{
			string file = filePaths[i];
			if (path == "Json")
			{
				LoadFromJson(file);
			}

			if (path == "Csv")
			{
				LoadFromCSV(file);
			}
		}
	}

	public void LoadAllJson()
	{

		dataContainer = new DataContainer();
		string[] files = Directory.GetFiles(Application.dataPath + "/AssetFolder/Resources/Data/Json");

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
			LoadFromJson(file);
		}
	}

	private bool LoadFromJson(string file)
	{

		string name = System.IO.Path.GetFileNameWithoutExtension(file);

		System.Type t = name.GetAssemblyType();

		try
		{
			var jsonData = JsonConverter.ToData(file);
			ContainedData containData = new ContainedData();
			containData.type = t;
			containData.data = jsonData;
			dataContainer.Add(containData);

		}
		catch (Exception e)
		{
			return false;
		}
		return true;
	}

	private bool LoadFromCSV(string file)
	{
		string name = System.IO.Path.GetFileNameWithoutExtension(file);

		System.Type t = name.GetAssemblyType();

		try
		{
			var jsonData = CsvConverter.ToData(file);
			ContainedData containData = new ContainedData();
			containData.type = t;
			containData.data = jsonData;
			dataContainer.Add(containData);

		}
		catch (Exception e)
		{
			return false;
		}
		return true;
	}

	private void UpdateKeyInput()
	{
		var e = Event.current;
		if (e?.isKey == true)
		{
			if (e.CtrlOrCmd())
			{
				switch (e.type)
				{
					case EventType.KeyDown:
						{
							if (e.keyCode == KeyCode.S)
							{
								if (save == false)
								{
									Debug.Log("Save");
									ToJson();
									LoadAllDataPath("Json");
									Repaint();

									EditorUtility.SetDirty(scriptableObject);
									serializedObject.ApplyModifiedProperties();

									save = true;
								}
							}
						}
						break;

					case EventType.KeyUp:
						{
							save = false;
						}
						break;
				}
			}
		}
	}


	void OnGUI()
	{
		LoadSettings();

		EditorGUILayout.Space(10);
		if (GUILayout.Button("TID 규칙"))
		{
			Application.OpenURL("https://docs.google.com/spreadsheets/d/1GRH90StHBWwtqGP3OsufETLJONKXJ6TKFoEbtC-pABY/edit#gid=864885465");
		}
		pageIndex = GUILayout.Toolbar(pageIndex, pageNames);

		EditorGUILayout.Space(3);
		if (pageIndex == 0)
		{
			DrawDataList();
		}
		else if (pageIndex == 1)
		{
			DrawDataEdit();
		}
		else if (pageIndex == 2)
		{
			DrawTidList();
		}
		EditorUtility.SetDirty(this);

		UpdateKeyInput();
	}

	private bool save;

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
		System.Type type = $"{name}Object".GetAssemblyType();
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

	void ToJson()
	{
		var targetObje = serializedObject.FindProperty("dataSheet").serializedObject.targetObject;

		System.Reflection.FieldInfo info = targetObje.GetType().GetField("dataSheet");
		var sd = info.GetValue(targetObje);

		if (VerifyTidDuplicate(sd) == false)
		{

		}
		if (currentJsonFilePath.IsNullOrEmpty())
		{
			currentJsonFilePath = $"{Application.dataPath}/AssetFolder/Resources/Data/Json/{label}.json";
		}
		if (currentJsonFileName.IsNullOrEmpty())
		{
			currentJsonFileName = "";
		}
		if (currentJsonFileName.Contains(label) == false)
		{
			currentJsonFileName = $"{label}.json";
			currentJsonFilePath = $"{Application.dataPath}/AssetFolder/Resources/Data/Json/{label}.json";
		}

		if (currentJsonFilePath.Contains(".csv"))
		{
			currentJsonFilePath = $"{Application.dataPath}/AssetFolder/Resources/Data/Json/{label}.json";
		}
		currentJsonFileName = currentJsonFileName.Replace(".csv", ".json");

		string rootPath = Path.GetDirectoryName(currentJsonFilePath);
		string path = currentJsonFilePath;

		JsonConverter.FromData(sd, path);
		//string json = JsonUtility.ToJson(sd, true);
		//File.WriteAllText(path, json);

		currentJsonFileName = Path.GetFileName(path);
		currentJsonFilePath = path;

		//AssetDatabase.Refresh();
	}


	void FromJson(bool openFilePanel = true)
	{

		string rootPath = Path.GetDirectoryName($"{Application.dataPath}/AssetFolder/Resources/Data/Json/.json");
		string path = currentJsonFilePath;
		if (openFilePanel)
		{
			path = EditorUtility.OpenFilePanel("", rootPath, "json");

			if (path.IsNullOrEmpty())
			{
				return;
			}

		}

		string fileName = Path.GetFileNameWithoutExtension(path);
		string typeName = fileName;

		object json = JsonConverter.ToData(path);
		if (json != null)
		{
			System.Type type = json.GetType();

			label = fileName;
			typeName = (string)type.GetField("typeName").GetValue(json);
			if (GetScriptableObject(typeName))
			{
				if (TypeExist())
				{
					instanceType.GetField("dataSheet").SetValue(scriptableObject, null);
					instanceType.GetField("dataSheet").SetValue(scriptableObject, json);
					serializedObject = new SerializedObject(scriptableObject);
				}
			}
		}
		LoadAllDataPath("Json");
		LoadAllData("Json");


		CreateReorderableList();
		EditorGUI.FocusTextInControl(null);
		currentJsonFileName = Path.GetFileName(path);
		currentJsonFilePath = path;
	}

	private void CollectDataList(object currentData, Dictionary<long, List<string>> duplicatedDatas)
	{
		string typename = "";
		long tid = 0;
		foreach (var table in dataContainer.dataContainer)
		{

			if (currentData != null && currentData.GetType() == table.type)
			{
				continue;
			}

			//현재 편집 중인 데이터이면 따로 추가 한다.

			IList list = (IList)table.type.GetField("infos").GetValue(table.data);

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
		return true;
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

	void ToCSV()
	{
		var targetObje = serializedObject.FindProperty("dataSheet").serializedObject.targetObject;

		string csv = CsvConverter.FromData(targetObje);
		if (currentJsonFilePath.IsNullOrEmpty())
		{
			currentJsonFilePath = $"{Application.dataPath}/AssetFolder/Resources/Data/Csv/{label}.csv";
		}
		if (currentJsonFileName.IsNullOrEmpty())
		{
			currentJsonFileName = "";
		}

		if (currentJsonFilePath.Contains(".json"))
		{
			currentJsonFilePath = $"{Application.dataPath}/AssetFolder/Resources/Data/Csv/{label}.csv";
		}
		currentJsonFileName = currentJsonFileName.Replace(".json", ".csv");

		using (StreamWriter streamWriter = new StreamWriter(currentJsonFilePath, false, System.Text.Encoding.UTF8))
		{
			streamWriter.Write(csv);
			streamWriter.Close();
		}

		AssetDatabase.Refresh();
	}

	void ConvertJsonToCSV()
	{
		if (currentJsonFilePath.IsNullOrEmpty())
		{
			currentJsonFilePath = $"{Application.dataPath}/AssetFolder/Resources/Data/Json";
		}
		string rootPath = Path.GetDirectoryName(currentJsonFilePath);
		string path = EditorUtility.OpenFilePanel("", rootPath, "json");

		if (path.IsNullOrEmpty())
		{
			return;
		}

		object json = JsonConverter.ToData(path);
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

	void FromCSV(bool openFilePanel = true)
	{
		string rootPath = Path.GetDirectoryName($"{Application.dataPath}/AssetFolder/Resources/Data/Csv/.csv");
		string path = currentJsonFilePath;

		if (openFilePanel)
		{
			path = EditorUtility.OpenFilePanel("", rootPath, "csv");

			if (path.IsNullOrEmpty())
			{
				return;
			}
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
					instanceType.GetField("dataSheet").SetValue(scriptableObject, null);
					instanceType.GetField("dataSheet").SetValue(scriptableObject, data);
					serializedObject = new SerializedObject(scriptableObject);
				}
			}
		}
		LoadAllDataPath("Csv");
		LoadAllData("Csv");
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
				sb.AppendLine("#if UNITY_EDITOR");
				sb.AppendLine("using UnityEditor;");
				sb.AppendLine("#endif");
				sb.AppendLine("[Serializable]");
				sb.AppendLine($"public class {label}Object : BaseDataSheetObject ");
				sb.AppendLine("{");
				sb.AppendLine("\t[SerializeField]");
				sb.AppendLine($"\tpublic {label} dataSheet;");

				sb.AppendLine("public override void Call()");
				sb.AppendLine("{");
				sb.AppendLine("#if UNITY_EDITOR");
				sb.AppendLine("#endif");
				sb.AppendLine("}");

				sb.AppendLine("}");

				writer.Write(sb.ToString());
			}
		}

		AssetDatabase.Refresh();
	}

	//void ToJsonBinary()
	//{
	//	var targetObje = serializedObject.FindProperty("dataSheet").serializedObject.targetObject;

	//	System.Reflection.FieldInfo info = targetObje.GetType().GetField("dataSheet");
	//	var sd = info.GetValue(targetObje);

	//	if (VerifyTidDuplicate(sd) == false)
	//	{

	//	}
	//	if (currentJsonFilePath.IsNullOrEmpty())
	//	{
	//		currentJsonFilePath = $"{Application.dataPath}/AssetFolder/Resources/Json/{label}.bytes";
	//	}
	//	if (currentJsonFileName.IsNullOrEmpty())
	//	{
	//		currentJsonFileName = "";
	//	}
	//	string rootPath = Path.GetDirectoryName(currentJsonFilePath);

	//	string path = EditorUtility.SaveFilePanel("", rootPath, currentJsonFileName, "bytes");

	//	if (path.IsNullOrEmpty())
	//	{
	//		return;
	//	}

	//	string json = JsonUtility.ToJson(sd);

	//	using (FileStream fs = File.Create(path))
	//	{
	//		using (BinaryWriter sw = new BinaryWriter(fs))
	//		{
	//			sw.Write(json);
	//		}
	//	}

	//	currentJsonFileName = Path.GetFileName(path);
	//	currentJsonFilePath = path;
	//	AssetDatabase.Refresh();

	//}


}
