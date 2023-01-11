using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

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
		container = new Dictionary<Type, object>();
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
		return (T)container[type];
	}
}
