using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

/// <summary>
/// 데이터를 관리하는 객체
/// 현재는 DataSheet 클래스에서 infos 를 가지고오도록 되어있으나
/// 추후 infos 만 따로 추출 하여 저장 하도록 변경 할 것 
/// </summary>
public class DataManager : MonoBehaviour
{
	private static DataManager instance;
	public static DataManager it => instance;

	private Dictionary<Type, object> container;

	public string path = "";
	private void Awake()
	{
		instance = this;
	}
	void Start()
	{
		LoadAllJson();
	}
	public void LoadAllJson()
	{
		if (path.IsNullOrEmpty())
		{
			path = "Resources/Json";
		}
		container = new Dictionary<Type, object>();
		string[] files = Directory.GetFiles($"{Application.dataPath}/AssetFolder/{path}");

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
					string json = br.ReadString();
					Dictionary<string, object> jsonDict = (Dictionary<string, object>)Json.Deserialize(json);
					string name = Path.GetFileNameWithoutExtension(file);
					if (jsonDict.ContainsKey("typeName"))
					{
						name = (string)jsonDict["typeName"];
					}
					System.Type t = System.Type.GetType($"{name}, Assembly-CSharp");

					if (t == null)
					{
						continue;
					}

					var dd = JsonUtility.FromJson(json, t);
					container.Add(t, dd);
				}
			}
		}
	}

	public T Get<T>()
	{
		if (container == null || container.Count == 0)
		{
			VLog.LogError("no data container");
			return default;
		}
		System.Type type = typeof(T);
		if (container.ContainsKey(type) == false)
		{
			return default;
		}

		return (T)container[type];
	}
}
