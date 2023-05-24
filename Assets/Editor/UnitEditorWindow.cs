using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.DemiEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditorInternal;
using System.Runtime.InteropServices.WindowsRuntime;

[System.Serializable]
public class FakeUnitObject : ScriptableObject
{
	public UnitData data;

}

[System.Serializable]
public class UnitEditorWindow : EditorWindow
{
	private GameObject gameObject;
	private string newName;

	private string objectSavePath;
	[SerializeField]
	private DataContainer dataContainer;
	[SerializeField]
	private UnitDataSheet unitDataSheet;
	[SerializeField]
	private FakeUnitObject fakeUnitObject;


	private string[] unitNameArray;
	[SerializeField]
	private int selectUnitIndex = 0;
	[SerializeField]
	private UnitData currentData = null;
	[SerializeField]
	public Dictionary<string, LinkedData> linkedDataDic;
	private LinkedTypeContainer linkedTypeContainer;
	private SerializedObject fakeobj;

	UnityEditorInternal.ReorderableList reorderableList;

	[MenuItem("Tools/GameObject Editor", false, 84)]
	public static void ShowWindow()
	{
		GetWindow<UnitEditorWindow>();
	}


	private void OnGUI()
	{

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("window", GUILayout.MaxWidth(300));
		DrawObjectPanel();

		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical("window");
		DrawFilePanel();

		DrawUnitDataPanel();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
	}


