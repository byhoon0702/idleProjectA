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
using JetBrains.Annotations;
using System.Linq;
using Codice.CM.Common;

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
		return CreateCSV(sd);
	}

	public static string FromJson(object jsonObject)
	{
		var targetObje = jsonObject;

		return CreateCSV(targetObje);
		//var fields = targetObje.GetType().GetFields();

		//StringBuilder sb = new StringBuilder();
		//foreach (var field in fields)
		//{
		//	object value = field.GetValue(targetObje);
		//	if (typeof(IList).IsAssignableFrom(value))
		//	{
		//		IList list = (IList)value;

		//		for (int i = 0; i < list.Count; i++)
		//		{
		//			object item = list[i];
		//			Type itemType = item.GetType();
		//			var listFields = itemType.GetFields();
		//			//컬럼 먼저
		//			FieldInfo tidField = itemType.GetField("tid");
		//			FieldInfo descField = itemType.GetField("description");

		//			if (i == 0)
		//			{
		//				sb.Append("tid");
		//				sb.Append(",description");

		//				//데이터 
		//				foreach (var listField in listFields)
		//				{
		//					if (listField.Name == "tid" || listField.Name == "description")
		//					{
		//						continue;
		//					}
		//					sb.Append($"{delimeter}{listField.Name}");
		//				}

		//			}
		//			sb.AppendLine();
		//			sb.Append(tidField.GetValue(item));
		//			sb.Append($"{delimeter}{descField.GetValue(item)}");
		//			foreach (var listField in listFields)
		//			{
		//				if (listField.Name == "tid" || listField.Name == "description")
		//				{
		//					continue;
		//				}
		//				sb.Append(delimeter);
		//				sb.Append(listField.GetValue(item).ToString());
		//			}
		//		}
		//	}
		//	else
		//	{
		//		sb.AppendLine(field.Name);
		//		sb.AppendLine(value.ToString());
		//	}

		//}
		//return sb.ToString();

	}
	private static void CreateTypeField(StringBuilder sb, Type itemType)
	{
		FieldInfo tidField = itemType.GetField("tid");
		FieldInfo descField = itemType.GetField("description");
		var listFields = itemType.GetFields();
		string typename = ConvertUtility.ConvertTypeToString(tidField.FieldType.Name);
		sb.Append($"{typename}:tid");
		typename = ConvertUtility.ConvertTypeToString(descField.FieldType.Name);
		sb.Append($",{typename}:description");

		foreach (var listField in listFields)
		{
			if (listField.Name == "tid" || listField.Name == "description")
			{
				continue;
			}
			typename = ConvertUtility.ConvertTypeToString(listField.FieldType.Name);
			if (typename == "list")
			{
				sb.Append($"{delimeter}{typename}:{listField.Name}");

				var sublistTypes = listField.FieldType.GetGenericArguments();

				if (sublistTypes[0].BaseType.Equals(typeof(object)))
				{
					Type sublistType = sublistTypes[0];
					var subListFields = sublistType.GetFields();
					for (int ii = 0; ii < subListFields.Length; ii++)
					{
						var subListField = subListFields[ii];
						sb.Append($"{delimeter}{typename}:{listField.Name}:{subListField.ReflectedType.Name}:{subListFields[ii].Name}");

					}
				}
				else
				{
					Type sublistType = sublistTypes[0];
					sb.Append($"{delimeter}{typename}:{listField.Name}:{sublistType.Name}");
				}
			}
			else
			{
				sb.Append($"{delimeter}{typename}:{listField.Name}");
			}
		}
	}

	private static string CreateCSV(object obj)
	{
		var fields = obj.GetType().GetFields();

		Dictionary<string, int> listIndent = new Dictionary<string, int>();
		StringBuilder sb = new StringBuilder();
		foreach (var field in fields)
		{
			object value = field.GetValue(obj);
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

					if (i == 0) //데이터 타입 정의 하는 부분
					{
						CreateTypeField(sb, itemType);
					}

					Dictionary<int, List<string>> indentedData = new Dictionary<int, List<string>>();
					//데이터 
					int indent = 0;
					sb.AppendLine();
					sb.Append(tidField.GetValue(item));
					indent++;
					sb.Append($"{delimeter}{descField.GetValue(item)}");
					indent++;
					foreach (var listField in listFields)
					{
						if (listField.Name == "tid" || listField.Name == "description")
						{
							continue;
						}
						indent++;
						string typename = ConvertUtility.ConvertTypeToString(listField.FieldType.Name);
						if (typename == "list")
						{
							sb.Append($"{delimeter}");
							Type[] sublistTypes = listField.FieldType.GetGenericArguments();

							object data = listField.GetValue(item);

							IList datalist = (IList)data;
							if (sublistTypes[0].BaseType.Equals(typeof(object)))
							{
								Type sublistType = sublistTypes[0];
								var subListFields = sublistType.GetFields();

								if (datalist.Count > 0)
								{
									indent++;
									for (int ii = 1; ii < datalist.Count; ii++)
									{
										for (int iii = 0; iii < subListFields.Length; iii++)
										{
											var subListField = subListFields[iii];
											sb.Append($"{delimeter}{subListField.GetValue(datalist[0])}");

											//따로 리스트로 저장 해둔 다음 스트링 다시 추가

											if (indentedData.ContainsKey(indent) == false)
											{
												indentedData.Add(indent, new List<string>());
											}
											indentedData[indent].Add($"{delimeter}{subListField.GetValue(datalist[ii])}");
										}
									}
								}
								else
								{
									indent++;
									sb.Append($"{delimeter}");
									for (int iii = 1; iii < datalist.Count; iii++)
									{
										if (indentedData.ContainsKey(indent) == false)
										{
											indentedData.Add(indent, new List<string>());
										}
										indentedData[indent].Add($"{delimeter}");
									}
								}
							}
							else
							{
								indent++;
								if (datalist.Count > 0)
								{
									sb.Append($"{delimeter}{datalist[0]}");
									//따로 리스트로 저장 해둔 다음 스트링 다시 추가
									for (int ii = 1; ii < datalist.Count; ii++)
									{
										if (indentedData.ContainsKey(indent) == false)
										{
											indentedData.Add(indent, new List<string>());
										}
										indentedData[indent].Add($"{datalist[ii]}");
									}
								}
								else
								{
									sb.Append($"{delimeter}");
								}
							}
						}
						else
						{
							sb.Append($"{delimeter}{listField.GetValue(item)}");
						}
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
			string[] values = Regex.Split(lines[i], SPLIT_RE);
			for (int ii = 0; ii < header.Length; ii++)
			{


				string value = values[ii];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

				FieldInfo info = elementType.GetField(header[ii]);
				object convertedValue = null;
				if (info.FieldType.BaseType == typeof(Enum))
				{
					Enum.TryParse(info.FieldType, value, out convertedValue);
				}
				else
				{
					convertedValue = Convert.ChangeType(value, info.FieldType);
				}

				info.SetValue(data, convertedValue);
			}

			list.Add(data);
		}
		infosField.SetValue(instance, list);
		return instance;
	}
}
