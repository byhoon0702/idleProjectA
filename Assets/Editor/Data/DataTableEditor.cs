using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;



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
	[SerializeField]
	public SerializedProperty dataSheetProperty;
	[MenuItem("Custom Menu/DataEditor/DataTableEditor", false, 5000)]
	public static void Init()
	{
		var window = EditorWindow.GetWindow<DataTableEditor>();
		window.minSize = new Vector2(330f, 360f);

		window.Show();
	}

	private void OnDestroy()
	{

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

	void OnGUI()
	{
		label = EditorGUILayout.TextField("데이터 테이블 이름", label);

		if (GetScriptableObject() == false)
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
		}
		else
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Save Json"))
			{
				ToJson();
			}
			if (GUILayout.Button("Load Json"))
			{
				FromJson();
			}
			EditorGUILayout.EndHorizontal();
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
			CreateReorderableList();
		}

		serializedObject.Update();



		EditorGUILayout.PropertyField(dataSheetProperty);


		//reorderableList.DoLayoutList();


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
		return;
		var property = dataSheetProperty.FindPropertyRelative("infos");
		reorderableList = new ReorderableList(dataSheetProperty.serializedObject, property, true, true, true, true);
		reorderableList.drawElementCallback = DrawReorderableList;
		reorderableList.drawHeaderCallback = DrawHeader;


	}
	void DrawHeader(Rect rect)
	{
		reorderableList.headerHeight = EditorGUIUtility.singleLineHeight * 2;
		EditorGUI.LabelField(rect, "Header");
		rect.height += EditorGUIUtility.singleLineHeight;
		EditorGUI.LabelField(rect, "Header");
	}


	void DrawReorderableList(Rect rect, int index, bool isActive, bool isFocused)
	{
		SerializedProperty info = reorderableList.serializedProperty.GetArrayElementAtIndex(index);

		EditorGUI.BeginChangeCheck()
			 ;
		Type type = System.Type.GetType($"{info.type}, Assembly-CSharp");

		var fields = type.GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			rect.width = (EditorGUIUtility.currentViewWidth / fields.Length) - 10;
			rect.height = EditorGUIUtility.singleLineHeight;
			var sf = info.FindPropertyRelative(fields[i].Name);

			if (sf.isExpanded)
			{
				rect.height += EditorGUIUtility.singleLineHeight;
			}

			EditorGUI.PropertyField(rect, sf, GUIContent.none);

			rect.x += rect.width + 5;
		}

		if (EditorGUI.EndChangeCheck())
		{
			info.serializedObject.ApplyModifiedProperties();
		}

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

	bool GetScriptableObject()
	{
		if (label.IsNullOrEmpty())
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
			if (filename.ToLower() == $"{label}Object".ToLower())
			{
				scriptableObject = (ScriptableObject)AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject));
				searchString = filename;
				return true;
			}
		}

		return false;
	}

	void ToJson()
	{
		var targetObje = serializedObject.FindProperty("dataSheet").serializedObject.targetObject;


		System.Reflection.FieldInfo info = targetObje.GetType().GetField("dataSheet");
		var sd = info.GetValue(targetObje);


		string path = EditorUtility.SaveFilePanel("", Application.dataPath + "/AssetFolder/Resources/Json", label, "json");

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
	}

	void FromJson()
	{
		string path = EditorUtility.OpenFilePanel("", Application.dataPath + "/AssetFolder/Resources/Json", "json");

		if (path.IsNullOrEmpty())
		{
			return;
		}
		using (FileStream fs = File.OpenRead(path))
		{
			string fileName = Path.GetFileNameWithoutExtension(path);
			string[] fileNames = fileName.Split('_');

			string typeName = fileName;

			if (fileNames.Length > 1)
			{
				typeName = fileNames[0];
			}

			using (BinaryReader sr = new BinaryReader(fs))
			{
				Type type = System.Type.GetType($"{typeName}, Assembly-CSharp");
				if (type == null)
				{
					sr.Close();
				}

				label = typeName;
				var json = JsonUtility.FromJson(sr.ReadString(), type);
				instanceType.GetField("dataSheet").SetValue(scriptableObject, json);

				serializedObject = null;
				serializedObject = new SerializedObject(scriptableObject);
				CreateReorderableList();
				EditorGUI.FocusTextInControl(null);
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

	//void GenerateCode()
	//{
	//	if (variableList.Count == 0)
	//	{
	//		return;
	//	}

	//	float i = 0;

	//	string path = string.Concat(Application.dataPath, Path.DirectorySeparatorChar, $"DataClass/{className}.cs");
	//	using (FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
	//	{
	//		using (StreamWriter writer = new StreamWriter(fs))
	//		{
	//			StringBuilder sb = new StringBuilder();

	//			sb.AppendLine($"//========AUTO GENERATED CODE======//");
	//			sb.AppendLine($"public class {className}");
	//			sb.AppendLine("{");
	//			// AppDomain.CurrentDomain.GetAssemblies();

	//			foreach (var variable in variableList)
	//			{
	//				System.Type dataType = null;
	//				if (typeList.ContainsKey(variable.dataType))
	//				{
	//					dataType = typeList[variable.dataType];
	//				}
	//				else
	//				{
	//					dataType = ConvertStringToType(variable.dataType);
	//				}

	//				if (dataType == null)
	//				{
	//					Debug.LogError("올바르지 않은 자료형");

	//					return;
	//				}
	//				if (variable.variableName.IsNullOrEmpty())
	//				{
	//					Debug.LogError("변수 이름이 비어있음");

	//					return;
	//				}
	//				if (variable.variableName[0] >= '0' && variable.variableName[0] <= '9')
	//				{
	//					Debug.LogError("변수 이름이 숫자로 시작 하면 안됨");

	//					return;
	//				}
	//				sb.AppendLine($"\tpublic {variable.dataType} {variable.variableName};");

	//				//EditorUtility.DisplayProgressBar("Please Wait...", "", i / variableList.Count);
	//				//i++;
	//			}

	//			sb.AppendLine("}");

	//			writer.Write(sb.ToString());


	//		}
	//	}
	//}

	private Type ConvertStringToType(string name)
	{

		var assemblise = System.AppDomain.CurrentDomain.GetAssemblies();

		foreach (var assembly in assemblise)
		{
			foreach (var type in assembly.GetTypes())
			{
				if (type.Name == name)
				{
					return type;
				}

			}
		}
		return null;
	}
}
