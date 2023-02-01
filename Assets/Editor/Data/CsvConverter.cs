using System;
using System.Collections.Generic;

using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections;

using System.Text.RegularExpressions;
using JetBrains.Annotations;

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


	}
	private static int CreateTypeField(StringBuilder sb, Type itemType)
	{
		int totalFieldCount = 0;
		FieldInfo tidField = itemType.GetField("tid");
		FieldInfo descField = itemType.GetField("description");
		var listFields = itemType.GetFields();
		string typename = ConvertUtility.ConvertTypeToString(tidField.FieldType.Name);
		totalFieldCount++;
		sb.Append($"{typename}:tid");

		typename = ConvertUtility.ConvertTypeToString(descField.FieldType.Name);
		totalFieldCount++;
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
				totalFieldCount++;
				if (sublistTypes[0].BaseType.Equals(typeof(object)))
				{
					Type sublistType = sublistTypes[0];
					var subListFields = sublistType.GetFields();
					for (int ii = 0; ii < subListFields.Length; ii++)
					{
						totalFieldCount++;
						var subListField = subListFields[ii];
						sb.Append($"{delimeter}{typename}:{listField.Name}:{subListField.ReflectedType.Name}:{subListFields[ii].Name}");
					}
				}
				else
				{
					totalFieldCount++;
					Type sublistType = sublistTypes[0];
					sb.Append($"{delimeter}{typename}:{listField.Name}:{sublistType.Name}");
				}
			}
			else if (typename == "object")
			{
				sb.Append($"{delimeter}{typename}:{listField.FieldType.Name}:{listField.Name}");
				var objectFields = listField.FieldType.GetFields();
				for (int i = 0; i < objectFields.Length; i++)
				{
					sb.Append($"{delimeter}{typename}:{listField.FieldType.Name}:{listField.Name}:{objectFields[i].Name}");
				}
			}
			else
			{
				totalFieldCount++;
				sb.Append($"{delimeter}{typename}:{listField.Name}");
			}
		}
		return totalFieldCount;
	}

	private static string CreateCSV(object obj)
	{
		var fields = obj.GetType().GetFields();

		Dictionary<string, int> listIndent = new Dictionary<string, int>();
		StringBuilder sb = new StringBuilder();
		foreach (var field in fields)
		{
			object value = field.GetValue(obj);
			if (typeof(IList).IsAssignableFrom(value) == false)
			{
				StringBuilder typeSb = new StringBuilder();
				//TypeName 필드 정의
				typeSb.AppendLine(field.Name);
				typeSb.AppendLine(value.ToString());
				sb.Append(typeSb.ToString());
			}
			else
			{

				//infos 필드
				IList list = (IList)value;

				object item = list[0];
				Type itemType = item.GetType();

				int totalFieldCount = CreateTypeField(sb, itemType);

				List<List<string>> dataStrings = new List<List<string>>();

				for (int i = 0; i < list.Count; i++)
				{
					object listitem = list[i];
					var listFields = itemType.GetFields();
					List<string> datastring = new List<string>();
					//컬럼 먼저
					FieldInfo tidField = itemType.GetField("tid");
					FieldInfo descField = itemType.GetField("description");

					Dictionary<int, List<string>> indentedData = new Dictionary<int, List<string>>();
					//데이터 
					int indent = 0;

					datastring.Add($"{tidField.GetValue(listitem)}");
					datastring.Add($"{descField.GetValue(listitem)}");


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
							object data = listField.GetValue(listitem);

							IList datalist = (IList)data;

							CreateListTable(datalist, datastring, listField.FieldType, indent, indentedData);
						}
						else if (typename == "object")
						{
							object data = listField.GetValue(listitem);

							CreateObjectField(data, datastring, listField.FieldType);
						}
						else
						{
							datastring.Add($"{listField.GetValue(listitem)}");
							//리스트 형태가 아닌 단일 데이터 필드
						}
					}
					dataStrings.Add(datastring);

					//하위 리스트에 있는 데이터를 스트링으로 변환
					List<string[]> lowerListData = new List<string[]>();
					foreach (var datad in indentedData)
					{
						string[] stringarray = new string[totalFieldCount];
						for (int ii = 0; ii < datad.Value.Count; ii++)
						{
							if (lowerListData.Count - 1 < ii)
							{
								lowerListData.Add(new string[totalFieldCount]);
							}
							lowerListData[ii][datad.Key] = datad.Value[ii];
						}
					}


					for (int ii = 0; ii < lowerListData.Count; ii++)
					{
						List<string> temp = new List<string>();
						for (int iii = 0; iii < lowerListData[ii].Length; iii++)
						{
							if (lowerListData[ii][iii] == null)
							{
								temp.Add("");
								continue;
							}
							temp.Add(lowerListData[ii][iii]);
						}
						dataStrings.Add(temp);
					}
				}


				//모든 데이터 스트링을 csv 스트링으로 변환
				for (int i = 0; i < dataStrings.Count; i++)
				{
					sb.AppendLine();
					sb.Append(dataStrings[i][0]);
					for (int ii = 1; ii < dataStrings[i].Count; ii++)
					{
						sb.Append(delimeter);
						sb.Append(dataStrings[i][ii]);
					}
				}
			}
		}
		return sb.ToString();
	}

	private static void CreateListTable(IList datalist, List<string> stringdata, Type fieldType, int indent, Dictionary<int, List<string>> indentedData)
	{
		Type[] sublistTypes = fieldType.GetGenericArguments();

		if (sublistTypes[0].BaseType.Equals(typeof(object)))
		{
			Type sublistType = sublistTypes[0];
			var subListFields = sublistType.GetFields();


			indent++;
			stringdata.Add("");
			if (datalist.Count > 0)
			{
				indent++;
				for (int i = 0; i < subListFields.Length; i++)
				{

					var subListField = subListFields[i];
					stringdata.Add($"{subListField.GetValue(datalist[0])}");
				}

				for (int i = 1; i < datalist.Count; i++)
				{

					for (int ii = 0; ii < subListFields.Length; ii++)
					{
						var subListField = subListFields[ii];

						indent++;
						if (indentedData.ContainsKey(indent) == false)
						{
							indentedData.Add(indent, new List<string>());
						}
						indentedData[indent].Add($"{subListField.GetValue(datalist[i])}");
					}
				}
			}
			else
			{

				for (int i = 0; i < subListFields.Length; i++)
				{
					indent++;
					stringdata.Add($"");
				}
			}
		}

		else
		{
			indent++;
			stringdata.Add("");
			if (datalist.Count > 0)
			{

				stringdata.Add($"{datalist[0]}");
				//따로 리스트로 저장 해둔 다음 스트링 다시 추가
				for (int i = 1; i < datalist.Count; i++)
				{
					if (indentedData.ContainsKey(indent) == false)
					{
						indentedData.Add(indent, new List<string>());
					}
					indentedData[indent].Add($"{datalist[i]}");
				}
			}
			else
			{
				indent++;
				stringdata.Add($"");
			}
		}
	}
	private static void CreateObjectField(object objectData, List<string> stringdata, Type fieldType)
	{
		FieldInfo[] objectFields = fieldType.GetFields();

		stringdata.Add("");
		for (int i = 0; i < objectFields.Length; i++)
		{
			stringdata.Add($"{objectFields[i].GetValue(objectData)}");
		}
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
		Dictionary<long, List<string>> datas = new Dictionary<long, List<string>>();
		Dictionary<long, Dictionary<string, List<string>>> dataDic = new Dictionary<long, Dictionary<string, List<string>>>();

		long currentID = 0;
		for (int i = 3; i < lines.Length; i++)
		{
			string[] values = Regex.Split(lines[i], SPLIT_RE);

			if (Int64.TryParse(values[0], out long result))
			{
				currentID = result;
				datas.Add(currentID, new List<string>());
				dataDic.Add(currentID, new Dictionary<string, List<string>>());
			}

			datas[currentID].Add(lines[i]);

			for (int ii = 0; ii < header.Length; ii++)
			{
				string headerName = header[ii];
				if (dataDic[currentID].ContainsKey(headerName) == false)
				{
					dataDic[currentID].Add(headerName, new List<string>());
				}
				if (ii < values.Length)
				{
					dataDic[currentID][headerName].Add(values[ii]);
				}
				else
				{
					dataDic[currentID][headerName].Add("");
				}

			}
		}

		foreach (var data in dataDic)
		{
			var dataInstance = Activator.CreateInstance(elementType);

			Dictionary<FieldInfo, List<string>> singleDataList = new Dictionary<FieldInfo, List<string>>();
			Dictionary<FieldInfo, Dictionary<string, List<string>>> objectDataList = new Dictionary<FieldInfo, Dictionary<string, List<string>>>();
			Dictionary<FieldInfo, List<string>> classDatas = new Dictionary<FieldInfo, List<string>>();
			foreach (var dic in data.Value)
			{
				string headerName = dic.Key;//header
				string[] headerSplit = headerName.Split(':');

				FieldInfo info = elementType.GetField(headerSplit[1]);
				if (headerSplit[0].Equals("list") == false)
				{
					//리스트가 아니면 데이터는 무조건 첫번째 배열에만 들어있다. 그게 아닌경우 데이터 쓰기에서 버그가 발생한것
					var value = dic.Value[0];
					value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
					if (headerSplit[0].Equals("object"))
					{
						if (headerSplit.Length > 3)
						{
							info = elementType.GetField(headerSplit[2]);
							var objedtFields = info.FieldType.GetField(headerSplit[3]);

							if (classDatas.ContainsKey(info) == false)
							{
								classDatas.Add(info, new List<string>());
							}
							classDatas[info].Add(value);

						}
					}
					else
					{
						object convertedValue = ConvertDataType(info.FieldType, value);

						info.SetValue(dataInstance, convertedValue);
					}
				}
				else
				{
					//데이터를 Set 하지 않고 한번더 필터링을 거친다
					//헤더 스플릿의 2번째 배열 까지가 리스트의 이름이기 때문에 3번째 부터 데이터 여부를 검사 한다.
					if (headerSplit.Length > 2)
					{
						if (headerSplit.Length > 3)
						{
							string className = headerSplit[2];
							string classVariableName = headerSplit[3];
							if (objectDataList.ContainsKey(info) == false)
							{
								objectDataList.Add(info, new Dictionary<string, List<string>>());
							}

							for (int i = 0; i < dic.Value.Count; i++)
							{
								if (dic.Value[i] == "")
								{
									continue;
								}
								if (objectDataList[info].ContainsKey(classVariableName) == false)
								{
									objectDataList[info].Add(classVariableName, new List<string>());
								}
								objectDataList[info][classVariableName].Add(dic.Value[i]);
							}
							//클래스, 구조체 등의 자료형을 가지는 리스트 
						}
						else
						{
							//1개의 변수를 가지는 리스트 
							if (singleDataList.ContainsKey(info) == false)
							{
								singleDataList.Add(info, new List<string>());
							}
							for (int i = 0; i < dic.Value.Count; i++)
							{
								if (dic.Value[i] == "")
								{
									continue;
								}
								singleDataList[info].Add(dic.Value[i]);
							}
						}
					}
				}
			}

			foreach (var classObject in classDatas)
			{
				var classInst = Activator.CreateInstance(classObject.Key.FieldType);

				var fields = classObject.Key.FieldType.GetFields();

				for (int i = 0; i < fields.Length; i++)
				{
					var dat = ConvertDataType(fields[i].FieldType, classObject.Value[i]);

					fields[i].SetValue(classInst, dat);
				}

				classObject.Key.SetValue(dataInstance, classInst);

			}

			if (objectDataList.Count > 0 || singleDataList.Count > 0)
			{
				foreach (var objectData in objectDataList)
				{

					var genericTypes = objectData.Key.FieldType.GenericTypeArguments[0];
					var subinst = Activator.CreateInstance(objectData.Key.FieldType);
					if (objectData.Value.Count == 0)
					{
						objectData.Key.SetValue(dataInstance, subinst);
						continue;
					}
					int index = 0;
					FieldInfo[] genericFields = genericTypes.GetFields();

					while (index < objectData.Value[genericFields[0].Name].Count)
					{
						var classinst = Activator.CreateInstance(genericTypes);
						for (int i = 0; i < genericFields.Length; i++)
						{
							var fieldInfo = genericFields[i];
							var ss = objectData.Value[fieldInfo.Name][index];
							var converted = ConvertDataType(fieldInfo.FieldType, ss);
							fieldInfo.SetValue(classinst, converted);
						}
						index++;
						((IList)subinst).Add(classinst);
					}
					objectData.Key.SetValue(dataInstance, subinst);

				}

				foreach (var singleData in singleDataList)
				{
					var genericTypes = singleData.Key.FieldType.GenericTypeArguments[0];
					System.Type listtype = singleData.Key.FieldType;
					var subinst = Activator.CreateInstance(listtype);
					IList lsist = (IList)subinst;
					for (int i = 0; i < singleData.Value.Count; i++)
					{
						var ssd = ConvertDataType(genericTypes, singleData.Value[i]);
						lsist.Add(ssd);
					}
					singleData.Key.SetValue(dataInstance, lsist);
				}
			}
			list.Add(dataInstance);
		}

		//리스트 및 객체를 가지는 테이블일 경우, FieldInfo 를 다르게 가지고 와야 한다.
		infosField.SetValue(instance, list);
		return instance;
	}

	static object ConvertDataType(Type fieldType, string value)
	{
		object convertedValue = null;
		if (fieldType.BaseType == typeof(Enum))
		{
			Enum.TryParse(fieldType, value, out convertedValue);
		}
		else
		{
			convertedValue = Convert.ChangeType(value, fieldType);
		}
		return convertedValue;
	}
}
