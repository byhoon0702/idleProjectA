
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class ResultCodeData : BaseData
{
	public string key;
	public PopAlertType alertType;
	public string title;
	public string content;
	public string okText;
	public string cancelText;

	public ResultCodeData Clone()
	{
		ResultCodeData resultCodeData = new ResultCodeData();

		resultCodeData.key = key;
		resultCodeData.alertType = alertType;
		resultCodeData.title = title;
		resultCodeData.content = content;

		return resultCodeData;
	}
}

[Serializable]
public class ResultCodeDataSheet : DataSheetBase<ResultCodeData>
{
	public ResultCodeData Get(long tid)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == tid)
			{
				return infos[i];
			}
		}
		return null;
	}

	public ResultCodeData Get(string _key)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].key == _key)
			{
				return infos[i];
			}
		}

		return null;
	}
}

#if UNITY_EDITOR
public static class EditorResultCodeUtility
{
	public static void GenerateResultCode(List<ResultCodeData> _infos)
	{
		string path = string.Concat(Application.dataPath, Path.DirectorySeparatorChar, $"Scripts/System/ResultCode.cs");
		List<Int64> tidList = new List<Int64>();
		List<string> keyList = new List<string>();

		using (StreamWriter writer = new StreamWriter(path, false))
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"//========AUTO GENERATED CODE======//");
			sb.AppendLine($"public enum ResultCode ");
			sb.AppendLine("{");
			for (Int32 i = 0 ; i < _infos.Count ; i++)
			{
				if (tidList.Contains(_infos[i].tid))
				{
					Debug.LogError($"TID 중복오류 tid: {_infos[i].tid}, {_infos[i].key}: {_infos[i].description}");
					continue;
				}
				tidList.Add(_infos[i].tid);

				if(keyList.Contains(_infos[i].key))
				{
					Debug.LogError($"Key 중복오류 tid: {_infos[i].tid}, {_infos[i].key}: {_infos[i].description}");
					continue;
				}
				keyList.Add(_infos[i].key);

				sb.AppendLine("/// <summary>");
				sb.AppendLine($"/// {_infos[i].description}");
				sb.AppendLine("/// </summary>");
				sb.AppendLine($"{_infos[i].key} = {_infos[i].tid},\n");
			}
			sb.AppendLine("}");
			writer.Write(sb.ToString());
		}

		UnityEditor.AssetDatabase.Refresh();
	}
}
#endif