	private void LoadData()
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
		catch (System.Exception e)
		{
			return false;
		}
		return true;
	}

	private void CreateLinkedTypeList()
	{
		//if (linkedTypeContainer == null)
		{
			linkedTypeContainer = new LinkedTypeContainer();
			System.Type unitDataType = typeof(UnitData);

			var fields = unitDataType.GetFields();
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

					linkedTypeContainer.Add(fields[i].Name, new List<System.Type>() { type });
				}
			}
		}
	}
	private void CreateLinkedData()
	{

		linkedDataDic = new Dictionary<string, LinkedData>();
		for (int i = 0; i < linkedTypeContainer.container.Count; i++)
		{
			string fieldName = linkedTypeContainer.container[i].fieldName;
			var linkType = linkedTypeContainer.Find(fieldName);
			if (linkType.type == null)
			{
				linkedTypeContainer = null;
				linkedDataDic = null;
				continue;
			}
			List<long> tids = new List<long>();
			List<string> names = new List<string>();
			for (int ii = 0; ii < linkType.type.Count; ii++)
			{
				var containedData = dataContainer.Find(linkType.type[ii]);

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
						for (int iii = 0; iii < list.Count; iii++)
						{
							object item = list[iii];

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

	void DataReset()
	{
		LoadData();
		CreateLinkedTypeList();
		CreateLinkedData();

		unitDataSheet = (UnitDataSheet)dataContainer.Find(typeof(UnitDataSheet)).data;
		currentData = unitDataSheet.infos[selectUnitIndex];
		fakeUnitObject = null;
		fakeUnitObject = ScriptableObject.CreateInstance<FakeUnitObject>();
		fakeUnitObject.data = currentData;
		SerializedObject obj = new SerializedObject(fakeUnitObject);
		fakeobj = obj;
	}

	void DrawFilePanel()
	{


		if (linkedDataDic == null)
		{
			LoadData();
			CreateLinkedTypeList();
			CreateLinkedData();
		}
		if (unitDataSheet == null)
		{
			unitDataSheet = (UnitDataSheet)dataContainer.Find(typeof(UnitDataSheet)).data;

			unitNameArray = new string[unitDataSheet.infos.Count];

			for (int i = 0; i < unitDataSheet.infos.Count; i++)
			{
				string name = unitDataSheet.infos[i].name;
				unitNameArray[i] = $"{name}";
			}
		}

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Json 저장"))
		{
			Save();
		}

		if (GUILayout.Button("데이터 초기화"))
		{
			DataReset();
		}

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField("Data");
		EditorGUI.BeginChangeCheck();
		selectUnitIndex = EditorGUILayout.Popup("캐릭터", selectUnitIndex, unitNameArray);
		if (selectUnitIndex < 0)
		{
			selectUnitIndex = 0;
		}
		if (selectUnitIndex > unitDataSheet.infos.Count - 1)
		{
			selectUnitIndex = unitDataSheet.infos.Count - 1;

		}
		currentData = unitDataSheet.infos[selectUnitIndex];
		if (EditorGUI.EndChangeCheck() || fakeUnitObject == null)
		{
			fakeUnitObject = null;
			fakeUnitObject = ScriptableObject.CreateInstance<FakeUnitObject>();
			fakeUnitObject.data = currentData;
			SerializedObject obj = new SerializedObject(fakeUnitObject);
			fakeobj = obj;

			string resourceName = fakeobj.FindProperty("data").FindPropertyRelative("resource").stringValue;

			string resourcePath = "B";
			gameObject = (GameObject)Resources.Load($"{resourcePath}/{resourceName}");

			reorderableList = null;
		}

		if (fakeUnitObject != null && fakeobj == null)
		{
			SerializedObject obj = new SerializedObject(fakeUnitObject);
			fakeobj = obj;
		}

	}
	void DrawObjectPanel()
	{
		EditorGUILayout.BeginVertical("window", GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));

		objectSavePath = EditorGUILayout.TextField("경로", objectSavePath);

		gameObject = (GameObject)EditorGUILayout.ObjectField("프리팹", gameObject, typeof(GameObject), false);

		if (gameObject != null)
		{
			string path = AssetDatabase.GetAssetPath(gameObject);
			if (path.Contains(".prefab"))
			{
				if (GUILayout.Button("Open Prefab"))
				{
					OpenPrefab();
				}
				if (GUILayout.Button("Instantiate Prefab"))
				{
					InstantiatePrefabIntoScene();
				}
			}
			else
			{
				newName = EditorGUILayout.TextField("프리팹 이름", newName);
				if (newName.IsNullOrEmpty() == false && gameObject.name != newName)
				{
					EditorGUILayout.HelpBox("이름이 다른 경우 새로 생성, 이름이 같을 경우 덮어쓰기", MessageType.Info);
				}

				if (GUILayout.Button("Create Prefab"))
				{
					SavePrefab();
				}
			}
		}

		EditorGUILayout.EndVertical();
	}

	public void Save()
	{
		if (currentData == null)
		{
			return;
		}
		var data = unitDataSheet.GetData(currentData.tid);
		if (data == null)
		{
			unitDataSheet.AddData(currentData);
		}
		else
		{
			unitDataSheet.SetData(currentData.tid, currentData);
		}


		string path = $"{Application.dataPath}/AssetFolder/Resources/Data/Json/UnitDataSheet.json";
		JsonConverter.FromData(unitDataSheet, path);
	}

	Vector2 scrollPos;
	void DrawUnitDataPanel()
	{
		EditorGUILayout.BeginVertical("window");



		System.Type type = typeof(UnitData);
		System.Type fakeType = typeof(FakeUnitObject);
		if (fakeobj != null)
		{
			int index = 0;


			fakeobj.Update();

			SerializedProperty dataProperty = fakeobj.FindProperty("data");
			object data = fakeType.GetField("data").GetValue(fakeobj.targetObject);

			EditorGUILayout.LabelField("tid", type.GetValue<long>("tid", data).ToString());
			//type.SetValue("tid", data, tid);

			string description = EditorGUILayout.TextField("description", type.GetValue<string>("description", data));
			type.SetValue("description", data, description);

			string name = EditorGUILayout.TextField("name", type.GetValue<string>("name", data));
			type.SetValue("name", data, name);

			string resource = EditorGUILayout.TextField("resource", type.GetValue<string>("resource", data));
			type.SetValue("resource", data, resource);

			DrawLinkTidGUILayoutField(type, "skillEffectTidNormal", index, data);

			Grade grade = (Grade)EditorGUILayout.EnumPopup("grade", type.GetValue<Grade>("grade", data));
			type.SetValue("grade", data, grade);

			int star = EditorGUILayout.IntField("starlevel", type.GetValue<int>("starlevel", data));
			type.SetValue("starlevel", data, star);

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			DrawStatusListField(fakeobj, dataProperty.FindPropertyRelative("statusDataList"), data);
			EditorGUILayout.EndScrollView();

			DrawLinkTidGUILayoutField(type, "skillTid", index, data);
			DrawLinkTidGUILayoutField(type, "finalSkillTid", index, data);

			EditorUtility.SetDirty(fakeobj.targetObject);
			fakeobj.ApplyModifiedProperties();
		}
		EditorGUILayout.EndVertical();
	}


	private void DrawStatusListField(SerializedObject so, SerializedProperty property, object data)
	{
		if (reorderableList == null)
		{
			reorderableList = new ReorderableList(so, property);


			reorderableList.drawHeaderCallback += rect =>
			{
				EditorGUI.LabelField(rect, property.displayName);
			};



			reorderableList.drawElementCallback += (rect, index, isActive, isFocused) =>
			{
				float width = EditorGUIUtility.labelWidth;
				Rect tempRect = new Rect(rect);
				tempRect.y = rect.y;
				tempRect.width = Mathf.Min(rect.width / 2, 150);
				tempRect.height = EditorGUIUtility.singleLineHeight;
				var sp = reorderableList.serializedProperty;
				var element = sp.GetArrayElementAtIndex(index);


				Vector2 size = GUI.skin.label.CalcSize(new GUIContent("type"));
				EditorGUIUtility.labelWidth = size.x + 2;

				EditorGUI.PropertyField(tempRect, element.FindPropertyRelative("type"));
				//DrawLinkTidGUIField(property.name, element.FindPropertyRelative("type"), tempRect);
				tempRect.x += tempRect.width + 10;

				size = GUI.skin.label.CalcSize(new GUIContent("value"));
				EditorGUIUtility.labelWidth = size.x + 2;
				EditorGUI.PropertyField(tempRect, element.FindPropertyRelative("value"));

				EditorGUIUtility.labelWidth = width;
			};
		}
		reorderableList.DoLayoutList();
	}

	private void DrawLinkTidGUILayoutField(System.Type type, string fieldName, int index, object data)
	{

		if (linkedDataDic.ContainsKey(fieldName) == false)
		{
			EditorGUILayout.LongField(fieldName, type.GetValue<long>(fieldName, data));
			return;
		}

		var linkedData = linkedDataDic[fieldName];

		long tid = type.GetValue<long>(fieldName, data);

		for (int i = 0; i < linkedData.tidArray.Length; i++)
		{
			if (linkedData.tidArray[i] == tid)
			{
				index = i;
				break;
			}
		}

		index = EditorGUILayout.Popup(fieldName, index, linkedData.nameArray);
		type.SetValue(fieldName, data, linkedData.tidArray[index]);
	}

	private void DrawLinkTidGUIField(string name, SerializedProperty property, Rect rect)
	{
		if (linkedDataDic.ContainsKey(name) == false)
		{
			EditorGUI.PropertyField(rect, property);
			return;
		}
		int index = 0;
		var linkedData = linkedDataDic[name];
		if (property == null)
		{
			return;
		}
		long tid = property.longValue;

		for (int i = 0; i < linkedData.tidArray.Length; i++)
		{
			if (linkedData.tidArray[i] == tid)
			{
				index = i;
				break;
			}
		}

		index = EditorGUI.Popup(rect, index, linkedData.nameArray);
		property.longValue = linkedData.tidArray[index];

	}

	private void OnDisable()
	{
		fakeobj = null;
		System.GC.Collect();
	}

	void SavePrefab()
	{
		GameObject newObject = new GameObject(newName);


		GameObject ZoomInPivot = new GameObject("ZoomInPivot");
		GameObject HeadPivot = new GameObject("HeadPivot");

		ZoomInPivot.transform.SetParent(newObject.transform);
		HeadPivot.transform.SetParent(newObject.transform);

		GameObject copy = Instantiate(gameObject);
		copy.transform.SetParent(ZoomInPivot.transform);
		copy.name = copy.name.Replace("(Clone)", "");


		UnitAnimation animation = newObject.AddComponent<UnitAnimation>();
		animation.SetModel();

		string path = objectSavePath.Resources().Assets();
		string prefabPath = $"{path}/{newName}.prefab";


		PrefabUtility.SaveAsPrefabAssetAndConnect(newObject, prefabPath, InteractionMode.AutomatedAction);

		UnityEditor.SceneManagement.PrefabStageUtility.OpenPrefab(prefabPath);

		DestroyImmediate(newObject);
	}
	void InstantiatePrefabIntoScene()
	{

		if (EditorApplication.isPlaying)
		{
			if (UnitEditor.it != null)
			{
				UnitEditor.it.OnClickSpawn(currentData);
				//UnitEditor.it.editorToolUI.skillEffectEditorPanel.SetUnitAnimationState(UnitEditor.it.layerStates);
			}
			else
			{
				GameObject go = Instantiate(gameObject);
			}
		}
		else
		{
			GameObject go = Instantiate(gameObject);
		}
	}

	void OpenPrefab()
	{
		string path = AssetDatabase.GetAssetPath(gameObject);

		UnityEditor.SceneManagement.PrefabStageUtility.OpenPrefab(path);
	}
}


public static class CustomEditorHelper
{
	public static T GetValue<T>(this System.Type type, string field, object data)
	{
		return (T)type.GetField(field).GetValue(data);
	}
	public static void SetValue(this System.Type type, string field, object data, object value)
	{
		type.GetField(field).SetValue(data, value);
	}

}
