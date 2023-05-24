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


public static class JsonConverter
{

	public static void FromCsv()
	{

	}

#if UNITY_EDITOR
	public static void FromData(object data, string path)
	{
		string json = JsonUtility.ToJson(data, true);
		File.WriteAllText(path, json);
		AssetDatabase.Refresh();
	}
#endif
	public static string ToCsv()
	{

		return "";
	}

	public static object ToData(string filePath)
	{
		string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
		string typeName = fileName;
		try
		{
			using (FileStream fs = File.OpenRead(filePath))
			{

				using (BinaryReader sr = new BinaryReader(fs))
				{
					string jsonstring = sr.ReadString();
					Dictionary<string, object> jb = (Dictionary<string, object>)Json.Deserialize(jsonstring);

					if (jb.ContainsKey("typeName"))
					{
						typeName = (string)jb["typeName"];
					}

					Type type = typeName.GetAssemblyType();
					if (type == null)
					{
						sr.Close();
						return null;
					}
					var json = JsonUtility.FromJson(jsonstring, type);

					return json;
				}
			}
		}
		catch
		{
			string jsonstring = File.ReadAllText(filePath);
			Dictionary<string, object> jb = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonstring);
			if (jb.ContainsKey("typeName"))
			{
				typeName = (string)jb["typeName"];
			}

			Type type = typeName.GetAssemblyType();
			if (type == null)
			{
				return null;
			}

			var json = JsonUtility.FromJson(jsonstring, type);
			return json;
		}

	}
}

