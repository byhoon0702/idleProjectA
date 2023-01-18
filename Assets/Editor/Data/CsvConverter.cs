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
using System.Text.RegularExpressions;

public static class CsvConverter
{
	private static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	private static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	private static char[] TRIM_CHARS = { '\"' };
	private const string assembly = "Assembly-CSharp";
	private const string delimeter = ",";
	public static string FromData(UnityEngine.Object unityObject)
	{
		var targetObje = unityObject;

		System.Reflection.FieldInfo info = targetObje.GetType().GetField("dataSheet");
		var sd = info.GetValue(targetObje);
		var fields = sd.GetType().GetFields();

		StringBuilder sb = new StringBuilder();
		foreach (var field in fields)
		{
			object value = field.GetValue(sd);
			if (typeof(IList).IsAssignableFrom(value))
			{
				IList list = (IList)value;

				for (int i = 0; i < list.Count; i++)
				{
					object item = list[i];
					Type itemType = item.GetType();
					var listFields = itemType.GetFields();
					//컬럼 먼저
					FieldInfo tidField = itemType.GetField("tid");
					FieldInfo descField = itemType.GetField("description");

					if (i == 0)
					{
						sb.Append("tid");
						sb.Append(",description");

						//데이터 
						foreach (var listField in listFields)
						{
							if (listField.Name == "tid" || listField.Name == "description")
							{
								continue;
							}
							sb.Append($"{delimeter}{listField.Name}");
						}

					}
					sb.AppendLine();
					sb.Append(tidField.GetValue(item));
					sb.Append($"{delimeter}{descField.GetValue(item)}");
					foreach (var listField in listFields)
					{
						if (listField.Name == "tid" || listField.Name == "description")
						{
							continue;
						}
						sb.Append(delimeter);
						sb.Append(listField.GetValue(item).ToString());
					}
				}
			}
			else
			{
				sb.AppendLine(field.Name);
				sb.AppendLine(value.ToString());
			}

		}
		return sb.ToString();
	}

	public static string FromJson(object jsonObject)
	{
		var targetObje = jsonObject;


		var fields = targetObje.GetType().GetFields();

		StringBuilder sb = new StringBuilder();
		foreach (var field in fields)
		{
			object value = field.GetValue(targetObje);
			if (typeof(IList).IsAssignableFrom(value))
			{
				IList list = (IList)value;

				for (int i = 0; i < list.Count; i++)
				{
					object item = list[i];
					Type itemType = item.GetType();
					var listFields = itemType.GetFields();
					//컬럼 먼저
					FieldInfo tidField = itemType.GetField("tid");
					FieldInfo descField = itemType.GetField("description");

					if (i == 0)
					{
						sb.Append("tid");
						sb.Append(",description");

						//데이터 
						foreach (var listField in listFields)
						{
							if (listField.Name == "tid" || listField.Name == "description")
							{
								continue;
							}
							sb.Append($"{delimeter}{listField.Name}");
						}

					}
					sb.AppendLine();
					sb.Append(tidField.GetValue(item));
					sb.Append($"{delimeter}{descField.GetValue(item)}");
					foreach (var listField in listFields)
					{
						if (listField.Name == "tid" || listField.Name == "description")
						{
							continue;
						}
						sb.Append(delimeter);
						sb.Append(listField.GetValue(item).ToString());
					}
				}
			}
			else
			{
				sb.AppendLine(field.Name);
				sb.AppendLine(value.ToString());
			}

		}
		return sb.ToString();

	}

	public static string ToJson()
	{

		return "";
	}
	public static object ToData(string filePath)
	{
		StreamReader streamReader = new StreamReader(filePath);
		string source = streamReader.ReadToEnd();
		streamReader.Close();

		string[] lines = Regex.Split(source, LINE_SPLIT_RE);

		System.Type type = System.Type.GetType($"{lines[1]}, {assembly}");
		var instance = Activator.CreateInstance(type);

		FieldInfo typeField = type.GetField("typeName");
		typeField.SetValue(instance, lines[1]);

		FieldInfo infosField = type.GetField("infos");
		Type elementType = null;
		IList list = null;
		if (typeof(IList).IsAssignableFrom(infosField.FieldType))
		{
			list = (IList)infosField.GetValue(instance);

			Type[] genericType = list.GetType().GetGenericArguments();
			if (genericType != null)
			{
				elementType = genericType[0];
			}
		}

		if (elementType == null)
		{
			Debug.LogError("Can not find type");
			return null;
		}

		string[] header = Regex.Split(lines[2], SPLIT_RE);

		for (int i = 3; i < lines.Length; i++)
		{
			object data = Activator.CreateInstance(elementType);
			string[] value = Regex.Split(lines[i], SPLIT_RE);
			for (int ii = 0; ii < header.Length; ii++)
			{
				FieldInfo info = elementType.GetField(header[ii]);
				object convertedValue = null;
				if (info.FieldType.BaseType == typeof(Enum))
				{
					Enum.TryParse(info.FieldType, value[ii], out convertedValue);
				}
				else
				{
					convertedValue = Convert.ChangeType(value[ii], info.FieldType);
				}

				info.SetValue(data, convertedValue);
			}

			list.Add(data);
		}
		infosField.SetValue(instance, list);
		return instance;
	}
}
