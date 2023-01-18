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
using DG.Tweening.Plugins.Core.PathCore;

public static class JsonConverter
{

	public static void FromCsv()
	{

	}

	public static void FromData()
	{

	}

	public static string ToCsv()
	{

		return "";
	}

	public static object ToData(bool isBinary, string filePath)
	{
		string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
		string typeName = fileName;
		if (isBinary)
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

					Type type = System.Type.GetType($"{typeName}, Assembly-CSharp");
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
		else
		{
			string jsonstring = File.ReadAllText(filePath);
			Dictionary<string, object> jb = (Dictionary<string, object>)Json.Deserialize(jsonstring);
			if (jb.ContainsKey("typeName"))
			{
				typeName = (string)jb["typeName"];
			}

			Type type = System.Type.GetType($"{typeName}, Assembly-CSharp");
			if (type == null)
			{
				return null;
			}

			var json = JsonUtility.FromJson(jsonstring, type);
			return json;
		}
	}
}

