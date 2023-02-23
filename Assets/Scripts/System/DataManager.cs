using System;
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
public static class DataManager
{
	private static SkillMeta skillMeta;
	private static Dictionary<Type, object> container;

	public static SkillMeta SkillMeta => skillMeta;
	public static string path = "";

	public static void LoadAllJson()
	{
		container = new Dictionary<Type, object>();

		TextAsset[] textAssets = Resources.LoadAll<TextAsset>("Data/Json");

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

		skillMeta = new SkillMeta();
		skillMeta.LoadData();
	}

	private static bool LoadFromNonBinary(TextAsset file)
	{
		Dictionary<string, object> jsonDict = (Dictionary<string, object>)Json.Deserialize(file.text);
		if (jsonDict == null)
		{
			return false;
		}

		string name = file.name;
		if (jsonDict.ContainsKey("typeName"))
		{
			name = (string)jsonDict["typeName"];
		}
		System.Type t = name.GetAssemblyType();

		if (t == null)
		{
			return false;
		}

		var json = JsonUtility.FromJson(file.text, t);

		AddToContainer(t, json);
		return true;
	}
	private static bool LoadFromBinary(TextAsset file)
	{
		try
		{
			using (MemoryStream fs = new MemoryStream(file.bytes))
			{
				using (BinaryReader br = new BinaryReader(fs))
				{
					string jsonString = "";
					Dictionary<string, object> jsonDict = new Dictionary<string, object>();

					jsonString = br.ReadString();
					jsonDict = (Dictionary<string, object>)Json.Deserialize(jsonString);

					string name = file.name;
					if (jsonDict.ContainsKey("typeName"))
					{
						name = (string)jsonDict["typeName"];
					}
					System.Type t = name.GetAssemblyType();

					if (t == null)
					{
						return false;
					}

					var json = JsonUtility.FromJson(jsonString, t);
					AddToContainer(t, json);
				}

			}
		}
		catch (Exception e)
		{
			VLog.LogWarning(file);
			return false;
		}
		return true;
	}
	static void AddToContainer(Type type, object data)
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


	public static T Get<T>()
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
