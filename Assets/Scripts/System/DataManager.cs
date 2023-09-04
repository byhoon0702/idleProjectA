using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using System.Runtime.InteropServices.WindowsRuntime;

/// <summary>
/// 데이터를 관리하는 객체
/// 현재는 DataSheet 클래스에서 infos 를 가지고오도록 되어있으나
/// 추후 infos 만 따로 추출 하여 저장 하도록 변경 할 것 
/// </summary>
public static class DataManager
{

	private static Dictionary<Type, object> container;

	private static List<IItemData<BaseData>> dataList;

	public static string path = "";

	public static bool isInit = false;
	public static void LoadAllJson()
	{
		if (isInit)
		{
			return;
		}
		isInit = true;
		container = new Dictionary<Type, object>();

		dataList = new List<IItemData<BaseData>>();

		TextAsset[] textAssets = Resources.LoadAll<TextAsset>("Data/Json");

		foreach (TextAsset file in textAssets)
		{
			if (file.name.Contains("DataSheet") == false)
			{
				continue;
			}
			//if (LoadFromBinary(file) == false)
			{
				if (LoadFromNonBinary(file) == false)
				{
					continue;
				}
			}
		}
	}

	public static bool LoadFromNonBinary(TextAsset file)
	{
		//Dictionary<string, object> jsonDict = (Dictionary<string, object>)Json.Deserialize(file.text);
		//if (jsonDict == null)
		//{
		//	return false;
		//}

		string name = file.name.Split('_')[0];
		//if (jsonDict.ContainsKey("typeName"))
		//{
		//	name = (string)jsonDict["typeName"];
		//}
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
			//VLog.LogWarning(file);
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
			dataList.Add(data as IItemData<BaseData>);
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

	public static T GetFromAll<T>(long tid) where T : BaseData
	{
		for (int i = 0; i < dataList.Count; i++)
		{
			var data = dataList[i].Get(tid);
			if (data != null)
			{
				return (T)data;
			}
		}

		return default;
	}

	public static List<T> GetFromAllHashTag<T>(string tag) where T : BaseData
	{
		List<T> datas = new List<T>();
		for (int i = 0; i < dataList.Count; i++)
		{
			var data = dataList[i].Get(tag);
			if (data != null && data is T)
			{
				datas.Add((T)data);
			}
		}

		return datas;
	}
	public static T GetFromHashTag<T>(string tag) where T : BaseData
	{
		for (int i = 0; i < dataList.Count; i++)
		{
			var data = dataList[i].Get(tag);
			if (data != null && data is T)
			{
				return (T)data;
			}
		}

		return null;
	}

}
