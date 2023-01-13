﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
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
		container = new Dictionary<Type, object>();

		TextAsset[] textAssets = Resources.LoadAll<TextAsset>("Json");

		foreach (TextAsset file in textAssets)
		{
			if (file.name.Contains("DataSheet") == false)
			{
				continue;
			}
			if (LoadFromBinary(file) == false)
			{
				if (LoadFromNonBinary(file) == false)
				{
					continue;
				}
			}

		}
	}

	private bool LoadFromNonBinary(TextAsset file)
	{

		Dictionary<string, object> jsonDict = (Dictionary<string, object>)Json.Deserialize(file.text);
		string name = file.name;
		if (jsonDict.ContainsKey("typeName"))
		{
			name = (string)jsonDict["typeName"];
		}
		System.Type t = System.Type.GetType($"{name}, Assembly-CSharp");

		if (t == null)
		{
			return false;
		}

		var dd = JsonUtility.FromJson(file.text, t);

		AddToContainer(t, dd);
		return true;
	}

	void AddToContainer(Type type, object data)
	{
		var fieldInfo = type.GetField("infos");

		if (container.ContainsKey(type) == false)
		{
			container.Add(type, data);
		}
		else
		{
			var fieldValue = fieldInfo.GetValue(container[type]);

			var addValue = fieldInfo.GetValue(data);

			if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType))
			{
				IList merge = (IList)Activator.CreateInstance(fieldInfo.FieldType);
				foreach (var item in fieldValue as IList)
				{
					merge.Add(item);
				}
				foreach (var item in addValue as IList)
				{
					merge.Add(item);
				}

				fieldInfo.SetValue(container[type], merge);
			}
		}
	}
	private bool LoadFromBinary(TextAsset file)
	{
		using (MemoryStream fs = new MemoryStream(file.bytes))
		{
			using (BinaryReader br = new BinaryReader(fs))
			{
				string json = br.ReadString();
				try
				{
					Dictionary<string, object> jsonDict = (Dictionary<string, object>)Json.Deserialize(json);
					string name = file.name;
					if (jsonDict.ContainsKey("typeName"))
					{
						name = (string)jsonDict["typeName"];
					}
					System.Type t = System.Type.GetType($"{name}, Assembly-CSharp");

					if (t == null)
					{
						return false;
					}

					var dd = JsonUtility.FromJson(json, t);
					AddToContainer(t, dd);
				}
				catch (Exception e)
				{
					Debug.LogError(file);
					return false;
				}
			}
		}
		return true;
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
